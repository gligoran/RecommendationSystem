using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RecommendationSystem.Data
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }

        public User()
        { }

        public User(string userId)
            : this()
        {
            this.UserId = userId;
            this.Ratings = new List<Rating>();
        }
    }
}
