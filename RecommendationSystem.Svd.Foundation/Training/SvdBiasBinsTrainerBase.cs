using System.Collections.Generic;
using RecommendationSystem.Data;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Training;

namespace RecommendationSystem.Svd.Foundation.Training
{
    public abstract class SvdBiasBinsTrainerBase<TBiasBinsSvdModel> : SvdTrainerBase<TBiasBinsSvdModel>, ISvdBiasBinsTrainer<TBiasBinsSvdModel>
        where TBiasBinsSvdModel : ISvdBiasBinsModel
    {
        public IBiasBinsCalculator<TBiasBinsSvdModel> BiasBinsCalculator { get; set; }

        protected SvdBiasBinsTrainerBase(IBiasBinsCalculator<TBiasBinsSvdModel> biasBinsCalculator)
        {
            BiasBinsCalculator = biasBinsCalculator;
            ModelSaver.ModelPartSavers.Add(new BiasBinsModelPartSaver());
        }

        public new TBiasBinsSvdModel TrainModel(List<IUser> users, List<IArtist> artists, List<IRating> ratings)
        {
            return TrainModel(users, artists, ratings, new TrainingParameters());
        }

        public new TBiasBinsSvdModel TrainModel(List<IUser> users, List<IArtist> artists, List<IRating> ratings, TrainingParameters trainingParameters)
        {
            var model = TrainModel(users.GetLookupTable(), artists.GetLookupTable(), ratings, trainingParameters);
            BiasBinsCalculator.CalculateBiasBins(model, ratings, users, artists, trainingParameters.BiasBinCount);
            return model;
        }
    }
}