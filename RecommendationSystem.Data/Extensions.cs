using System.Collections.Generic;

namespace RecommendationSystem.Data
{
    public static class Extensions
    {
        public static IEnumerable<int> IntersectSorted(this IEnumerable<int> first, IEnumerable<int> second)
        {
            using (var firstCursor = first.GetEnumerator())
            using (var secondCursor = second.GetEnumerator())
            {
                if (!firstCursor.MoveNext() || !secondCursor.MoveNext())
                    yield break;

                var firstValue = firstCursor.Current;
                var secondValue = secondCursor.Current;
                while (true)
                {
                    var comparison = firstValue.CompareTo(secondValue);
                    if (comparison < 0)
                    {
                        if (!firstCursor.MoveNext())
                            yield break;

                        firstValue = firstCursor.Current;
                    }
                    else if (comparison > 0)
                    {
                        if (!secondCursor.MoveNext())
                            yield break;

                        secondValue = secondCursor.Current;
                    }
                    else
                    {
                        yield return firstValue;
                        if (!firstCursor.MoveNext() || !secondCursor.MoveNext())
                            yield break;

                        firstValue = firstCursor.Current;
                        secondValue = secondCursor.Current;
                    }
                }
            }
        }
    }
}