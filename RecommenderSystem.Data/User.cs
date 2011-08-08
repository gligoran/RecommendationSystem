using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace RecommenderSystem.Data
{
    public class User
    {
        [XmlAttribute]
        public string UserId { get; set; }

        [XmlElement]
        public Sexes? Sex { get; set; }

        [XmlElement]
        public int? Age { get; set; }

        [XmlElement]
        public string Country { get; set; }

        [XmlElement]
        public DateTime JoinDate { get; set; }

        [XmlArray]
        public PlayCounts PlayCounts { get; set; }

        public User() { }

        public User(string userId, string country, DateTime joinDate, Sexes? sex = null, int? age = null)
        {
            this.UserId = userId;
            this.Country = country;
            this.JoinDate = joinDate;
            this.Sex = sex;
            this.Age = age;

            this.PlayCounts = new PlayCounts();
        }
    }

    public enum Sexes
    {
        Male,
        Female
    }
}
