using System;
using System.Collections.Generic;
using System.Linq;

namespace RecommendationSystem.Entities
{
    public class User : IUser
    {
        public string UserId { get; set; }
        public List<IRating> Ratings { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public DateTime SignUp { get; set; }

        #region Costructor
        public User(string userId)
        {
            UserId = userId;
            Ratings = new List<IRating>();
        }

        public User(string userId, DateTime signUp, string gender = "", int age = -1, string country = "")
            : this(userId)
        {
            Gender = gender;
            Age = age;
            Country = country;
            SignUp = signUp;
        }
        #endregion

        #region IComparable
        public int CompareTo(User other)
        {
            return UserId.CompareTo(other.UserId);
        }

        public int CompareTo(object other)
        {
            if (other == null)
                return 1;

            if (!(other is User))
                throw new ArgumentException("Argument must be of type User");

            return CompareTo((User)other);
        }
        #endregion

        #region Operators
        public static bool operator ==(User first, User second)
        {
            if (ReferenceEquals(null, first))
                return false;
            if (ReferenceEquals(null, second))
                return false;
            if (ReferenceEquals(first, second))
                return true;

            return first.UserId == second.UserId;
        }

        public static bool operator !=(User first, User second)
        {
            return !(first == second);
        }
        #endregion

        #region Equals
        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Equals(UserId, other.UserId);
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (other.GetType() != typeof(User))
                return false;
            return Equals((User)other);
        }

        public override int GetHashCode()
        {
            return (UserId != null ? UserId.GetHashCode() : 0);
        }
        #endregion
    }
}