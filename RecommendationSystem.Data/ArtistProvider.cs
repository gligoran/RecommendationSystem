using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RecommendationSystem.Entities;

namespace RecommendationSystem.Data
{
    public static class ArtistProvider
    {
        #region ImportFromDataset
        public static IEnumerable<IArtist> ImportFromDataset(string filename, out List<string> artistIndexLookupTable, int limit = int.MaxValue)
        {
            return ImportFromDataset(filename, 2, out artistIndexLookupTable);
        }

        public static List<IArtist> ImportFromDataset(string filename, int limit = int.MaxValue)
        {
            return ImportFromDataset(filename, 2, limit);
        }

        private static List<IArtist> ImportFromDataset(string filename, int column, out List<string> artistIndexLookupTable, int limit = int.MaxValue)
        {
            var artists = ImportFromDataset(filename, column, limit);
            artistIndexLookupTable = artists.GetLookupTable();
            return artists;
        }

        private static List<IArtist> ImportFromDataset(string filename, int column, int limit = int.MaxValue)
        {
            TextReader reader = new StreamReader(filename);

            var artists = new SortedSet<IArtist>();

            string line;
            var sep = new[] {"\t"};
            while ((line = reader.ReadLine()) != null && limit > 0)
            {
                var parts = line.Split(sep, StringSplitOptions.None);
                artists.Add(new Artist(parts[column]));
                limit--;
            }

            reader.Close();
            return artists.ToList();
        }
        #endregion

        #region Text

        #region Save
        public static void Save(string filename, IEnumerable<IArtist> artists)
        {
            var dir = Path.GetDirectoryName(filename);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            TextWriter writer = new StreamWriter(filename);

            foreach (var artist in artists)
                writer.WriteLine("{0}", artist.Name);

            writer.Flush();
            writer.Close();
        }
        #endregion

        #region Load
        public static List<IArtist> Load(string filename, int limit = int.MaxValue)
        {
            return ImportFromDataset(filename, 0, limit);
        }

        public static List<IArtist> Load(string filename, out List<string> artistIndexLookupTable, int limit = int.MaxValue)
        {
            return ImportFromDataset(filename, 0, out artistIndexLookupTable, limit);
        }
        #endregion

        #endregion

        #region GetLookupTable
        public static List<string> GetLookupTable(this IEnumerable<IArtist> artists)
        {
            return artists.Select(artist => artist.Name).ToList();
        }
        #endregion
    }
}