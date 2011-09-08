using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Training;

namespace RecommendationSystem.Naive.MostCommonRating
{
    public class MostCommonRatingTrainer : ITrainer<IMostCommonRatingModel>
    {
        public IMostCommonRatingModel TrainModel(List<IUser> trainUsers, List<IArtist> artists, List<IRating> trainRatings)
        {
            if (trainRatings == null)
                return new MostCommonRatingModel(0.0f);

            var ratingGroups = new Dictionary<float, int>();
            foreach (var rating in trainRatings)
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