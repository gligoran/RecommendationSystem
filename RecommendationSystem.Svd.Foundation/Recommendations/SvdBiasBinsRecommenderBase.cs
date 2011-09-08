using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Prediction;
using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.Svd.Foundation.Recommendations
{
    public abstract class SvdBiasBinsRecommenderBase<TSvdBiasBinsModel> : SvdRecommenderBase<TSvdBiasBinsModel>, ISvdBiasBinsRecommender<TSvdBiasBinsModel>
        where TSvdBiasBinsModel : ISvdBiasBinsModel
    {
        public IBiasBinsAdjustor<TSvdBiasBinsModel> BiasBinsAdjustor { get; set; }

        protected SvdBiasBinsRecommenderBase(IPredictor<TSvdBiasBinsModel> predictor, IBiasBinsAdjustor<TSvdBiasBinsModel> biasBinsAdjustor)
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