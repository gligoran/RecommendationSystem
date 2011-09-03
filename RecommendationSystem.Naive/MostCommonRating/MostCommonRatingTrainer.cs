using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Training;

namespace RecommendationSystem.Naive.MostCommonRating
{
    public class MostCommonRatingTrainer : ITrainer<IMostCommonRatingModel, IUser>
    {
        public IMostCommonRatingModel TrainModel(List<IUser> users, List<IArtist> artists, List<IRating> ratings)
        {
            if (ratings == null)
                return new MostCommonRatingModel(0.0f);

            var ratingGroups = new Dictionary<float, int>();
            foreach (var rating in ratings)
            {
                if (ratingGroups.ContainsKey(rating.Value))
                    ratingGroups[rating.Value]++;
                else
                    ratingGroups.Add(rating.Value, 1);
            }

            var mostCommon = float.MinValue;
            foreach (var ratingGroup in ratingGroups)
            {
                if (ratingGroup.Value > mostCommon)
                    mostCommon = ratingGroup.Key;
            }

            return new MostCommonRatingModel(mostCommon);
        }
    }
}