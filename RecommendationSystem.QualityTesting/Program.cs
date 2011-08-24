using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using RecommendationSystem.Data;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn;
using RecommendationSystem.Knn.Models;
using RecommendationSystem.Knn.RatingAggregation;
using RecommendationSystem.Knn.Recommendations;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Training;
using RecommendationSystem.Models;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Simple.AverageRating;
using RecommendationSystem.Simple.MedianRating;
using RecommendationSystem.Simple.MostCommonRating;
using RecommendationSystem.Training;

namespace RecommendationSystem.QualityTesting
{
    public static class Program
    {
        private static readonly Stopwatch timer = new Stopwatch();
        private static CryptoRandom rng = new CryptoRandom();
        private static TextWriter fileWriter;

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

                switch (mode)
                {
                    case "simple":

                        #region Simple RS'
                        LoadData(out trainUsers, out trainRatings, out testUsers, out testRatings, out artists);

                        //initialize rs'
                        var ar = new AverageRatingRecommendationSystem();
                        var mr = new MedianRatingRecommendationSystem();
                        var mcr = new MostCommonRatingRecommendationSystem();

                        //train models
                        var arModel = ar.Trainer.TrainModel(trainUsers, artists, trainRatings);
                        var mrModel = mr.Trainer.TrainModel(trainUsers, artists, trainRatings);
                        var mcrModel = mcr.Trainer.TrainModel(trainUsers, artists, trainRatings);

                        var rv = CompleteTestRecommendationSystem(artists, testUsers, arModel, ar);
                        Console.WriteLine("AR: {0}", rv);

                        rv = CompleteTestRecommendationSystem(artists, testUsers, mrModel, mr);
                        Console.WriteLine("MR: {0}", rv);

                        rv = CompleteTestRecommendationSystem(artists, testUsers, mcrModel, mcr);
                        Console.WriteLine("MCR: {0}", rv);
                        break;
                        #endregion

                    case "knn":

                        #region Knn RS'
                        LoadData(out trainUsers, out trainRatings, out testUsers, out testRatings, out artists);

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
                                        KnnTest<KnnRecommender>(k, sim, ra, testUsers, knnModel, trainer, artists, numberOfTests);

                                    if (performContentKnnTests)
                                        KnnTest<ContentKnnRecommender>(k, sim, ra, testUsers, knnModel, trainer, artists, numberOfTests);
                                }
                            }
                        }
                        break;
                        #endregion

                    case "svd":
                    case "mf":

                        #region MF/SVD RS'
                        LoadData(out trainUsers, out trainRatings, out testUsers, out testRatings, out artists);

                        Console.WriteLine("Matrix factorization (SVD) tests are not yet implemented.");
                        break;
                        #endregion
                }

                if (args.Where(arg => arg.ToLower() == "-wait").Count() != 0)
                    Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}{1}{1}{2}", e, Environment.NewLine, e.Message);
            }
        }

        #region KnnTest
        private static void KnnTest<TRecommender>(int k, ISimilarityEstimator sim, IRatingAggregator ra, List<IUser> testUsers, IKnnModel knnModel, ITrainer<IKnnModel, IUser> trainer, List<IArtist> artists, int numberOfTests)
            where TRecommender : IRecommender<IKnnModel>
        {
            var recommender = (TRecommender)Activator.CreateInstance(typeof(TRecommender), new object[] {sim, ra, k});

            InitializeResultWriter(String.Format(@"D:\Dataset\results\Knn-{0}-{1}-{2}.txt", sim, ra, recommender));
            Write("------------------------------------------------------", false);
            Write(string.Format("Test Knn-{0}-{1}-{2} ({3})", sim, ra, recommender, DateTime.Now));
            Write("------------------------------------------------------");

            var rs = new KnnRecommendationSystem(trainer, recommender);
            timer.Restart();
            var rv = TestRecommendationSystem(artists, testUsers, knnModel, rs, numberOfTests);
            timer.Stop();

            Write(string.Format("RMSE(Knn-{0}-{1}-{2}) = {3} ({4}ms).", sim, ra, recommender, rv, timer.ElapsedMilliseconds));
            fileWriter.Close();
        }
        #endregion

        #region TestRecommendationSystem
        private static RmseAndVariance TestRecommendationSystem<TModel, TUser>(List<IArtist> artists, List<TUser> testUsers, TModel model, IRecommendationSystem<TModel, TUser, ITrainer<TModel, TUser>, IRecommender<TModel>> rs, int numberOfTests = 100000)
            where TModel : IModel
            where TUser : IUser
        {
            var tenPrecent = numberOfTests / 10;
            var rmseList = new List<float>();
            while (rmseList.Count < numberOfTests)
            {
                var userIndex = rng.Next(testUsers.Count);
                var user = testUsers[userIndex];

                //if true we'll remove the only rating
                if (user.Ratings.Count < 2)
                    continue;

                var ratingIndex = rng.Next(user.Ratings.Count);
                var rating = user.Ratings[ratingIndex];

                var error = GerPredictionError(artists, model, rs, rating, user);

                rmseList.Add((float)Math.Sqrt(error * error));

                if (rmseList.Count % tenPrecent != 0)
                    continue;

                Write(string.Format("Test at {0} in {1}ms with RMSE {2}", rmseList.Count, timer.ElapsedMilliseconds, rmseList.Sum() / rmseList.Count));
            }

            var rv = new RmseAndVariance(rmseList);
            return rv;
        }
        #endregion

        #region CompleteTestRecommendationSystem
        private static RmseAndVariance CompleteTestRecommendationSystem<TModel, TUser>(List<IArtist> artists, List<TUser> testUsers, TModel model, IRecommendationSystem<TModel, TUser, ITrainer<TModel, TUser>, IRecommender<TModel>> rs)
            where TModel : IModel
            where TUser : IUser
        {
            var rmseList = (from user in testUsers
                            let userError = user.Ratings.Select(rating => GerPredictionError(artists, model, rs, rating, user)).Select(error => error * error).Sum()
                            select (float)Math.Sqrt(userError / user.Ratings.Count)).ToList();

            var rv = new RmseAndVariance(rmseList);
            return rv;
        }
        #endregion

        #region GerPredictionError
        private static float GerPredictionError<TModel, TUser>(List<IArtist> artists, TModel model, IRecommendationSystem<TModel, TUser, ITrainer<TModel, TUser>, IRecommender<TModel>> rs, IRating rating, TUser user)
            where TModel : IModel
            where TUser : IUser
        {
            var originalRatings = user.Ratings;
            user.Ratings = user.Ratings.Where(r => r != rating).ToList();

            var error = rating.Value - rs.Recommender.PredictRatingForArtist(user, model, artists, rating.ArtistIndex);

            user.Ratings = originalRatings;
            return error;
        }
        #endregion

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

        #region SavingResults
        private static void InitializeResultWriter(string filename)
        {
            var dir = Path.GetDirectoryName(filename);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            fileWriter = new StreamWriter(filename);
        }

        private static void Write(string text, bool toFile = true)
        {
            Console.WriteLine(text);

            if (!toFile)
                return;

            fileWriter.WriteLine(text);
            fileWriter.Flush();
        }
        #endregion
    }
}