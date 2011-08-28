using System;
using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Bias.Models;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.MatrixFactorization.Bias.Recommendations
{
    public class BiasSvdRecommender : IRecommender<IBiasSvdModel>
    {
        public float PredictRatingForArtist(IUser user, IBiasSvdModel model, List<IArtist> artists, int artist)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, IBiasSvdModel model, List<IArtist> artists)
        {
            throw new NotImplementedException();
        }
    }
}