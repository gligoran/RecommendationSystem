using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RecommendationSystem.Data
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Artist")]
        public string ArtistId { get; set; }

        public double Value { get; set; }

        public virtual User User { get; set; }
        public virtual Artist Artist { get; set; }
    }
}
