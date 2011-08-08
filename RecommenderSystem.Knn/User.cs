using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System;

namespace RecommenderSystem.Knn
{
    public class User : IComparable<User>
    {
        public string UserId { get; set; }
        public Dictionary<string, int> Plays { get; set; }
        public int TotalPlays { get; set; }
        public SortedSet<User> Neighbours { get; set; }

        public User(string userId)
        {
            this.UserId = userId;
            this.TotalPlays = 0;
            this.Plays = new Dictionary<string, int>();
            this.Neighbours = new SortedSet<User>();
        }

        public static User CreateFormPlays(List<string[]> plays)
        {
            if (plays.Count == 0)
                return null;

            User user = new User(plays[0][0]);

            int count;
            foreach (var play in plays)
            {
                count = int.Parse(play[2]);
                if (user.Plays.ContainsKey(play[1]))
                    user.Plays[play[1]] += count;
                else
                    user.Plays.Add(play[1], count);

                user.TotalPlays += count;
            }

            return user;
        }

        public int CompareTo(User other)
        {
            var i = this.Plays.Keys;
            var j = other.Plays.Keys;

            var artists = i.Intersect<string>(j);

            return artists.Count();
        }
    }
}
