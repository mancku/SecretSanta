using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Extentions
{
    public static class SantasLittleHelpers
    {
        public static IList<T> GetShuffle<T>(this IEnumerable<T> source)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            return source.OrderBy(x => rand.Next()).ToList();
        }

        // Extracted from https://stackoverflow.com/questions/15150147/all-permutations-of-a-list
        public static IEnumerable<IList<T>> GetPermutations<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                yield break;
            }

            var list = sequence.ToList();

            if (!list.Any())
            {
                yield return new List<T>();
            }
            else
            {
                var startingElementIndex = 0;

                foreach (var startingElement in list)
                {
                    var index = startingElementIndex;
                    var remainingItems = list.Where((e, i) => i != index);

                    foreach (var permutationOfRemainder in remainingItems.GetPermutations())
                    {
                        yield return startingElement.Concat(permutationOfRemainder).ToList();
                    }

                    startingElementIndex++;
                }
            }
        }

        private static IEnumerable<T> Concat<T>(this T firstElement, IEnumerable<T> secondSequence)
        {
            yield return firstElement;
            if (secondSequence == null)
            {
                yield break;
            }

            foreach (var item in secondSequence)
            {
                yield return item;
            }
        }

        private static void MoveLastItemToFirstPosition<T>(this IList<T> source)
        {
            var rotateBuffer = source[source.Count - 1];
            source.RemoveAt(source.Count - 1);
            source.Insert(0, rotateBuffer);
        }

        public static IDictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> source)
        {
            return source.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static IEnumerable<KeyValuePair<K, V>> MergeToKeyValuePair<K, V>(this IEnumerable<K> keys, IEnumerable<V> values)
        {
            return keys.Zip(values, (key, value) => new KeyValuePair<K, V>(key, value));
        }
    }
}
