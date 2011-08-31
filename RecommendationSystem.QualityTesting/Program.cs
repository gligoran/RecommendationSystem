using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RecommendationSystem.Data;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.RatingAggregation;
using RecommendationSystem.Knn.Recommendations;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Training;
using RecommendationSystem.MatrixFactorization.Basic.Training;
using RecommendationSystem.MatrixFactorization.Bias.Training;
using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.MatrixFactorization.Training;
using RecommendationSystem.QualityTesting.Testers;

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
                        var simpleTester = new SimpleTester(trainUsers, artists, trainRatings, testUsers);
                        simpleTester.Test();
                        break;
                        #endregion

                    case "knn":

                        #region kNN
                        var ks = argList[argList.IndexOf("-k") + 1].ToLower().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                        var numberOfTests = int.Parse(argList[argList.IndexOf("-t") + 1].ToLower());
                        var performNoContentKnnTests = argList.IndexOf("-noco") >= 0;
                        var performContentKnnTests = argList.IndexOf("-co") >= 0;

                        var sims = new List<ISimilarityEstimator>();
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

                        var rgs = new List<IRecommendationGenerator>();
                        var argRgs = argList[argList.IndexOf("-rg") + 1].ToLower().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var argRg in argRgs)
                        {
                            switch (argRg)
                            {
                                case "sara":
                                    rgs.Add(new RatingAggregationRecommendationGenerator(new SimpleAverageRatingAggregator()));
                                    break;
                                case "wsra":
                                    rgs.Add(new RatingAggregationRecommendationGenerator(new WeightedSumRatingAggregator()));
                                    break;
                                case "awsra":
                                    rgs.Add(new RatingAggregationRecommendationGenerator(new AdjustedWeightedSumRatingAggregator()));
                                    break;
                                case "frg":
                                    rgs.Add(new FifthsRecommendationGenerator());
                                    break;
                                case "edrg":
                                    rgs.Add(new EqualDescentRecommendationGenerator());
                                    break;
                            }
                        }

                        var knnTrainer = new KnnTrainer();
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
                                        var knnTester = new KnnTester<KnnRecommender>
                                                        {
                                                            K = k,
                                                            Sim = sim,
                                                            Rg = rg,
                                                            TestUsers = testUsers,
                                                            KnnModel = knnModel,
                                                            Trainer = knnTrainer,
                                                            Artists = artists,
                                                            NumberOfTests = numberOfTests
                                                        };
                                        knnTester.Test();
                                    }

                                    if (performContentKnnTests)
                                    {
                                        var contentKnnTester = new KnnTester<ContentKnnRecommender>
                                                               {
                                                                   K = k,
                                                                   Sim = sim,
                                                                   Rg = rg,
                                                                   TestUsers = testUsers,
                                                                   KnnModel = knnModel,
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
                        var ttype = argList[argList.IndexOf("-type") + 1].ToLower().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)[0];
                        if (ttype == "basic")
                        {
                            var svdTrainer = new BasicSvdTrainer();
                            var model = svdTrainer.TrainModel(trainUsers, artists, trainRatings, new TrainingParameters());
                            svdTrainer.SaveModel(@"D:\Dataset\models\basic-svd-model.rs", model);
                        }
                        else if (ttype == "bias")
                        {
                            var svdTrainer = new BiasSvdTrainer();
                            var model = svdTrainer.TrainModel(trainUsers, artists, trainRatings, new TrainingParameters());
                            svdTrainer.SaveModel(@"D:\Dataset\models\bias-svd-model.rs", model);
                        }

                        break;
                        #endregion

                    case "svd":
                    case "mf":

                        #region SVD/MF Prediction
                        ISvdModel svdModel = null;
                        var ptype = argList[argList.IndexOf("-type") + 1].ToLower().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)[0];
                        if (ptype == "basic")
                        {
                            var svdTrainer = new BasicSvdTrainer();
                            svdModel = svdTrainer.LoadModel(@"D:\Dataset\models\basic-svd-model.rs");
                        }
                        else if (ptype == "bias")
                        {
                            var svdTrainer = new BiasSvdTrainer();
                            svdModel = svdTrainer.LoadModel(@"D:\Dataset\models\bias-svd-model.rs");
                        }
                        else
                            throw new ArgumentException("Unknown type of SVD model. Enter BASIC or BIAS.");

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