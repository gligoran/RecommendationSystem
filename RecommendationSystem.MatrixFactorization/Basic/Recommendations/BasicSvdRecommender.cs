using System;
using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Basic.Models;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.MatrixFactorization.Basic.Recommendations
{
    public class BasicSvdRecommender : IRecommender<IBasicSvdModel>
    {
        public float PredictRatingForArtist(IUser user, IBasicSvdModel model, List<IArtist> artists, int artist)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, IBasicSvdModel model, List<IArtist> artists)
        {
            throw new NotImplementedException();
        }
    }
}