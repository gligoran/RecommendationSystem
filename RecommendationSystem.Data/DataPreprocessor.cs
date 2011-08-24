using System;
using System.Collections.Generic;
using RecommendationSystem.Entities;

namespace RecommendationSystem.Data
{
    public static class DataPreprocessor
    {
        #region PreprocessAllData
        public static void PreprocessAllData(bool splitTrainAndTest = false)
        {
            Console.WriteLine("Loading data from datasets... {0}", DateTime.Now.ToLongTimeString());
            var userCount = LoadFromDataset();
            Console.WriteLine("Data loaded. {0}", DateTime.Now.ToLongTimeString());
            GC.Collect();

            Console.WriteLine("Preprocessing all ratings... {0}", DateTime.Now.ToLongTimeString());
            PreprocessAllRatings(userCount);
            GC.Collect();

            if (!splitTrainAndTest)
                return;

            Console.WriteLine("Spliting into train and test sets and preprocessing them... {0}", DateTime.Now.ToLongTimeString());
            GenerateSplitTestAndTrainDataFiles();
            Console.WriteLine("Split and preprocessing of train and test sets complete {0}", DateTime.Now.ToLongTimeString());
            GC.Collect();
        }

        public static void GenerateSplitTestAndTrainDataFiles()
        {
            //split
            Console.WriteLine("Loading all users... {0}", DateTime.Now.ToLongTimeString());
            var users = UserProvider.Load(DataFiles.Users);

            List<IUser> trainUsers,
                        testUsers;
            Console.WriteLine("Spliting users into train and test sets... {0}", DateTime.Now.ToLongTimeString());
            users.SplitIntoTrainAndTest(out trainUsers, out testUsers);
            trainUsers.Sort();
            testUsers.Sort();

            Console.WriteLine("Saving train users... {0}", DateTime.Now.ToLongTimeString());
            UserProvider.Save(DataFiles.TrainUsers, trainUsers);
            Console.WriteLine("Saving test users... {0}", DateTime.Now.ToLongTimeString());
            UserProvider.Save(DataFiles.TestUsers, testUsers);

            var trainUserCount = trainUsers.Count;
            var testUserCount = testUsers.Count;
            Console.WriteLine("Loading artists... {0}", DateTime.Now.ToLongTimeString());
            var artistLut = ArtistProvider.Load(DataFiles.Artists).GetLookupTable();

            //train dataset
            Console.WriteLine("Saving and reloading train ratings... {0}", DateTime.Now.ToLongTimeString());
            LoadAndSaveRatings(DataFiles.TrainPlaycounts, trainUsers, artistLut);
            GC.Collect();
            Console.WriteLine("Preprocessing train ratings... {0}", DateTime.Now.ToLongTimeString());
            PreprocessTrainRatings(trainUserCount);

            //test dataset
            Console.WriteLine("Saving and reloading test ratings... {0}", DateTime.Now.ToLongTimeString());
            LoadAndSaveRatings(DataFiles.TestPlaycounts, testUsers, artistLut);
            GC.Collect();
            Console.WriteLine("Preprocessing test ratings... {0}", DateTime.Now.ToLongTimeString());
            PreprocessTestRatings(testUserCount);
            GC.Collect();
        }
        #endregion

        #region Helpers
        private static int LoadFromDataset()
        {
            Console.WriteLine("Loading users from dataset... {0}", DateTime.Now.ToLongTimeString());
            List<string> userLut,
                         artistLut;
            var users = UserProvider.ImportFromDataset(DataFiles.UserDataset, out userLut);
            UserProvider.Save(DataFiles.Users, users);
            GC.Collect();
            Console.WriteLine("Loading artists from dataset... {0}", DateTime.Now.ToLongTimeString());
            var artists = ArtistProvider.ImportFromDataset(DataFiles.RatingDataset, out artistLut);
            ArtistProvider.Save(DataFiles.Artists, artists);
            Console.WriteLine("Loading ratings from dataset... {0}", DateTime.Now.ToLongTimeString());
            var ratings = RatingProvider.ImportFromDataset(DataFiles.RatingDataset, userLut, artistLut);
            Console.WriteLine("Populating users with ratings... {0}", DateTime.Now.ToLongTimeString());
            users.PopulateWithRatings(ratings, true);
            GC.Collect();
            Console.WriteLine("Extracting ratings from users... {0}", DateTime.Now.ToLongTimeString());
            ratings.ExtractFromUsers(users, false);
            RatingProvider.Save(DataFiles.Playcounts, ratings);
            return users.Count;
        }

        private static void LoadAndSaveRatings(string playcountsFile, List<IUser> users, List<string> artistLut)
        {
            var userLut = users.GetLookupTable();
            var ratings = RatingProvider.ImportFromDataset(DataFiles.RatingDataset, userLut, artistLut);
            users.PopulateWithRatings(ratings, true);
            ratings.ExtractFromUsers(users);
            RatingProvider.Save(playcountsFile, ratings);
        }

