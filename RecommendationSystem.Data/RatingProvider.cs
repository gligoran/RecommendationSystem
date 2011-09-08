using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using RecommendationSystem.Entities;

namespace RecommendationSystem.Data
{
    public static class RatingProvider
    {
        #region ImportFromDataset
        public static List<IRating> ImportFromDataset(string filename, List<string> users, List<string> artists, int limit = int.MaxValue)
        {
            TextReader reader = new StreamReader(filename);

            var ratings = new List<IRating>();

            string line;
            var sep = new[] {"\t"};
            while ((line = reader.ReadLine()) != null && limit > 0)
            {
                limit--;

                var parts = line.Split(sep, StringSplitOptions.None);

                var userIndex = users.BinarySearch(parts[0]);
                if (userIndex < 0)
                    continue;

                ratings.Add(new Rating(
                                userIndex,
                                artists.BinarySearch(parts[2]),
                                float.Parse(parts[3], CultureInfo.InvariantCulture)
                                ));
            }

            reader.Close();
            return ratings;
        }
        #endregion

        #region Load
        public static List<IRating> Load(string filename, int limit = int.MaxValue)
        {
            TextReader reader = new StreamReader(filename);

            var ratings = new List<IRating>();

            string line;
            var sep = new[] {"\t"};
            while ((line = reader.ReadLine()) != null && limit > 0)
            {
                var parts = line.Split(sep, StringSplitOptions.None);
                ratings.Add(new Rating(int.Parse(parts[0]), int.Parse(parts[1]), float.Parse(parts[2], CultureInfo.InvariantCulture)));
                limit--;
            }

            reader.Close();
            return ratings;
        }
        #endregion

        #region Save
        public static void Save(string filename, IEnumerable<IRating> ratings)
        {
            var dir = Path.GetDirectoryName(filename);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            TextWriter writer = new StreamWriter(filename);

            foreach (var rating in ratings)
                writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}\t{1}\t{2}", rating.UserIndex, rating.ArtistIndex, rating.Value));

            writer.Flush();
            writer.Close();
        }
        #endregion

        #region PopulateUsersWithRatings
        public static void PopulateWithRatings(this List<IUser> users, string ratingsFile, bool combineDuplicateArtists = false)
        {
            var ratings = Load(ratingsFile);
            users.PopulateWithRatings(ratings);
        }

        public static void PopulateWithRatings(this List<IUser> users, IEnumerable<IRating> ratings, bool combineDuplicateArtists = false)
        {
            foreach (var rating in ratings.TakeWhile(rating => rating.UserIndex < users.Count))
                users[rating.UserIndex].Ratings.Add(rating);

            if (combineDuplicateArtists)
                users.CombineDuplicateArtists();
        }

        private static void CombineDuplicateArtists(this IEnumerable<IUser> users)
        {
            var max = 0;
            var count = 0;
            foreach (var user in users)
            {
                var artistIndices = user.Ratings.Select(rating => rating.ArtistIndex).Distinct().ToList();
                if (artistIndices.Count == user.Ratings.Count)
                    continue;

                var ratings = new List<IRating>();
                foreach (var artistIndex in artistIndices)
                {
                    var rs = user.Ratings.Where(r => r.ArtistIndex == artistIndex).ToList();
                    var rating = rs[0];
                    for (var i = 1; i < rs.Count; i++)
                        rating.Value += rs[i].Value;

                    ratings.Add(rating);
                }

                var diff = user.Ratings.Count - ratings.Count;
                if (max < diff)
                    max = diff;
                user.Ratings = ratings;
                count++;
            }

            Console.WriteLine("{0} users with duplicat artist entires. Max removed artists {1}.", count, max);
        }
        #endregion

        #region PopulateArtistsWithRatings
        public static void PopulateWithRatings(this List<IArtist> artists, string ratingsFile)
        {
            var ratings = Load(ratingsFile);
            artists.PopulateWithRatings(ratings);
        }

        public static void PopulateWithRatings(this List<IArtist> artists, IEnumerable<IRating> ratings)
        {
            foreach (var rating in ratings)
                artists[rating.ArtistIndex].Ratings.Add(rating);
        }
        #endregion

        #region ExtractFromUsers
        public static List<IRating> ExtractRatingsFromUsers(IEnumerable<IUser> users)
        {
            if (users == null)
                return null;

            var ratings = new List<IRating>();

            foreach (var user in users)
                ratings.AddRange(user.Ratings.Select(rating => rating.Clone()));

            return ratings;
        }

        public static List<IRating> ExtractFromUsers(this List<IRating> ratings, IEnumerable<IUser> users, bool copyRatings = true)
        {
            if (users == null)
                return null;

            ratings.Clear();

            foreach (var user in users)
                ratings.AddRange(copyRatings ? user.Ratings.Select(rating => rating.Clone()) : user.Ratings);

            return ratings;
        }
        #endregion
    }
}