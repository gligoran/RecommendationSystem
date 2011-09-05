using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Prediction;
using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.SimpleSvd.Recommendation
{
    public abstract class SimpleSvdBiasBinsRecommenderBase<TSvdBiasBinsModel> : SimpleSvdRecommenderBase<TSvdBiasBinsModel>, ISimpleSvdBiasBinsRecommender<TSvdBiasBinsModel>
        where TSvdBiasBinsModel : ISvdBiasBinsModel
    {
        public IBiasBinsAdjustor<TSvdBiasBinsModel> BiasBinsAdjustor { get; set; }

        protected SimpleSvdBiasBinsRecommenderBase(IPredictor<TSvdBiasBinsModel> predictor, IBiasBinsAdjustor<TSvdBiasBinsModel> biasBinsAdjustor)
            : base(predictor)
        {
            BiasBinsAdjustor = biasBinsAdjustor;
            ModelLoader.ModelPartLoaders.Add(new BiasBinsModelPartLoader());
        }

        public new float PredictRatingForArtist(IUser user, TSvdBiasBinsModel model, List<IArtist> artists, int artistIndex)
        {
            var rating = Predictor.PredictRatingForArtist(user, model, artists, artistIndex);
            return BiasBinsAdjustor.AdjustRating(rating, model);
        }
    }
}