using System;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;

namespace RecommendationSystem.Data
{
    public static class DataSpliter
    {
        public static void GenerateSplitTestAndTrainDataFiles()
        {
            #region Split
            var users = UserProvider.Load(DataFiles.Users);

            List<IUser> train,
                        test;
            users.SplitIntoTrainAndTest(out train, out test);
            train.Sort();
            test.Sort();

            UserProvider.Save(DataFiles.TrainUsers, train);
            UserProvider.Save(DataFiles.TestUsers, test);
            #endregion

            #region Preprocess
            var artists = ArtistProvider.Load(DataFiles.Artists);
            var artistLut = artists.GetLookupTable();

            #region TrainDataset
            var userLut = train.GetLookupTable();
            var ratings = RatingProvider.ImportFromDataset(DataFiles.RatingDataset, userLut, artistLut);
            RatingProvider.Save(DataFiles.TrainPlaycounts, ratings);

            //normalize
            ratings = RatingProvider.Load(DataFiles.TrainPlaycounts);
            ratings.Normalize();
            RatingProvider.Save(DataFiles.TrainNormalizedRatings, ratings);

            GC.Collect();

            //log normalize
            ratings = RatingProvider.Load(DataFiles.TrainPlaycounts);
            ratings.LogNormalize();
            RatingProvider.Save(DataFiles.TrainLogNormalizedRatings, ratings);

            GC.Collect();

            //convert to equal frequency five scale
            ratings = RatingProvider.Load(DataFiles.TrainPlaycounts);
            ratings.ConvertToEqualFrequencyFiveScale();
            RatingProvider.Save(DataFiles.TrainEqualFerquencyFiveScaleRatings, ratings);

            GC.Collect();

            //convert to equal width five scale
            ratings = RatingProvider.Load(DataFiles.TrainPlaycounts);
            ratings.ConvertToEqualWidthFiveScale();
            RatingProvider.Save(DataFiles.TrainEqualWidthFiveScaleRatings, ratings);

            GC.Collect();

            //convert to log equal width five scale
            ratings = RatingProvider.Load(DataFiles.TrainPlaycounts);
            ratings.ConvertToLogEqualWidthFiveScale();
            RatingProvider.Save(DataFiles.TrainLogEqualWidthFiveScaleRatings, ratings);
            #endregion

            #region TestDataset
            userLut = test.GetLookupTable();
            ratings = RatingProvider.ImportFromDataset(DataFiles.RatingDataset, userLut, artistLut);
            RatingProvider.Save(DataFiles.TestPlaycounts, ratings);

            //normalize
            ratings = RatingProvider.Load(DataFiles.TestPlaycounts);
            ratings.Normalize();
            RatingProvider.Save(DataFiles.TestNormalizedRatings, ratings);

            GC.Collect();

            //log normalize
            ratings = RatingProvider.Load(DataFiles.TestPlaycounts);
            ratings.LogNormalize();
            RatingProvider.Save(DataFiles.TestLogNormalizedRatings, ratings);

            GC.Collect();

            //convert to equal frequency five scale
            ratings = RatingProvider.Load(DataFiles.TestPlaycounts);
            ratings.ConvertToEqualFrequencyFiveScale();
            RatingProvider.Save(DataFiles.TestEqualFerquencyFiveScaleRatings, ratings);

            GC.Collect();

            //convert to equal width five scale
            ratings = RatingProvider.Load(DataFiles.TestPlaycounts);
            ratings.ConvertToEqualWidthFiveScale();
            RatingProvider.Save(DataFiles.TestEqualWidthFiveScaleRatings, ratings);

            GC.Collect();

            //convert to log equal width five scale
            ratings = RatingProvider.Load(DataFiles.TestPlaycounts);
            ratings.ConvertToLogEqualWidthFiveScale();
            RatingProvider.Save(DataFiles.TestLogEqualWidthFiveScaleRatings, ratings);
            #endregion

            #endregion
        }

        public static void SplitIntoTrainAndTest(this List<IUser> users, out List<IUser> train, out List<IUser> test, float trainShare = 0.7f)
        {
            train = new List<IUser>();

            var indices = users.Select((t, i) => i).ToList();

            var random = new Random();
            var count = (int)(users.Count * trainShare);
            for (var i = 0; i < count; i++)
            {
                var index = indices[random.Next(indices.Count)];
                train.Add(users[index]);

                indices.Remove(index);
            }

            test = indices.Select(index => new User(users[index].UserId, users[index].SignUp, users[index].Gender, users[index].Age, users[index].Country)).Cast<IUser>().ToList();
        }
    }
}