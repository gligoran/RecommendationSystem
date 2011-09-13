using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Prediction;
using RecommendationSystem.SvdBoostedKnn.Models;

namespace RecommendationSystem.SvdBoostedKnn.Training
{
    public interface IKnnTrainerForSvdModels<TSvdBoostedKnnModel>
        where TSvdBoostedKnnModel : ISvdBoostedKnnModel
    {
        INewUserFeatureGenerator<TSvdBoostedKnnModel> NewUserFeatureGenerator { get; set; }
        TSvdBoostedKnnModel TrainKnnModel(string filename, List<IUser> trainUsers);
        TSvdBoostedKnnModel TrainKnnModel(TSvdBoostedKnnModel model, List<IUser> trainUsers);
        TSvdBoostedKnnModel LoadSvdModel(string filename);
    }
}