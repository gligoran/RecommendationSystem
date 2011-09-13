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
using RecommendationSystem.SimpleSvd;
using RecommendationSystem.SimpleSvd.Bias.Recommendations;
using RecommendationSystem.SimpleSvd.Bias.Training;
using RecommendationSystem.SimpleSvd.Recommendations;
using RecommendationSystem.SimpleSvd.Training;
using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Bias.Prediction;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Prediction;
using RecommendationSystem.Svd.Foundation.Training;
using RecommendationSystem.SvdBoostedKnn;
using RecommendationSystem.SvdBoostedKnn.Bias.Models;
using RecommendationSystem.SvdBoostedKnn.Bias.Training;
using RecommendationSystem.SvdBoostedKnn.Models;
using RecommendationSystem.SvdBoostedKnn.Recommendations;
using RecommendationSystem.SvdBoostedKnn.Similarity;
using RecommendationSystem.SvdBoostedKnn.Training;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.QualityTesting
{
    public static class Program
    {
        private static readonly Stopwatch timer = new Stopwatch();

        public static void Main(string[] args)
        {
#if !DEBUG
            try
            {
#endif
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter testing arguments.");
                return;
            }

            var argList = args.Select(arg => arg.ToLower()).ToList();

            var mode = argList[argList.IndexOf("-mode") + 1].ToLower();
            Console.WriteLine("Performing {0} tests...", mode.ToUpper());

            List<IArtist> artists;
            List<IUser> trainUsers;
            List<IRating> trainRatings;
            List<IUser> testUsers;
            List<IRating> testRatings;
            LoadData(out trainUsers, out trainRatings, out testUsers, out testRatings, out artists);

            List<int> selectedModels;
            List<string> models;

            int numberOfTests;
            List<int> ks;
            bool performNoContentKnnTests;
            bool performContentKnnTests;

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
                    ks = argList[argList.IndexOf("-k") + 1].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                    numberOfTests = int.Parse(argList[argList.IndexOf("-t") + 1]);
                    performNoContentKnnTests = argList.IndexOf("-noco") >= 0;
                    performContentKnnTests = argList.IndexOf("-co") >= 0;

                    var sims = new List<ISimilarityEstimator<ISimpleKnnUser>>();
                    foreach (var argSim in argList[argList.IndexOf("-sim") + 1].ToLower().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
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
                    foreach (var argRg in argList[argList.IndexOf("-rg") + 1].ToLower().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
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
                                rgs.Add(new LinearDescentSimpleRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser>());
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
                                        var filename = string.Format(CultureInfo.InvariantCulture, "F{0}-LR{1}-K{2}-RI{3}-E{4}-{5}", svdF, svdLr, svdK, svdRi, minEpoch, maxEpoch);

                                        if (svdBasic)
                                        {
                                            var basicSimpleSvdBiasBinsTrainer = new SimpleSvdBiasBinsTrainer();
                                            var model = basicSimpleSvdBiasBinsTrainer.TrainModel(trainUsers, artists, trainRatings, trainingParameters);
                                            basicSimpleSvdBiasBinsTrainer.SaveModel(string.Format(CultureInfo.InvariantCulture, @"D:\Dataset\models\basicSvd-{0}-BB{1}.rs", filename, svdBb), model);

                                            new SimpleSvdTrainer().SaveModel(string.Format(CultureInfo.InvariantCulture, @"D:\Dataset\models\basicSvd-{0}.rs", filename), model);
                                        }

                                        if (svdBias)
                                        {
                                            var biasSimpleSvdBiasBinsTrainer = new BiasSimpleSvdBiasBinsTrainer();
                                            var model = biasSimpleSvdBiasBinsTrainer.TrainModel(trainUsers, artists, trainRatings, trainingParameters);
                                            biasSimpleSvdBiasBinsTrainer.SaveModel(string.Format(CultureInfo.InvariantCulture, @"D:\Dataset\models\biasSvd-{0}-BB{1}.rs", filename, svdBb), model);

                                            new BiasSimpleSvdTrainer().SaveModel(string.Format(CultureInfo.InvariantCulture, @"D:\Dataset\models\biasSvd-{0}.rs", filename), model);
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
                    selectedModels = new List<int>();
                    models = Directory.GetFiles(@"D:\Dataset\models\", "*svd*.rs", SearchOption.TopDirectoryOnly).ToList();
                    if (argList.Contains("-all"))
                        selectedModels.AddRange(models.Select((t, i) => i));
                    else
                    {
                        for (var i = 0; i < models.Count; i++)
                        {
                            var modelName = Path.GetFileName(models[i]);
                            modelName = modelName != null ? modelName.Remove(modelName.Length - 3) : "UnnamedModel";
                            Console.WriteLine("{0}) {1}", i + 1, modelName);
                        }

                        Console.WriteLine("Enter numbers of models for testing (separate with comma):");
                        string line;
                        while ((line = Console.ReadLine()) == null)
                        {}

                        selectedModels.AddRange(line == "all" ? models.Select((t, i) => i) : line.Split(new[] {' ', ','}).Select(part => int.Parse(part) - 1));
                    }

                    for (var i = 0; i < selectedModels.Count; i++)
                    {
                        var selectedModel = selectedModels[i];
                        if (selectedModel >= models.Count)
                            continue;

                        var modelFile = models[selectedModel];

                        var testName = Path.GetFileName(modelFile);
                        testName = testName != null ? testName.Remove(testName.Length - 3) : "SvdTest";

                        if (modelFile.Contains("basic"))
                        {
                            if (modelFile.Contains("BB"))
                            {
                                var rs = new SimpleSvdRecommendationSystem<ISvdBiasBinsModel>(new SimpleSvdBiasBinsTrainer(), new SimpleSvdBiasBinsRecommender());
                                var model = new SvdBiasBinsModel();
                                rs.Recommender.LoadModel(model, modelFile);
                                new SvdTester<ISvdBiasBinsModel>(testName, rs, model, testUsers, testRatings, artists).Test();
                            }
                            else
                            {
                                var rs = new SimpleSvdRecommendationSystem<ISvdModel>(new SimpleSvdTrainer(), new SimpleSvdRecommender());
                                var model = new SvdModel();
                                rs.Recommender.LoadModel(model, modelFile);
                                new SvdTester<ISvdModel>(testName, rs, model, testUsers, testRatings, artists).Test();
                            }
                        }
                        else if (modelFile.Contains("bias"))
                        {
                            if (modelFile.Contains("BB"))
                            {
                                var rs = new SimpleSvdRecommendationSystem<IBiasSvdBiasBinsModel>(new BiasSimpleSvdBiasBinsTrainer(), new BiasSimpleSvdBiasBinsRecommender());
                                var model = new BiasSvdBiasBinsModel();
                                rs.Recommender.LoadModel(model, modelFile);
                                new SvdTester<IBiasSvdBiasBinsModel>(testName, rs, model, testUsers, testRatings, artists).Test();
                            }
                            else
                            {
                                var rs = new SimpleSvdRecommendationSystem<IBiasSvdModel>(new BiasSimpleSvdTrainer(), new BiasSimpleSvdRecommender());
                                var model = new BiasSvdModel();
                                rs.Recommender.LoadModel(model, modelFile);
                                new SvdTester<IBiasSvdModel>(testName, rs, model, testUsers, testRatings, artists).Test();
                            }
                        }
                    }

                    break;
                    #endregion

                case "sbk":

                    #region SVD boosted kNN

                    #region CLI argument parsin
                    numberOfTests = int.Parse(argList[argList.IndexOf("-t") + 1]);

                    //TODO: implement support
                    performNoContentKnnTests = argList.IndexOf("-noco") >= 0;
                    performContentKnnTests = argList.IndexOf("-co") >= 0;

                    ks = argList[argList.IndexOf("-k") + 1].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    var sbkSims = new List<ISimilarityEstimator<ISvdBoostedKnnUser>>();
                    foreach (var argSim in argList[argList.IndexOf("-sim") + 1].ToLower().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        switch (argSim)
                        {
                            case "pse":
                                sbkSims.Add(new PearsonSvdBoostedKnnSimilarityEstimator());
                                break;
                            case "cse":
                                sbkSims.Add(new CosineSvdBoostedKnnSimilarityEstimator());
                                break;
                                //TODO: implement
                                //case "upse":
                                //    sims.Add(new UnionPearsonSimilarityEstimator());
                                //    break;
                                //case "ucse":
                                //    sims.Add(new UnionCosineSimilarityEstimator());
                                //    break;
                        }
                    }

                    var sbkRgs = new List<IRecommendationGenerator<ISvdBoostedKnnModel, ISvdBoostedKnnUser>>();
                    foreach (var argRg in argList[argList.IndexOf("-rg") + 1].ToLower().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        switch (argRg)
                        {
                            case "sara":
                                sbkRgs.Add(new RatingAggregationRecommendationGenerator<ISvdBoostedKnnModel, ISvdBoostedKnnUser>(new SimpleAverageRatingAggregator<ISvdBoostedKnnUser>()));
                                break;
                            case "wsra":
                                sbkRgs.Add(new RatingAggregationRecommendationGenerator<ISvdBoostedKnnModel, ISvdBoostedKnnUser>(new WeightedSumRatingAggregator<ISvdBoostedKnnUser>()));
                                break;
                            case "awsra":
                                sbkRgs.Add(new RatingAggregationRecommendationGenerator<ISvdBoostedKnnModel, ISvdBoostedKnnUser>(new AdjustedWeightedSumRatingAggregator<ISvdBoostedKnnUser>()));
                                break;
                            case "frg":
                                sbkRgs.Add(new FifthsSimpleRecommendationGenerator<ISvdBoostedKnnModel, ISvdBoostedKnnUser>());
                                break;
                            case "edrg":
                                sbkRgs.Add(new LinearDescentSimpleRecommendationGenerator<ISvdBoostedKnnModel, ISvdBoostedKnnUser>());
                                break;
                        }
                    }
                    #endregion

                    #region Model selection
                    selectedModels = new List<int>();
                    models = Directory.GetFiles(@"D:\Dataset\models\", "*svd*.rs", SearchOption.TopDirectoryOnly).Where(m => !m.Contains("BB")).ToList();
                    if (argList.Contains("-all"))
                        selectedModels.AddRange(models.Select((t, i) => i));
                    else
                    {
                        for (var i = 0; i < models.Count; i++)
                        {
                            var modelName = Path.GetFileName(models[i]);
                            modelName = modelName != null ? modelName.Remove(modelName.Length - 3) : "UnnamedModel";
                            Console.WriteLine("{0}) {1}", i + 1, modelName);
                        }

                        Console.WriteLine("Enter numbers of models for testing (separate with comma):");
                        string line;
                        while ((line = Console.ReadLine()) == null)
                        {}

                        selectedModels.AddRange(line == "all" ? models.Select((t, i) => i) : line.Split(new[] {' ', ','}).Select(part => int.Parse(part) - 1));
                    }
                    #endregion

                    #region Testing
                    foreach (var k in ks)
                    {
                        foreach (var sbkSim in sbkSims)
                        {
                            foreach (var sbkRg in sbkRgs)
                            {
                                for (var i = 0; i < selectedModels.Count; i++)
                                {
                                    var selectedModel = selectedModels[i];
                                    if (selectedModel >= models.Count)
                                        continue;

                                    var modelFile = models[selectedModel];
                                    var testName = Path.GetFileName(modelFile);
                                    testName = testName != null ? testName.Remove(testName.Length - 3) : "SvdBoostedKnnTest";

                                    if (modelFile.Contains("basic"))
                                    {
                                        if (performNoContentKnnTests)
                                        {
                                            var sbkRs = new SvdBoostedKnnRecommendationSystem<ISvdBoostedKnnModel>(new SvdBoostedKnnSvdTrainer(),
                                                                                                                   new KnnTrainerForSvdModels(),
                                                                                                                   new SvdBoostedKnnRecommender<ISvdBoostedKnnModel>(sbkSim,
                                                                                                                                                                     sbkRg,
                                                                                                                                                                     new NewUserFeatureGenerator(),
                                                                                                                                                                     k));

                                            var sbkModel = sbkRs.KnnTrainerForSvdModels.TrainKnnModel(modelFile, trainUsers);

                                            testName = string.Format("SBK-(k{0}-{1}-{2}-{3}-T{4})-{5}", k, sbkSim, sbkRg, sbkRs.Recommender, numberOfTests, testName);
                                            var sbkTester = new SvdBoostedKnnTester<ISvdBoostedKnnModel>(testName, sbkRs, sbkModel, testUsers, testRatings, artists, numberOfTests);
                                            sbkTester.Test();
                                        }

                                        if (performContentKnnTests)
                                        {
                                            var sbkRs = new SvdBoostedKnnRecommendationSystem<ISvdBoostedKnnModel>(new SvdBoostedKnnSvdTrainer(),
                                                                                                                   new KnnTrainerForSvdModels(),
                                                                                                                   new ContentSvdBoostedKnnRecommender<ISvdBoostedKnnModel>(sbkSim,
                                                                                                                                                                            sbkRg,
                                                                                                                                                                            new NewUserFeatureGenerator(),
                                                                                                                                                                            new ContentSimilarityEstimator(),
                                                                                                                                                                            k));

                                            var sbkModel = sbkRs.KnnTrainerForSvdModels.TrainKnnModel(modelFile, trainUsers);

                                            testName = string.Format("SBK-(k{0}-{1}-{2}-{3}-T{4})-{5}", k, sbkSim, sbkRg, sbkRs.Recommender, numberOfTests, testName);
                                            var sbkTester = new SvdBoostedKnnTester<ISvdBoostedKnnModel>(testName, sbkRs, sbkModel, testUsers, testRatings, artists, numberOfTests);
                                            sbkTester.Test();
                                        }
                                    }
                                    else if (modelFile.Contains("bias"))
                                    {
                                        if (performNoContentKnnTests)
                                        {
                                            var sbkRs = new SvdBoostedKnnRecommendationSystem<IBiasSvdBoostedKnnModel>(new BiasSvdBoostedKnnSvdTrainer(),
                                                                                                                       new BiasKnnTrainerForSvdModels(),
                                                                                                                       new SvdBoostedKnnRecommender<IBiasSvdBoostedKnnModel>(sbkSim,
                                                                                                                                                                             sbkRg,
                                                                                                                                                                             new BiasNewUserFeatureGenerator(),
                                                                                                                                                                             k));

                                            var sbkModel = sbkRs.KnnTrainerForSvdModels.TrainKnnModel(modelFile, trainUsers);

                                            testName = string.Format("SBK-(k{0}-{1}-{2}-{3}-T{4})-{5}", k, sbkSim, sbkRg, sbkRs.Recommender, numberOfTests, testName);
                                            var sbkTester = new SvdBoostedKnnTester<IBiasSvdBoostedKnnModel>(testName, sbkRs, sbkModel, testUsers, testRatings, artists, numberOfTests);
                                            sbkTester.Test();
                                        }

                                        if (performContentKnnTests)
                                        {
                                            var sbkRs = new SvdBoostedKnnRecommendationSystem<IBiasSvdBoostedKnnModel>(new BiasSvdBoostedKnnSvdTrainer(),
                                                                                                                       new BiasKnnTrainerForSvdModels(),
                                                                                                                       new ContentSvdBoostedKnnRecommender<IBiasSvdBoostedKnnModel>(sbkSim,
                                                                                                                                                                                    sbkRg,
                                                                                                                                                                                    new BiasNewUserFeatureGenerator(),
                                                                                                                                                                                    new ContentSimilarityEstimator(),
                                                                                                                                                                                    k));

                                            var sbkModel = sbkRs.KnnTrainerForSvdModels.TrainKnnModel(modelFile, trainUsers);

                                            testName = string.Format("SBK-(k{0}-{1}-{2}-{3}-T{4})-{5}", k, sbkSim, sbkRg, sbkRs.Recommender, numberOfTests, testName);
                                            var sbkTester = new SvdBoostedKnnTester<IBiasSvdBoostedKnnModel>(testName, sbkRs, sbkModel, testUsers, testRatings, artists, numberOfTests);
                                            sbkTester.Test();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    break;
                    #endregion
            }
#if !DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}{1}{1}{2}", e, Environment.NewLine, e.Message);
            }
#endif

            Console.WriteLine("DONE!");
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