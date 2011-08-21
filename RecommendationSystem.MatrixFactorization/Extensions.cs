namespace RecommendationSystem.MatrixFactorization
{
    public static class Extensions
    {
        public static void Populate<T>(this T[,] array, T defaultVaue)
        {
            if (array == null)
                return;

            for (var i = 0; i <= array.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= array.GetUpperBound(1); j++)
                    array[i, j] = defaultVaue;
            }
        }
    }
}