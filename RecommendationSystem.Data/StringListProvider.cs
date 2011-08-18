using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RecommendationSystem.Data
{
    public static class StringListProvider
    {
        public static List<string> ImportFromDataset(string filename, int column, bool sort = true)
        {
            TextReader reader = new StreamReader(filename);

            var data = new List<string>();

            string line;
            var sep = new[] { "\t" };
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(sep, StringSplitOptions.None);
                data.Add(parts[column]);
            }
            if (sort)
                data.Sort();

            reader.Close();
            return data;
        }

        public static List<string> Load(string filename)
        {
            Stream stream = File.Open(filename, FileMode.Open);
            var bformatter = new BinaryFormatter();

            var data = bformatter.Deserialize(stream) as List<string>;
            stream.Close();

            return data;
        }

        public static void Save(string filename, List<string> data)
        {
            Stream stream = File.Open(filename, FileMode.OpenOrCreate);
            var bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, data);
            stream.Close();
        }
    }
}