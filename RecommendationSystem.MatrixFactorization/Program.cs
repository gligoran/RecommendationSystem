using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecommendationSystem.Data;
using System.Data.Entity;

namespace RecommendationSystem.MatrixFactorization
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<LastFmContext>());
            Database.SetInitializer(new DropCreateDatabaseAlways<LastFmContext>());
            LastFmContext data = new LastFmContext();


            Console.WriteLine("Importing data...");
            Manager.ImportFromFile(data, @"D:\Dataset\data-with-mbids.tsv");
            Console.WriteLine("Import complete.");

            /*foreach (var rating in data.Ratings.ToList())
            {
                Console.WriteLine("{0}: <{1}, {2}, {3}>", rating.RatingId, rating.User.UserId, rating.Artist.Name, rating.Value);
            }*/

            Console.WriteLine("Users: {0}, Artists: {1}, Ratings: {2}", data.Users.Count(), data.Artists.Count(), data.Ratings.Count());

            Console.WriteLine("Done. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
