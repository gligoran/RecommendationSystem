﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using RecommendationSystem.Data.Entities;

namespace RecommendationSystem.Data
{
    public static class UserProvider
    {
        #region ImportFromDataset
        public static List<IUser> ImportFromDataset(string filename, out List<string> userIndexLookupTable, int limit = int.MaxValue)
        {
            var users = ImportFromDataset(filename, limit);
            userIndexLookupTable = CreateLookupTable(users);
            return users;
        }

        public static List<IUser> ImportFromDataset(string filename, int limit = int.MaxValue)
        {
            TextReader reader = new StreamReader(filename);

            var users = new SortedSet<IUser>();

            string line;
            var sep = new[] { "\t" };
            while ((line = reader.ReadLine()) != null && limit > 0)
            {
                var parts = line.Split(sep, StringSplitOptions.None);

                int age;
                int.TryParse(parts[2], out age);
                users.Add(new User(parts[0], DateTime.Parse(parts[4], CultureInfo.InvariantCulture), parts[1], age, parts[3]));

                limit--;
            }

            reader.Close();
            return users.ToList();
        }
        #endregion

        #region Text

        #region Save
        public static void Save(string filename, List<IUser> users)
        {
            TextWriter writer = new StreamWriter(filename);

            foreach (var user in users)
                writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", user.UserId, user.Gender, user.Age, user.Country, user.SignUp.ToString("d", CultureInfo.InvariantCulture));

            writer.Flush();
            writer.Close();
        }
        #endregion

        #region Load
        public static List<IUser> Load(string filename, int limit = int.MaxValue)
        {
            return ImportFromDataset(filename, limit);
        }

        public static List<IUser> Load(string filename, out List<string> userIndexLookupTable, int limit = int.MaxValue)
        {
            return ImportFromDataset(filename, out userIndexLookupTable, limit);
        }
        #endregion

        #endregion

        #region CreateLookupTable
        private static List<string> CreateLookupTable(IEnumerable<IUser> users)
        {
            return users.Select(u => u.UserId).ToList();
        }
        #endregion
    }
}