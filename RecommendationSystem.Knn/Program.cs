using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RecommendationSystem.Data;
using RecommendationSystem.Knn.RatingAggregation;
using RecommendationSystem.Knn.Recommendations;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Training;

namespace RecommendationSystem.Knn
{
    public static class Program
    {
        private static readonly Stopwatch timer = new Stopwatch();
        private const string myUserId = "cb732aa2abb82e9527716dc9f083110b22265380";

        private static void Main()
        {
            timer.Start();
            List<string> userLut,
                         artistLut;
            var users = UserProvider.Load(DataFiles.Users, out userLut);
            var artists = ArtistProvider.Load(DataFiles.Artists, out artistLut);
            users.PopulateWithRatings(DataFiles.EqualFerquencyFiveScaleRatings);
            timer.Stop();
            Console.WriteLine("{0} users loaded in {1}ms.", users.Count(), timer.ElapsedMilliseconds);

            var me = users[userLut.BinarySearch(myUserId)];

            var lp = artistLut.BinarySearch("linkin park");
            me.Ratings.Remove(me.Ratings.First(rating => rating.ArtistIndex == lp));

            var trainer = new KnnTrainer();
            var model = trainer.TrainModel(users, null, null);

            var cosineSimilarityEstimator = new CosineSimilarityEstimator();
            var simpleAverageRatingAggregator = new SimpleAverageRatingAggregator();
            var contentSimilarityEstimator = new ContentSimilarityEstimator();
            var weightedSumRatingAggregator = new WeightedSumRatingAggregator();
            var adjustedWeightedSumRatingAggregator = new AdjustedWeightedSumRatingAggregator();
            var pearsonSimilarityEstimator = new PearsonSimilarityEstimator();

            var recommender = new ContentKnnRecommender(cosineSimilarityEstimator, simpleAverageRatingAggregator, contentSimilarityEstimator, contentSimilarityWeight: 10.0f, ratingSimilarityWeight: 2.0f);
            var recommendations = recommender.GenerateRecommendations(me, model, artists);
            Debug.WriteLine("Cosine, SimpleAverage:");
            foreach (var recommendation in recommendations)
                Debug.WriteLine("- {0}", recommendation);

            recommender = new ContentKnnRecommender(cosineSimilarityEstimator, weightedSumRatingAggregator, contentSimilarityEstimator);
            recommendations = recommender.GenerateRecommendations(me, model, artists);
            Debug.WriteLine("Cosine, WeightedSum:");
            foreach (var recommendation in recommendations)
                Debug.WriteLine("- {0}", recommendation);

            recommender = new ContentKnnRecommender(cosineSimilarityEstimator, adjustedWeightedSumRatingAggregator, contentSimilarityEstimator);
            recommendations = recommender.GenerateRecommendations(me, model, artists);
            Debug.WriteLine("Cosine, AdjustedWeightedSum:");
            foreach (var recommendation in recommendations)
                Debug.WriteLine("- {0}", recommendation);

            recommender = new ContentKnnRecommender(pearsonSimilarityEstimator, simpleAverageRatingAggregator, contentSimilarityEstimator);
            recommendations = recommender.GenerateRecommendations(me, model, artists);
            Debug.WriteLine("Pearson, SimpleAverage:");
            foreach (var recommendation in recommendations)
                Debug.WriteLine("- {0}", recommendation);

            recommender = new ContentKnnRecommender(pearsonSimilarityEstimator, weightedSumRatingAggregator, contentSimilarityEstimator);
            recommendations = recommender.GenerateRecommendations(me, model, artists);
            Debug.WriteLine("Pearson, WeightedSum:");
            foreach (var recommendation in recommendations)
                Debug.WriteLine("- {0}", recommendation);

            recommender = new ContentKnnRecommender(pearsonSimilarityEstimator, adjustedWeightedSumRatingAggregator, contentSimilarityEstimator);
            recommendations = recommender.GenerateRecommendations(me, model, artists);
            Debug.WriteLine("Pearson, AdjustedWeightedSum:");
            foreach (var recommendation in recommendations)
                Debug.WriteLine("- {0}", recommendation);

            Console.ReadLine();
        }
    }
}