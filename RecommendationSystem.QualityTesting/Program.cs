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
using RecommendationSystem.QualityTesting.Testers;

//using Amib.Threading;

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
                Console.WriteLine("Performing {0} tests...", mode);

                List<IArtist> artists;
                List<IUser> trainUsers;
                List<IRating> trainRatings;
                List<IUser> testUsers;
                List<IRating> testRatings;
                LoadData(out trainUsers, out trainRatings, out testUsers, out testRatings, out artists);

                switch (mode)
                {
                    case "simple":
                        var simpleTester = new SimpleTester(trainUsers, artists, trainRatings, testUsers);
                        simpleTester.Test();
                        break;
                    case "knn":
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
                            }
                        }

                        var ras = new List<IRatingAggregator>();
                        var argRas = argList[argList.IndexOf("-ra") + 1].ToLower().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var argRa in argRas)
                        {
                            switch (argRa)
                            {
                                case "sara":
                                    ras.Add(new SimpleAverageRatingAggregator());
                                    break;
                                case "wsra":
                                    ras.Add(new WeightedSumRatingAggregator());
                                    break;
                                case "awsra":
                                    ras.Add(new AdjustedWeightedSumRatingAggregator());
                                    break;
                            }
                        }

                        var trainer = new KnnTrainer();
                        timer.Restart();
                        var knnModel = trainer.TrainModel(trainUsers, artists, trainRatings);
                        timer.Stop();
                        Console.WriteLine("Model trained in {0}ms.", timer.ElapsedMilliseconds);

                        foreach (var k in ks)
                        {
                            foreach (var sim in sims)
                            {
                                foreach (var ra in ras)
                                {
                                    if (performNoContentKnnTests)
                                    {
                                        var knnTester = new KnnTester<KnnRecommender>
                                                        {
                                                            K = k,
                                                            Sim = sim,
                                                            Ra = ra,
                                                            TestUsers = testUsers,
                                                            KnnModel = knnModel,
                                                            Trainer = trainer,
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
                                                                   Ra = ra,
                                                                   TestUsers = testUsers,
                                                                   KnnModel = knnModel,
                                                                   Trainer = trainer,
                                                                   Artists = artists,
                                                                   NumberOfTests = numberOfTests
                                                               };
                                        contentKnnTester.Test();
                                    }
                                }
                            }
                        }

                        break;
                    case "svd":
                    case "mf":
                        Console.WriteLine("Matrix factorization (SVD) tests are not yet implemented.");
                        break;
                }

                if (args.Where(arg => arg.ToLower() == "-wait").Count() != 0)
                    Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}{1}{1}{2}", e, Environment.NewLine, e.Message);
            }
        }

        #region LoadData
        private static void LoadData(out List<IUser> trainUsers, out List<IRating> trainRatings, out List<IUser> testUsers, out List<IRating> testRatings, out List<IArtist> artists)
        {
            Console.WriteLine("Loading data...");
            timer.Restart();
            trainRatings = RatingProvider.Load(DataFiles.TrainLogEqualWidthFiveScaleRatings);
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