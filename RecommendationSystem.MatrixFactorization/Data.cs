using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.Entity;
using System.Runtime.Serialization.Formatters.Binary;

namespace RecommendationSystem.MatrixFactorization
{
    public static class Data
    {
        #region ReloadData
        public static void ReloadData(bool users = false, bool artists = false, bool ratings = false)
        {
            if (users)
            {
                Console.WriteLine("Importing users...");
                var u = GetUsersFromDataset(@"D:\Dataset\users.tsv").ToList();
                Console.WriteLine("Users imported.");

                Console.WriteLine("Saving users...");
                SaveUsers(u, @"d:\Dataset\users.rs");
                Console.WriteLine("Save complete.");
            }

            if (artists)
            {
                Console.WriteLine("Importing artists...");
                var a = GetArtistsFromDataset(@"D:\Dataset\ratings.tsv").ToList();
                Console.WriteLine("Artists imported.");

                Console.WriteLine("Saving artists...");
                SaveArtists(a, @"d:\Dataset\artists.rs");
                Console.WriteLine("Save complete.");
            }

            if (ratings)
            {
                var u = LoadUsers(@"d:\Dataset\users.rs");
                var a = LoadArtists(@"d:\Dataset\artists.rs");

                Console.WriteLine("Importing ratings...");
                var r = GetRatingsFromDataset(@"D:\Dataset\ratings.tsv", u, a).ToList();
                Console.WriteLine("Ratings imported.");

                Console.WriteLine("Saving ratings...");
                SaveRatings(r, @"d:\Dataset\ratings.rs");
                Console.WriteLine("Save complete.");
            }
        }
        #endregion

        #region Artists

        #region GetArtistsFromDataset
        public static SortedSet<string> GetArtistsFromDataset(string filename)
        {
            TextReader reader = new StreamReader(filename);

            var a = new SortedSet<string>();

            string line;
            string[] sep = new string[] { "\t" };
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(sep, StringSplitOptions.None);
                a.Add(parts[2]);
            }

            reader.Close();
            return a;
        }
        #endregion

        #region LoadArtists
        public static List<string> LoadArtists(string filename)
        {
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();

            List<string> artists = bformatter.Deserialize(stream) as List<string>;
            stream.Close();

            return artists;
        }
        #endregion

        #region SaveArtists
        public static void SaveArtists(List<string> artists, string filename)
        {
            Stream stream = File.Open(filename, FileMode.OpenOrCreate);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, artists);
            stream.Close();
        }
        #endregion

        #endregion

        #region Users

        #region GetUsersFromDataset
        public static SortedSet<string> GetUsersFromDataset(string filename)
        {
            TextReader reader = new StreamReader(filename);

            var u = new SortedSet<string>();

            string line;
            string[] sep = new string[] { "\t" };
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(sep, StringSplitOptions.None);
                u.Add(parts[0]);
            }

            reader.Close();
            return u;
        }
        #endregion

        #region LoadUsers
        public static List<string> LoadUsers(string filename)
        {
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();

            List<string> users = bformatter.Deserialize(stream) as List<string>;
            stream.Close();

            return users;
        }
        #endregion

        #region SaveUsers
        public static void SaveUsers(List<string> users, string filename)
        {
            Stream stream = File.Open(filename, FileMode.OpenOrCreate);
            BinaryFormatter bformatter = new BinaryFormatter();

            Console.WriteLine("Writing artists to file");
            bformatter.Serialize(stream, users);
            stream.Close();
        }
        #endregion

        #endregion

        #region Ratings

        #region GetRatingsFromDataset
        public static IEnumerable<Rating> GetRatingsFromDataset(string filename, List<string> users, List<string> artists, int limit = 17559531)
        {
            TextReader reader = new StreamReader(filename);

            string line;
            string[] sep = new string[] { "\t" };
            while ((line = reader.ReadLine()) != null && limit > 0)
            {
                var parts = line.Split(sep, StringSplitOptions.None);

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
        public static List<Rating> LoadRatings(string filename)
        {
            TextReader reader = new StreamReader(filename);

            var ratings = new List<Rating>();

            string line;
            string[] sep = new string[] { "\t" };
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(sep, StringSplitOptions.None);

                ratings.Add(new Rating(
                    int.Parse(parts[0]),
                    int.Parse(parts[1]),
                    float.Parse(parts[2])
                ));
            }

            reader.Close();
            return ratings;
        }
        #endregion

        #region SaveRatings
        public static void SaveRatings(List<Rating> ratings, string filename)
        {
            TextWriter writer = new StreamWriter(filename);

            for (int i = 0; i < ratings.Count; i++)
            {
                writer.WriteLine("{0}\t{1}\t{2}", ratings[i].UserIndex, ratings[i].ArtistIndex, ratings[i].Value);
            }

            writer.Flush();
            writer.Close();
        }
        #endregion

        #endregion
    }
}
