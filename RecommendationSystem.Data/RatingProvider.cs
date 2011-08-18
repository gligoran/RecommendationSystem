using System;
using System.Collections.Generic;
using System.IO;

namespace RecommendationSystem.Data
{
    public static class RatingProvider
    {
        public static List<Rating> ImportFromDataset(string filename, List<string> users, List<string> artists, int limit = int.MaxValue)
        {
            TextReader reader = new StreamReader(filename);

            var ratings = new List<Rating>();

            string line;
            var sep = new[] { "\t" };
            while ((line = reader.ReadLine()) != null && limit > 0)
            {
                var parts = line.Split(sep, StringSplitOptions.None);
                
                ratings.Add(new Rating(
                    users.BinarySearch(parts[0]),
                    artists.BinarySearch(parts[2]),
                    float.Parse(parts[3])
                ));

                limit--;
            }

            reader.Close();
            return ratings;
        }

        public static List<Rating> Load(string filename, int limit = int.MaxValue)
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

        public static void Save(string filename, List<Rating> ratings)
        {
            TextWriter writer = new StreamWriter(filename);

            foreach (var t in ratings)
                writer.WriteLine("{0}\t{1}\t{2}", t.UserIndex, t.ArtistIndex, t.Value);

            writer.Flush();
            writer.Close();
        }
    }
}
