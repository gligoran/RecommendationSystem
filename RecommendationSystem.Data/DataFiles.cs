namespace RecommendationSystem.Data
{
    public static class DataFiles
    {
        //original
        public const string RatingDataset = @"D:\Dataset\ratings.tsv";
        public const string UserDataset = @"D:\Dataset\users.tsv";

        //ratings
        public const string Playcounts = @"D:\Dataset\playcounts.rs";
        public const string NormalizedRatings = @"D:\Dataset\ratings-normalized.rs";
        public const string LogNormalizedRatings = @"D:\Dataset\ratings-log-normalized.rs";
        public const string EqualFerquencyFiveScaleRatings = @"D:\Dataset\ratings-fivescale-ef.rs";
        public const string EqualWidthFiveScaleRatings = @"D:\Dataset\ratings-fivescale-ew.rs";
        public const string LogEqualWidthFiveScaleRatings = @"D:\Dataset\ratings-log-fivescale-ew.rs";

        //users
        public const string Users = @"D:\Dataset\users.rs";

        //artists
        public const string Artists = @"D:\Dataset\artists.rs";

        //train
        public const string TrainUsers = @"D:\Dataset\train\users.rs";
        public const string TrainPlaycounts = @"D:\Dataset\train\playcounts.rs";
        public const string TrainNormalizedRatings = @"D:\Dataset\train\ratings-normalized.rs";
        public const string TrainLogNormalizedRatings = @"D:\Dataset\train\ratings-log-normalized.rs";
        public const string TrainEqualFerquencyFiveScaleRatings = @"D:\Dataset\train\ratings-fivescale-ef.rs";
        public const string TrainEqualWidthFiveScaleRatings = @"D:\Dataset\train\ratings-fivescale-ew.rs";
        public const string TrainLogEqualWidthFiveScaleRatings = @"D:\Dataset\train\ratings-log-fivescale-ew.rs";

        //test
        public const string TestUsers = @"D:\Dataset\test\users.rs";
        public const string TestPlaycounts = @"D:\Dataset\test\playcounts.rs";
        public const string TestNormalizedRatings = @"D:\Dataset\test\ratings-normalized.rs";
        public const string TestLogNormalizedRatings = @"D:\Dataset\test\ratings-log-normalized.rs";
        public const string TestEqualFerquencyFiveScaleRatings = @"D:\Dataset\test\ratings-fivescale-ef.rs";
        public const string TestEqualWidthFiveScaleRatings = @"D:\Dataset\test\ratings-fivescale-ew.rs";
        public const string TestLogEqualWidthFiveScaleRatings = @"D:\Dataset\test\ratings-log-fivescale-ew.rs";
    }
}