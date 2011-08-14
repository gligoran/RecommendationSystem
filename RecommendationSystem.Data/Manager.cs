using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.Entity;

namespace RecommendationSystem.Data
{
    public static class Manager
    {
        public static void ImportFromFile(LastFmContext data, string filename, int limit = 0)
        {
            TextReader reader = new StreamReader(filename);

            string line;
            User user = null;
            Rating rating = null;
            Artist artist = null;
            List<string[]> lines = new List<string[]>();

            string aTmp;
            int count = 0;
            Console.Write("0");
            while ((line = reader.ReadLine()) != null && (count < limit || limit == 0))
            {
                var parts = line.Split(new string[] { "\t" }, StringSplitOptions.None);
                if (user == null || user.UserId != parts[0])
                {
                    count++;
                    user = data.Users.Add(new User(parts[0]));

                    Console.Write(", {0}", count);
                }

                aTmp = parts[2];
                artist = data.Artists.Where(a => a.Name == aTmp).SingleOrDefault();
                if (artist == null)
                    artist = data.Artists.Add(new Artist(parts[2]));

                rating = new Rating()
                {
                    User = user,
                    Artist = artist,
                    Value = double.Parse(parts[3])
                };
                data.Ratings.Add(rating);
            }

            // add last if 
            if (limit == 0)
                data.Users.Add(user);

            data.SaveChanges();
        }
    }
}