        private static void PreprocessAllRatings(int userCount)
        {
            const string playcountsFile = DataFiles.Playcounts;

            Console.WriteLine("Normalizing all ratings... {0}", DateTime.Now.ToLongTimeString());
            Normalize(userCount, playcountsFile, DataFiles.NormalizedRatings);
            GC.Collect();
            Console.WriteLine("Log normalizing all ratings... {0}", DateTime.Now.ToLongTimeString());
            LogNormalize(userCount, playcountsFile, DataFiles.LogNormalizedRatings);
            GC.Collect();
            Console.WriteLine("Converting all ratings to equal frequency five scale... {0}", DateTime.Now.ToLongTimeString());
            ConvertToEqualFrequencyFiveScale(userCount, playcountsFile, DataFiles.EqualFerquencyFiveScaleRatings);
            GC.Collect();
            Console.WriteLine("Converting all ratings to equal width five scale... {0}", DateTime.Now.ToLongTimeString());
            ConvertToEqualWidthFiveScale(userCount, playcountsFile, DataFiles.EqualWidthFiveScaleRatings);
            GC.Collect();
            Console.WriteLine("Converting all ratings to log equal width five scale... {0}", DateTime.Now.ToLongTimeString());
            ConvertToLogEqualWidthFiveScale(userCount, playcountsFile, DataFiles.LogEqualWidthFiveScaleRatings);
        }

        private static void PreprocessTrainRatings(int userCount)
        {
            const string playcountsFile = DataFiles.TrainPlaycounts;

            Console.WriteLine("Normalizing train ratings... {0}", DateTime.Now.ToLongTimeString());
            Normalize(userCount, playcountsFile, DataFiles.TrainNormalizedRatings);
            GC.Collect();
            Console.WriteLine("Log normalizing train ratings... {0}", DateTime.Now.ToLongTimeString());
            LogNormalize(userCount, playcountsFile, DataFiles.TrainLogNormalizedRatings);
            GC.Collect();
            Console.WriteLine("Converting train ratings to equal frequency five scale... {0}", DateTime.Now.ToLongTimeString());
            ConvertToEqualFrequencyFiveScale(userCount, playcountsFile, DataFiles.TrainEqualFerquencyFiveScaleRatings);
            GC.Collect();
            Console.WriteLine("Converting train ratings to equal width five scale... {0}", DateTime.Now.ToLongTimeString());
            ConvertToEqualWidthFiveScale(userCount, playcountsFile, DataFiles.TrainEqualWidthFiveScaleRatings);
            GC.Collect();
            Console.WriteLine("Converting train ratings to log equal width five scale... {0}", DateTime.Now.ToLongTimeString());
            ConvertToLogEqualWidthFiveScale(userCount, playcountsFile, DataFiles.TrainLogEqualWidthFiveScaleRatings);
        }

        private static void PreprocessTestRatings(int userCount)
        {
            const string playcountsFile = DataFiles.TestPlaycounts;

            Console.WriteLine("Normalizing test ratings... {0}", DateTime.Now.ToLongTimeString());
            Normalize(userCount, playcountsFile, DataFiles.TestNormalizedRatings);
            GC.Collect();
            Console.WriteLine("Log normalizing test ratings... {0}", DateTime.Now.ToLongTimeString());
            LogNormalize(userCount, playcountsFile, DataFiles.TestLogNormalizedRatings);
            GC.Collect();
            Console.WriteLine("Converting test ratings to equal frequency five scale... {0}", DateTime.Now.ToLongTimeString());
            ConvertToEqualFrequencyFiveScale(userCount, playcountsFile, DataFiles.TestEqualFerquencyFiveScaleRatings);
            GC.Collect();
            Console.WriteLine("Converting test ratings to equal width five scale... {0}", DateTime.Now.ToLongTimeString());
            ConvertToEqualWidthFiveScale(userCount, playcountsFile, DataFiles.TestEqualWidthFiveScaleRatings);
            GC.Collect();
            Console.WriteLine("Converting test ratings to log equal width five scale... {0}", DateTime.Now.ToLongTimeString());
            ConvertToLogEqualWidthFiveScale(userCount, playcountsFile, DataFiles.TestLogEqualWidthFiveScaleRatings);
        }

        private static void Normalize(int userCount, string playcountsFile, string saveFile)
        {
            var ratings = RatingProvider.Load(playcountsFile);
            ratings.Normalize(userCount);
            RatingProvider.Save(saveFile, ratings);
        }

        private static void LogNormalize(int userCount, string playcountsFile, string saveFile)
        {
            var rating = RatingProvider.Load(playcountsFile);
            rating.LogNormalize(userCount);
            RatingProvider.Save(saveFile, rating);
        }

        private static void ConvertToEqualFrequencyFiveScale(int userCount, string playcountsFile, string saveFile)
        {
            var ratings = RatingProvider.Load(playcountsFile);
            ratings.ConvertToEqualFrequencyFiveScale(userCount);
            RatingProvider.Save(saveFile, ratings);
        }

        private static void ConvertToEqualWidthFiveScale(int userCount, string playcountsFile, string saveFile)
        {
            var ratings = RatingProvider.Load(playcountsFile);
            ratings.ConvertToEqualWidthFiveScale(userCount);
            RatingProvider.Save(saveFile, ratings);
        }

        private static void ConvertToLogEqualWidthFiveScale(int userCount, string playcountsFile, string saveFile)
        {
            var ratings = RatingProvider.Load(playcountsFile);
            ratings.ConvertToLogEqualWidthFiveScale(userCount);
            RatingProvider.Save(saveFile, ratings);
        }
        #endregion
    }
}