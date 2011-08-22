using System;
using System.Collections.Generic;
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
                                float.Parse(parts[3])
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
                ratings.Add(new Rating(int.Parse(parts[0]), int.Parse(parts[1]), float.Parse(parts[2])));
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

            foreach (var t in ratings)
                writer.WriteLine("{0}\t{1}\t{2}", t.UserIndex, t.ArtistIndex, t.Value);

            writer.Flush();
            writer.Close();
        }
        #endregion

        #region PopulateUsersWithRatings
        public static void PopulateWithRatings(this List<IUser> users, string ratingsFile)
        {
            var ratings = Load(ratingsFile);
            users.PopulateWithRatings(ratings);
        }

        public static void PopulateWithRatings(this List<IUser> users, IEnumerable<IRating> ratings)
        {
            foreach (var rating in ratings)
                users[rating.UserIndex].Ratings.Add(rating);
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

        #region ExtractRatingsFromUsers
        public static List<IRating> ExtractRatingsFromUsers(this IEnumerable<IUser> users)
        {
            if (users == null)
                return null;

            var ratings = new List<IRating>();

            foreach (var user in users)
                ratings.AddRange(user.Ratings.Select(rating => rating.Clone()));

            return ratings;
        }
        #endregion
    }
}