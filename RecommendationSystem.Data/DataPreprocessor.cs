using System;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Data.Entities;

namespace RecommendationSystem.Data
{
    public static class DataPreprocessor
    {
        #region PreprocessAllData
        public static void PreprocessAllData()
        {
            List<string> userLut,
                         artistLut;
            var users = UserProvider.ImportFromDataset(DataFiles.UserDataset, out userLut);
            UserProvider.Save(DataFiles.Users, users);
            var artists = ArtistProvider.ImportFromDataset(DataFiles.RatingDataset, out artistLut);
            ArtistProvider.Save(DataFiles.Artists, artists);
            var ratings = RatingProvider.ImportFromDataset(DataFiles.RatingDataset, userLut, artistLut);
            RatingProvider.Save(DataFiles.Playcounts, ratings);

            //normalize
            ratings = RatingProvider.Load(DataFiles.Playcounts);
            ratings.Normalize();
            RatingProvider.Save(DataFiles.NormalizedRatings, ratings);

            GC.Collect();

            //log normalize
            ratings = RatingProvider.Load(DataFiles.Playcounts);
            ratings.LogNormalize();
            RatingProvider.Save(DataFiles.LogNormalizedRatings, ratings);

            GC.Collect();

            //convert to equal frequency five scale
            ratings = RatingProvider.Load(DataFiles.Playcounts);
            ratings.ConvertToEqualFrequencyFiveScale();
            RatingProvider.Save(DataFiles.EqualFerquencyFiveScaleRatings, ratings);

            GC.Collect();

            //convert to equal width five scale
            ratings = RatingProvider.Load(DataFiles.Playcounts);
            ratings.ConvertToEqualWidthFiveScale();
            RatingProvider.Save(DataFiles.EqualWidthFiveScaleRatings, ratings);

            GC.Collect();

            //convert to log equal width five scale
            ratings = RatingProvider.Load(DataFiles.Playcounts);
            ratings.ConvertToLogEqualWidthFiveScale();
            RatingProvider.Save(DataFiles.LogEqualWidthFiveScaleRatings, ratings);
        }
        #endregion

        #region Extensions

        #region Users

        #region Normalize
        public static void Normalize(this IEnumerable<IUser> users)
        {
            foreach (var user in users)
            {
                var max = user.Ratings.Max(rating => rating.Value);
                foreach (var rating in user.Ratings)
                    rating.Value /= max;
            }
        }
        #endregion

        #region LogNormalize
        public static void LogNormalize(this IEnumerable<IUser> users)
        {
            foreach (var user in users)
            {
                var max = (float)user.Ratings.Max(rating => Math.Log(rating.Value));
                foreach (var rating in user.Ratings)
                    rating.Value = (float)Math.Log(rating.Value) / max;
            }
        }
        #endregion

        #region ConvertRatingsToEqualFrequencyFiveScale
        public static void ConvertRatingsToEqualFrequencyFiveScale(this IEnumerable<IUser> users)
        {
            foreach (var user in users)
            {
                user.Ratings = user.Ratings.OrderByDescending(rating => rating.Value).ToList();
                for (var i = 0; i < user.Ratings.Count; i++)
                    user.Ratings[i].Value = 5 - i * 5 / user.Ratings.Count;
            }
        }
        #endregion

        #region ConvertRatingsToEqualWidthFiveScale
        public static void ConvertRatingsToEqualWidthFiveScale(this IEnumerable<IUser> users)
        {
            foreach (var user in users)
            {
                var min = user.Ratings.Min(rating => rating.Value);
                var width = user.Ratings.Max(rating => rating.Value) - min;
                foreach (var rating in user.Ratings)
                    rating.Value = GetEqualWidthRating(min, width, rating.Value);
            }
        }
        #endregion

        #region ConvertRatingsToLogEqualWidthFiveScale
        public static void ConvertRatingsToLogEqualWidthFiveScale(this IEnumerable<IUser> users)
        {
            foreach (var user in users)
            {
                var min = (float)user.Ratings.Min(rating => Math.Log(rating.Value));
                var width = (float)user.Ratings.Max(rating => Math.Log(rating.Value)) - min;
                foreach (var rating in user.Ratings)
                    rating.Value = GetEqualWidthRating(min, width, (float)Math.Log(rating.Value));
            }
        }
        #endregion

        #endregion

        #region Ratings

        #region Normalize
        public static void Normalize(this List<IRating> ratings)
        {
            var userCount = ratings.Select(rating => rating.UserIndex).Distinct().Count();
            ratings.Normalize(userCount);
        }

