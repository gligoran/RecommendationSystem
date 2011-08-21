using System.Collections.Generic;

namespace RecommendationSystem.Data.Entities
{
    public class Artist : IArtist
    {
        public string Name { get; set; }
        public List<IRating> Ratings { get; set; }

        public Artist(string name)
        {
            Name = name;
            Ratings = new List<IRating>();
        }

        #region IComparable
        public int CompareTo(IArtist other)
        {
            return Name.CompareTo(other.Name);
        }
        #endregion

        #region Operators
        public static bool operator ==(Artist first, Artist second)
        {
            if (ReferenceEquals(null, first))
                return false;
            if (ReferenceEquals(null, second))
                return false;
            if (ReferenceEquals(first, second))
                return true;

            return first.Name == second.Name;
        }

        public static bool operator !=(Artist first, Artist second)
        {
            return !(first == second);
        }
        #endregion

        #region Equals
        public bool Equals(IArtist other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Equals(Name, other.Name);
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (other.GetType() != typeof(IArtist))
                return false;
            return Equals((IArtist)other);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
        #endregion
    }
}