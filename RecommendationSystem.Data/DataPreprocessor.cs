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
            var userCount = LoadFromDataset();
            GC.Collect();
            PreprocessAllRatings(userCount);
            GC.Collect();

            if (!splitTrainAndTest)
                return;

            GenerateSplitTestAndTrainDataFiles();
            GC.Collect();
        }

        public static void GenerateSplitTestAndTrainDataFiles()
        {
            //split
            var users = UserProvider.Load(DataFiles.Users);

            List<IUser> trainUsers,
                        testUsers;
            users.SplitIntoTrainAndTest(out trainUsers, out testUsers);
            trainUsers.Sort();
            testUsers.Sort();

            UserProvider.Save(DataFiles.TrainUsers, trainUsers);
            UserProvider.Save(DataFiles.TestUsers, testUsers);

            var trainUserCount = trainUsers.Count;
            var testUserCount = testUsers.Count;
            var artistLut = ArtistProvider.Load(DataFiles.Artists).GetLookupTable();

            //train dataset
            LoadAndSaveRatings(DataFiles.TrainPlaycounts, trainUsers, artistLut);
            GC.Collect();
            PreprocessTrainRatings(trainUserCount);

            //test dataset
            LoadAndSaveRatings(DataFiles.TestPlaycounts, testUsers, artistLut);
            GC.Collect();
            PreprocessTestRatings(testUserCount);
            GC.Collect();
        }
        #endregion

        #region Helpers
        private static void LoadAndSaveRatings(string playcountsFile, List<IUser> trainUsers, List<string> artistLut)
        {
            var userLut = trainUsers.GetLookupTable();
            var ratings = RatingProvider.ImportFromDataset(DataFiles.RatingDataset, userLut, artistLut);
            RatingProvider.Save(playcountsFile, ratings);
        }

        private static void PreprocessAllRatings(int userCount)
        {
            const string playcountsFile = DataFiles.Playcounts;

            Normalize(userCount, playcountsFile, DataFiles.NormalizedRatings);
            GC.Collect();
            LogNormalize(userCount, playcountsFile, DataFiles.LogNormalizedRatings);
            GC.Collect();
            ConvertToEqualFrequencyFiveScale(userCount, playcountsFile, DataFiles.EqualFerquencyFiveScaleRatings);
            GC.Collect();
            ConvertToEqualWidthFiveScale(userCount, playcountsFile, DataFiles.EqualWidthFiveScaleRatings);
            GC.Collect();
            ConvertToLogEqualWidthFiveScale(userCount, playcountsFile, DataFiles.LogEqualWidthFiveScaleRatings);
        }

        private static void PreprocessTrainRatings(int userCount)
        {
            const string playcountsFile = DataFiles.TrainPlaycounts;

            Normalize(userCount, playcountsFile, DataFiles.TrainNormalizedRatings);
            GC.Collect();
            LogNormalize(userCount, playcountsFile, DataFiles.TrainLogNormalizedRatings);
            GC.Collect();
            ConvertToEqualFrequencyFiveScale(userCount, playcountsFile, DataFiles.TrainEqualFerquencyFiveScaleRatings);
            GC.Collect();
            ConvertToEqualWidthFiveScale(userCount, playcountsFile, DataFiles.TrainEqualWidthFiveScaleRatings);
            GC.Collect();
            ConvertToLogEqualWidthFiveScale(userCount, playcountsFile, DataFiles.TrainLogEqualWidthFiveScaleRatings);
        }

        private static void PreprocessTestRatings(int userCount)
        {
            const string playcountsFile = DataFiles.TestPlaycounts;

            Normalize(userCount, playcountsFile, DataFiles.TestNormalizedRatings);
            GC.Collect();
            LogNormalize(userCount, playcountsFile, DataFiles.TestLogNormalizedRatings);
            GC.Collect();
            ConvertToEqualFrequencyFiveScale(userCount, playcountsFile, DataFiles.TestEqualFerquencyFiveScaleRatings);
            GC.Collect();
            ConvertToEqualWidthFiveScale(userCount, playcountsFile, DataFiles.TestEqualWidthFiveScaleRatings);
            GC.Collect();
            ConvertToLogEqualWidthFiveScale(userCount, playcountsFile, DataFiles.TestLogEqualWidthFiveScaleRatings);
        }

        private static int LoadFromDataset()
        {
            List<string> userLut,
                         artistLut;
            var users = UserProvider.ImportFromDataset(DataFiles.UserDataset, out userLut);
            UserProvider.Save(DataFiles.Users, users);
            var artists = ArtistProvider.ImportFromDataset(DataFiles.RatingDataset, out artistLut);
            ArtistProvider.Save(DataFiles.Artists, artists);
            var ratings = RatingProvider.ImportFromDataset(DataFiles.RatingDataset, userLut, artistLut);
            RatingProvider.Save(DataFiles.Playcounts, ratings);
            return users.Count;
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