        private static void Normalize(this IEnumerable<IRating> ratings, int userCount)
        {
            var users = GroupRatingsByUsers(ratings, userCount);
            foreach (var userRatings in users)
            {
                var max = userRatings.Max(rating => rating.Value);
                foreach (var rating in userRatings)
                    rating.Value /= max;
            }
        }
        #endregion

        #region LogNormalize
        public static void LogNormalize(this List<IRating> ratings)
        {
            var userCount = ratings.Select(rating => rating.UserIndex).Distinct().Count();
            ratings.Normalize(userCount);
        }

        public static void LogNormalize(this IEnumerable<IRating> ratings, int userCount)
        {
            var users = GroupRatingsByUsers(ratings, userCount);
            foreach (var userRatings in users)
            {
                var max = (float)userRatings.Max(rating => Math.Log(rating.Value));
                foreach (var rating in userRatings)
                    rating.Value = (float)Math.Log(rating.Value) / max;
            }
        }
        #endregion

        #region ConvertToEqualFrequencyFiveScale
        public static void ConvertToEqualFrequencyFiveScale(this List<IRating> ratings)
        {
            var userCount = ratings.Select(rating => rating.UserIndex).Distinct().Count();
            ratings.ConvertToEqualFrequencyFiveScale(userCount);
        }

        public static void ConvertToEqualFrequencyFiveScale(this IEnumerable<IRating> ratings, int userCount)
        {
            var users = GroupRatingsByUsers(ratings, userCount);
            foreach (var userRatings in users.Select(user => user.OrderByDescending(rating => rating.Value).ToList()))
            {
                for (var i = 0; i < userRatings.Count; i++)
                    userRatings[i].Value = 5 - i * 5 / userRatings.Count;
            }
        }
        #endregion

        #region EqualWidth
        public static void ConvertToEqualWidthFiveScale(this List<IRating> ratings)
        {
            var userCount = ratings.Select(rating => rating.UserIndex).Distinct().Count();
            ratings.ConvertToEqualWidthFiveScale(userCount);
        }

        public static void ConvertToEqualWidthFiveScale(this IEnumerable<IRating> ratings, int userCount)
        {
            var users = GroupRatingsByUsers(ratings, userCount);
            foreach (var userRatings in users)
            {
                var min = userRatings.Min(rating => rating.Value);
                var width = userRatings.Max(rating => rating.Value) - min;
                foreach (var rating in userRatings)
                    rating.Value = GetEqualWidthRating(min, width, rating.Value);
            }
        }
        #endregion

        #region ConvertToLogEqualWidthFiveScale
        public static void ConvertToLogEqualWidthFiveScale(this List<IRating> ratings)
        {
            var userCount = ratings.Select(rating => rating.UserIndex).Distinct().Count();
            ratings.ConvertToLogEqualWidthFiveScale(userCount);
        }

        public static void ConvertToLogEqualWidthFiveScale(this IEnumerable<IRating> ratings, int userCount)
        {
            var users = GroupRatingsByUsers(ratings, userCount);
            foreach (var userRatings in users)
            {
                var min = (float)userRatings.Min(rating => Math.Log(rating.Value));
                var width = (float)userRatings.Max(rating => Math.Log(rating.Value)) - min;
                foreach (var rating in userRatings)
                    rating.Value = GetEqualWidthRating(min, width, (float)Math.Log(rating.Value));
            }
        }
        #endregion

        #endregion

        #endregion

        #region Helpers

        #region GetEqualWidthRating
        private static float GetEqualWidthRating(float min, float width, float rating)
        {
            if (rating < width / 5.0 + min)
                return 1.0f;
            if (rating < width * 2.0 / 5.0 + min)
                return 2.0f;
            if (rating < width * 3.0 / 5.0 + min)
                return 3.0f;
            if (rating < width * 4.0 / 5.0 + min)
                return 4.0f;

            return 5.0f;
        }
        #endregion

        #region GroupRatingsByUsers
        private static IEnumerable<List<IRating>> GroupRatingsByUsers(IEnumerable<IRating> ratings, int userCount)
        {
            var users = new List<IRating>[userCount];
            for (var i = 0; i < userCount; i++)
                users[i] = new List<IRating>();
            foreach (var rating in ratings)
                users[rating.UserIndex].Add(rating);
            return users;
        }
        #endregion

        #endregion
    }
}