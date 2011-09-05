using System;
using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Prediction;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Prediction;

namespace RecommendationSystem.SimpleSvd.Recommendation
{
    public abstract class SimpleSvdRecommenderBase<TSvdModel> : ISimpleSvdRecommender<TSvdModel>
        where TSvdModel : ISvdModel
    {
        public IPredictor<TSvdModel> Predictor { get; set; }
        public INewUserFeatureGenerator<TSvdModel> NewUserFeatureGenerator { get; set; }

        protected ModelLoader<TSvdModel> ModelLoader { get; set; }

        public void LoadModel(TSvdModel model, string filename)
        {
            ModelLoader.LoadModel(model, filename);
        }

        protected SimpleSvdRecommenderBase(IPredictor<TSvdModel> predictor)
        {
            Predictor = predictor;

            ModelLoader = new ModelLoader<TSvdModel>();
            ModelLoader.ModelPartLoaders.Add(new SvdModelPartLoader());
        }

        public float PredictRatingForArtist(IUser user, TSvdModel model, List<IArtist> artists, int artistIndex)
        {
            return Predictor.PredictRatingForArtist(user, model, artists, artistIndex);
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, TSvdModel model, List<IArtist> artists)
        {
            throw new NotImplementedException();
        }
    }
}