using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RecommendationSystem.Data
{
    public class Artist
    {
        [Key]
        public string Name { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public Artist()
        { }

        public Artist(string name)
            : this()
        {
            this.Name = name;
        }
    }
}
