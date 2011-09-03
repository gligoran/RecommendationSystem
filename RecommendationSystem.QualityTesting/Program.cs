using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using RecommendationSystem.Data;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Foundation.RatingAggregation;
using RecommendationSystem.Knn.Foundation.Recommendations.RecommendationGeneration;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.QualityTesting.Testers;
using RecommendationSystem.SimpleKnn.Models;
using RecommendationSystem.SimpleKnn.Recommendations;
using RecommendationSystem.SimpleKnn.Similarity;
using RecommendationSystem.SimpleKnn.Training;
using RecommendationSystem.SimpleKnn.Users;
using RecommendationSystem.SimpleSvd.Basic;
using RecommendationSystem.SimpleSvd.Basic.Recommendations;
using RecommendationSystem.SimpleSvd.Bias;
using RecommendationSystem.SimpleSvd.Bias.Recommendations;
using RecommendationSystem.Svd.Foundation.Basic.Models;
using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Training;

namespace RecommendationSystem.QualityTesting
{
    public static class Program
    {
        private static readonly Stopwatch timer = new Stopwatch();

        public static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Please enter testing arguments.");
                    return;
                }

                var argList = new List<string>(args);

                var mode = argList[argList.IndexOf("-mode") + 1].ToLower();
                Console.WriteLine("Performing {0} tests...", mode.ToUpper());

                List<IArtist> artists;
                List<IUser> trainUsers;
                List<IRating> trainRatings;
                List<IUser> testUsers;
                List<IRating> testRatings;
                LoadData(out trainUsers, out trainRatings, out testUsers, out testRatings, out artists);

                switch (mode)
                {
                    case "simple":

                        #region Simple
                        var simpleTester = new NaiveTester(trainUsers, artists, trainRatings, testUsers);
                        simpleTester.Test();
                        break;
                        #endregion

                    case "knn":

                        #region kNN
                        var ks = argList[argList.IndexOf("-k") + 1].ToLower().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                        var numberOfTests = int.Parse(argList[argList.IndexOf("-t") + 1].ToLower());
                        var performNoContentKnnTests = argList.IndexOf("-noco") >= 0;
                        var performContentKnnTests = argList.IndexOf("-co") >= 0;

                        var sims = new List<ISimilarityEstimator<ISimpleKnnUser>>();
                        var argSims = argList[argList.IndexOf("-sim") + 1].ToLower().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var argSim in argSims)
                        {
                            switch (argSim)
                            {
                                case "pse":
                                    sims.Add(new PearsonSimilarityEstimator());
                                    break;
                                case "cse":
                                    sims.Add(new CosineSimilarityEstimator());
                                    break;
                                case "upse":
                                    sims.Add(new UnionPearsonSimilarityEstimator());
                                    break;
                                case "ucse":
                                    sims.Add(new UnionCosineSimilarityEstimator());
                                    break;
                            }
                        }

