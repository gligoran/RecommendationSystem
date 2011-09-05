using System;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Prediction;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Training;

namespace RecommendationSystem.Svd.Foundation.Training
{
    public class SvdBiasBinsCalculator<TSvdBiasBinsModel> : BiasBinsCalculatorBase<TSvdBiasBinsModel>
        where TSvdBiasBinsModel : ISvdBiasBinsModel
    {
        public SvdBiasBinsCalculator(IPredictor<TSvdBiasBinsModel> predictor)
            : base(predictor)
        {}

        #region CalculateBiasBins
        public override void CalculateBiasBins(TSvdBiasBinsModel model, List<IRating> ratings, List<IUser> users, List<IArtist> artists, int biasBinCount)
        {
            Console.WriteLine("Calculating BiasBins...");

            var biasBins = new float[biasBinCount];
            var biasBinsPopulation = new int[biasBinCount];

            var percent = users.Count / 100;
            for (var i = 0; i < users.Count; i++)
            {
                var user = users[i];
                lock (user)
                {
                    if (user.Ratings.Count > 1)
                    {
                        var originalRatings = user.Ratings;
                        foreach (var rating in user.Ratings)
                        {
                            user.Ratings = originalRatings.Where(r => r != rating).ToList();
                            var predictedRating = Predictor.PredictRatingForArtist(user, model, artists, rating.ArtistIndex);

                            var error = predictedRating - rating.Value;
                            var biasBinIndex = GetBiasBinIndex(predictedRating, biasBinCount);
                            biasBins[biasBinIndex] += error;
                            biasBinsPopulation[biasBinIndex]++;
                        }
                        user.Ratings = originalRatings;
                    }

                    if (i % percent == 0)
                        Console.WriteLine("BiasBins calculation at {0} ({1}%)", i, i / percent);
                }
            }

            for (var i = 0; i < biasBinCount; i++)
            {
                if (biasBinsPopulation[i] > 0)
                    biasBins[i] /= biasBinsPopulation[i];
                else
                    biasBins[i] = 0.0f;
            }

            model.BiasBins = biasBins;

            Console.WriteLine("BiasBins calculation complete");
        }
        #endregion
    }
}