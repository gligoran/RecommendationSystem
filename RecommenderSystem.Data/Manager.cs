using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RDotNet;
using System.Xml.Serialization;
using System.IO;

namespace RecommenderSystem.Data
{
    public static class Manager
    {
        #region Load with R
        public static List<User> LoadUsers(string filename, REngine r)
        {
            List<User> users = new List<User>();
            var x = LoadDataFrame(filename, r);

            var ids = x[0].GetAttribute("levels").AsCharacter();
            var sexes = x[1].GetAttribute("levels").AsCharacter();
            var countries = x[3].GetAttribute("levels").AsCharacter();
            var dates = x[4].GetAttribute("levels").AsCharacter();

            Sexes? sex;

            for (int i = 0; i < x.RowCount; i++)
            {
                switch (sexes[(int)x[i, 1] - 1])
                {
                    case "m":
                        sex = Sexes.Male;
                        break;
                    case "f":
                        sex = Sexes.Female;
                        break;
                    default:
                        sex = null;
                        break;
                }

                users.Add(new User(ids[(int)x[i, 0] - 1], countries[(int)x[i, 3] - 1], DateTime.Parse(dates[(int)x[i, 4] - 1]), sex, (int?)x[i, 2]));
            }

            return users;
        }

        public static void LoadPlayCounts<U>(string filename, List<U> users, REngine r) where U : User
        {
            List<string> artists;
            LoadPlayCounts(filename, users, r, out artists);
        }

        public static void LoadPlayCounts<U>(string filename, List<U> users, REngine r, out List<string> artists)
            where U : User
        {
            var x = LoadDataFrame(filename, r);

            var userIds = x[0].GetAttribute("levels").AsCharacter();
            artists = new List<string>(x[2].GetAttribute("levels").AsCharacter().ToArray());

            string userId;
            int count;
            U user = null;

            Console.Write("0");
            for (int i = 0; i < x.RowCount; i++)
            {
                userId = userIds[(int)x[i, 0] - 1];
                if (user == null || user.UserId != userId)
                    user = users.Where(u => u.UserId == userId).First();

                count = -1;
                try
                {
                    count = (int)((double)x[i, 3]);
                }
                catch (InvalidCastException)
                {
                    count = (int)x[i, 3];
                }

                user.PlayCounts.Add(new PlayCount(artists[(int)x[i, 2] - 1], count));
            }
        }

        private static DataFrame LoadDataFrame(string filename, REngine r)
        {
            string rCommand = "t <- read.csv('" + filename + "', " +
                                            "header = FALSE, " +
                                            "sep = '\\t'," +
                                            "quote='')";

            return r.EagerEvaluate(rCommand).AsDataFrame();
        }
        #endregion

        #region Serialization
        public static void SerializeData(List<User> users, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
            TextWriter textWriter = new StreamWriter(filename);
            serializer.Serialize(textWriter, users);
            textWriter.Close();
        }

        public static List<T> DeserializeData<T>(string filename) where T : User
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            TextReader textReader = new StreamReader(filename);
            List<T> users = serializer.Deserialize(textReader) as List<T>;
            return users;
        }
        #endregion

        public static NumericMatrix GetUserItemMatrix(List<User> users, REngine r)
        {
            return GetUserItemMatrix(users, GetArtists(users), r);
        }

        public static NumericMatrix GetUserItemMatrix(List<User> users, List<string> artists, REngine r)
        {
            NumericMatrix df = new NumericMatrix(r, users.Count, artists.Count);
            for (int i = 0; i < users.Count; i++)
            {
                foreach (var playcount in users[i].PlayCounts)
                {
                    df[i, artists.IndexOf(playcount.Artist)] = playcount.Plays;
                }
            }

            return df;
        }

        private static List<string> GetArtists(List<User> users)
        {
            List<string> artists = new List<string>();
            foreach (var user in users)
            {
                foreach (var playcount in user.PlayCounts)
                {
                    if (!artists.Contains(playcount.Artist))
                        artists.Add(playcount.Artist);
                }
            }

            return artists;
        }
    }
}
