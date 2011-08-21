using System;
using System.Collections.Generic;

namespace RecommendationSystem.Entities
{
    public interface IUser : IComparable<User>, IEquatable<User>, IComparable
    {
        string UserId { get; set; }
        List<IRating> Ratings { get; set; }
        string Gender { get; set; }
        int Age { get; set; }
        string Country { get; set; }
        DateTime SignUp { get; set; }
    }
}