                        var rgs = new List<IRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser>>();
                        var argRgs = argList[argList.IndexOf("-rg") + 1].ToLower().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var argRg in argRgs)
                        {
                            switch (argRg)
                            {
                                case "sara":
                                    rgs.Add(new RatingAggregationRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser>(new SimpleAverageRatingAggregator<ISimpleKnnUser>()));
                                    break;
                                case "wsra":
                                    rgs.Add(new RatingAggregationRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser>(new WeightedSumRatingAggregator<ISimpleKnnUser>()));
                                    break;
                                case "awsra":
                                    rgs.Add(new RatingAggregationRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser>(new AdjustedWeightedSumRatingAggregator<ISimpleKnnUser>()));
                                    break;
                                case "frg":
                                    rgs.Add(new FifthsSimpleRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser>());
                                    break;
                                case "edrg":
                                    rgs.Add(new EqualDescentSimpleRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser>());
                                    break;
                            }
                        }

                        var knnTrainer = new SimpleKnnTrainer();
                        timer.Restart();
                        var knnModel = knnTrainer.TrainModel(trainUsers, artists, trainRatings);
                        timer.Stop();
                        Console.WriteLine("Model trained in {0}ms.", timer.ElapsedMilliseconds);

                        foreach (var k in ks)
                        {
                            foreach (var sim in sims)
                            {
                                foreach (var rg in rgs)
                                {
                                    if (performNoContentKnnTests)
                                    {
                                        var knnTester = new KnnTester<SimpleKnnRecommender>
                                                        {
                                                            K = k,
                                                            Sim = sim,
                                                            Rg = rg,
                                                            TestUsers = testUsers,
                                                            SimpleKnnModel = knnModel,
                                                            Trainer = knnTrainer,
                                                            Artists = artists,
                                                            NumberOfTests = numberOfTests
                                                        };
                                        knnTester.Test();
                                    }

                                    if (performContentKnnTests)
                                    {
                                        var contentKnnTester = new KnnTester<ContentSimpleKnnRecommender>
                                                               {
                                                                   K = k,
                                                                   Sim = sim,
                                                                   Rg = rg,
                                                                   TestUsers = testUsers,
                                                                   SimpleKnnModel = knnModel,
                                                                   Trainer = knnTrainer,
                                                                   Artists = artists,
                                                                   NumberOfTests = numberOfTests
                                                               };
                                        contentKnnTester.Test();
                                    }
                                }
                            }
                        }
                        break;
                        #endregion

                    case "svd-train":
                    case "mf-train":

                        #region SVD/MF Training
                        var svdFs = argList[argList.IndexOf("-f") + 1].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                        var svdLrs = argList[argList.IndexOf("-lr") + 1].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(s => float.Parse(s, CultureInfo.InvariantCulture)).ToList();
                        var svdKs = argList[argList.IndexOf("-k") + 1].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(s => float.Parse(s, CultureInfo.InvariantCulture)).ToList();
                        var svdRis = argList[argList.IndexOf("-ri") + 1].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(s => float.Parse(s, CultureInfo.InvariantCulture)).ToList();
                        var svdEs = argList[argList.IndexOf("-e") + 1].Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                        var svdBbs = argList[argList.IndexOf("-bb") + 1].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                        var svdTypes = argList[argList.IndexOf("-type") + 1].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();

                        var minEpoch = svdEs[0];
                        var maxEpoch = svdEs[1];
                        var svdBasic = svdTypes.Contains("basic");
                        var svdBias = svdTypes.Contains("bias");

                        foreach (var svdF in svdFs)
                        {
                            foreach (var svdLr in svdLrs)
                            {
                                foreach (var svdK in svdKs)
                                {
                                    foreach (var svdRi in svdRis)
                                    {
                                        foreach (var svdBb in svdBbs)
                                        {
                                            var trainingParameters = new TrainingParameters(svdF, svdLr, svdK, svdRi, minEpoch, maxEpoch, svdBb);
                                            var filename = string.Format("F{0}-LR{1}-K{2}-RI{3}-E{4}-{5}-BB{6}", svdF, svdLr, svdK, svdRi, minEpoch, maxEpoch, svdBb);

                                            if (svdBasic)
                                            {
                                                var rs = new BasicSvdRecommendationSystem();
                                                var model = rs.Trainer.TrainModel(trainUsers, artists, trainRatings, trainingParameters);
                                                rs.SaveModel(string.Format(@"D:\Dataset\models\basicSvd-{0}.rs", filename), model);
                                            }

                                            if (svdBias)
                                            {
                                                var rs = new BiasSvdRecommendationSystem();
                                                var model = rs.Trainer.TrainModel(trainUsers, artists, trainRatings, trainingParameters);
                                                rs.SaveModel(string.Format(@"D:\Dataset\models\biasSvd-{0}.rs", filename), model);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;
                        #endregion

                    case "svd":
                    case "mf":

                        #region SVD/MF Prediction
                        var models = Directory.GetFiles(@"D:\Dataset\models\", "*.rs", SearchOption.TopDirectoryOnly);
                        for (var i = 0; i < models.Length; i++)
                        {
                            var modelName = Path.GetFileName(models[i]);
                            modelName = modelName != null ? modelName.Remove(modelName.Length - 3) : "unnamed model";
                            Console.WriteLine("{0}) {1}", i + 1, modelName);
                        }

                        Console.WriteLine("Enter numbers of models for testing (separate with comma, add BB for bias bins usage):");
                        string line;
                        while ((line = Console.ReadLine()) == null)
                        {}
                        var selectedModels = new List<int>();
                        var useBiasBins = new List<bool>();
                        if (line == "all")
                        {
                            for (var i = 0; i < models.Length; i++)
                            {
                                selectedModels.Add(i);
                                selectedModels.Add(i);
                                useBiasBins.Add(true);
                                useBiasBins.Add(false);
                            }
                        }
                        else
                        {
                            var parts = line.Split(new[] {' ', ','});

                            foreach (var part in parts)
                            {
                                selectedModels.Add(int.Parse(part.Replace("BB", string.Empty)) - 1);
                                useBiasBins.Add(part.Contains("BB"));
                            }
                        }

                        for (var i = 0; i < selectedModels.Count; i++)
                        {
                            var selectedModel = selectedModels[i];
                            if (selectedModel >= models.Length)
                                continue;

                            var modelFile = models[selectedModel];

                            var testName = Path.GetFileName(modelFile);
                            testName = testName != null ? testName.Remove(testName.Length - 3) : "SvdTest";

                            if (useBiasBins[i])
                                testName += "-WithBB";

                            if (modelFile.Contains("basic"))
                            {
                                var basicSvdRecommender = new BasicSvdRecommender(useBiasBins[i]);
                                var rs = new BasicSvdRecommendationSystem(basicSvdRecommender);
                                var svdModel = rs.LoadModel(modelFile);
                                var basicSvdTester = new SvdTester<IBasicSvdModel>(testName, rs, svdModel, testUsers, testRatings, artists);
                                basicSvdTester.Test();
                            }
                            else if (models[selectedModel - 1].Contains("bias"))
                            {
                                var biasSvdRecommender = new BiasSvdRecommender(useBiasBins[i]);
                                var rs = new BiasSvdRecommendationSystem(biasSvdRecommender);
                                var svdModel = rs.LoadModel(modelFile);
                                var biasSvdTester = new SvdTester<IBiasSvdModel>(testName, rs, svdModel, testUsers, testRatings, artists);
                                biasSvdTester.Test();
                            }
                        }

                        break;
                        #endregion
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}{1}{1}{2}", e, Environment.NewLine, e.Message);
            }

            if (args.Where(arg => arg.ToLower() == "-wait").Count() != 0)
                Console.ReadLine();
        }

        #region LoadData
        private static void LoadData(out List<IUser> trainUsers, out List<IRating> trainRatings, out List<IUser> testUsers, out List<IRating> testRatings, out List<IArtist> artists)
        {
            Console.WriteLine("Loading data...");
            timer.Restart();
            trainRatings = RatingProvider.Load(DataFiles.TrainEqualFerquencyFiveScaleRatings);
            trainUsers = UserProvider.Load(DataFiles.TrainUsers);
            artists = ArtistProvider.Load(DataFiles.Artists);
            trainUsers.PopulateWithRatings(trainRatings);

            testRatings = RatingProvider.Load(DataFiles.TestEqualFerquencyFiveScaleRatings);
            testUsers = UserProvider.Load(DataFiles.TestUsers);
            testUsers.PopulateWithRatings(testRatings);
            timer.Stop();
            Console.WriteLine("Data loaded ({0}ms).", timer.ElapsedMilliseconds);
        }
        #endregion
    }
}