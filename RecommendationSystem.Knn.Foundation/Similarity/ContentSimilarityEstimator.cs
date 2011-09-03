using System;
using RecommendationSystem.Entities;

namespace RecommendationSystem.Knn.Foundation.Similarity
{
    public class ContentSimilarityEstimator : IContentSimilarityEstimator
    {
        public float GenderWeight { get; set; }
        public float CountryWeight { get; set; }
        public float AgeWeight { get; set; }

        public ContentSimilarityEstimator(float genderWeight = 1.0f, float countryWeight = 1.0f, float ageWeight = 1.0f)
        {
            GenderWeight = genderWeight / (genderWeight + countryWeight + ageWeight);
            CountryWeight = countryWeight / (genderWeight + countryWeight + ageWeight);
            AgeWeight = ageWeight / (genderWeight + countryWeight + ageWeight);
        }

        public float GetSimilarity(IUser first, IUser second)
        {
            var r = 1.0f;
            r -= first.Gender == second.Gender ? 0.0f : GenderWeight;
            r -= first.Country == second.Country ? 0.0f : CountryWeight;
            if (first.Age > 0 && second.Age > 0)
                r -= AgeWeight * Math.Abs(first.Age - second.Age) / ((float)first.Age * second.Age);

            return r;
        }
    }
}