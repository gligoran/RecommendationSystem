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
    }
}