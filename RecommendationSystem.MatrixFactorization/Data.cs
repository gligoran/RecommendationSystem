using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace RecommendationSystem.MatrixFactorization
{
    public static class Data
    {
        public static void ReprocessData(bool users = false, bool artists = false, bool ratings = false)
        {
            var u = users ? ReprocessData(@"D:\Dataset\ratings.tsv", 2, @"d:\Dataset\users.rs") : LoadData(@"d:\Dataset\users.rs");
            var a = artists ? ReprocessData(@"D:\Dataset\ratings.tsv", 3, @"d:\Dataset\artists.rs") : LoadData(@"d:\Dataset\artists.rs");

            if (!ratings)
                return;

            var r = GetRatingsFromDataset(@"D:\Dataset\ratings.tsv", u, a).ToList();
            SaveRatings(r, @"d:\Dataset\playcounts.rs");
        }

        private static List<string> ReprocessData(string source, int column, string destination)
        {
            var u = ImportFromDataset(source, column).ToList();
            SaveData(u, destination);
            return u;
        }

        public static SortedSet<string> ImportFromDataset(string filename, int column)
        {
            TextReader reader = new StreamReader(filename);

            var i = new SortedSet<string>();

            string line;
            var sep = new[] { "\t" };
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(sep, StringSplitOptions.None);
                i.Add(parts[column]);
            }

            reader.Close();
            return i;
        }

        public static List<string> LoadData(string filename)
        {
            Stream stream = File.Open(filename, FileMode.Open);
            var bformatter = new BinaryFormatter();

            var data = bformatter.Deserialize(stream) as List<string>;
            stream.Close();

            return data;
        }

        public static void SaveData(List<string> data, string filename)
        {
            Stream stream = File.Open(filename, FileMode.OpenOrCreate);
            var bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, data);
            stream.Close();
        }

        #region Ratings

        #region GetRatingsFromDataset
        public static IEnumerable<Rating> GetRatingsFromDataset(string filename, List<string> users, List<string> artists, int limit = int.MaxValue)
        {
            TextReader reader = new StreamReader(filename);

            string line;
            var sep = new[] { "\t" };
            while ((line = reader.ReadLine()) != null && limit > 0)
            {
                var parts = line.Split(sep, StringSplitOptions.None);

                var u = users.BinarySearch(parts[0]);
                if (u < 0)
                    Console.WriteLine("- {0}", parts[0]);

                yield return new Rating(
                    users.BinarySearch(parts[0]),
                    artists.BinarySearch(parts[2]),
                    float.Parse(parts[3])
                );

                limit--;
            }

            reader.Close();
        }
        #endregion

        #region LoadRatings
        public static List<Rating> LoadRatings(string filename, int limit = int.MaxValue)
        {
            TextReader reader = new StreamReader(filename);

            var ratings = new List<Rating>();

            string line;
            var sep = new[] { "\t" };
            while ((line = reader.ReadLine()) != null && limit > 0)
            {
                var parts = line.Split(sep, StringSplitOptions.None);

                ratings.Add(new Rating(
                    int.Parse(parts[0]),
                    int.Parse(parts[1]),
                    float.Parse(parts[2])
                ));

                limit--;
            }

            reader.Close();
            return ratings;
        }
        #endregion

        #region SaveRatings
        public static void SaveRatings(List<Rating> ratings, string filename)
        {
            TextWriter writer = new StreamWriter(filename);

            foreach (var t in ratings)
                writer.WriteLine("{0}\t{1}\t{2}", t.UserIndex, t.ArtistIndex, t.Value);

            writer.Flush();
            writer.Close();
        }
        #endregion

        #region ConvertTo1To5Ratings
        public static List<Rating> ConvertTo1To5Ratings(List<Rating> playCounts, int userCount)
        {
            var ratings = new List<Rating>();
            var users = new List<Rating>[userCount];

            //initiali user groups
            for (int i = 0; i < userCount; i++)
                users[i] = new List<Rating>();

            //group ratings and users
            foreach (var rating in playCounts)
                users[rating.UserIndex].Add(rating);

            //recalculate play counts into ratings
            foreach (var user in users)
            {
                var u = user.OrderByDescending(r => r.Value).ToList();
                for (var j = 0; j < u.Count; j++)
                    u[j].Value = 5 - j * 5 / u.Count;

                ratings.AddRange(u);
            }

            return ratings;
        }
        #endregion

        #endregion
    }
}
