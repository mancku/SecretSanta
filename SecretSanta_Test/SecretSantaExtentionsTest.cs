using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Extentions;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta_Test
{
    [TestClass]
    public class SecretSantaExtentionsTest
    {
        private IList<Participant> testList;

        [TestInitialize]
        public void SetUp()
        {
            this.testList = new List<Participant>
            {
                new Participant() { FirstName = "Name 1", LastName = "Last" },
                new Participant() { FirstName = "Name 2", LastName = "Last" },
                new Participant() { FirstName = "Name 3", LastName = "Last" },
                new Participant() { FirstName = "Name 4", LastName = "Last" },
                new Participant() { FirstName = "Name 5", LastName = "Last" }
            };
        }

        [TestMethod]
        public void Helpers_GetShuffle_AllReturned_1000Tries()
        {
            for (var i = 0; i < 1000; i++)
            {
                var result = this.testList.GetShuffle();

                foreach (var a in this.testList)
                {
                    Assert.IsTrue(result.Contains(a));
                }
            }
        }

        [TestMethod]
        public void Helpers_GetPermutations_AllPermutationsReturned()
        {
            var result = this.testList.GetPermutations().Count();
            var expected = this.Factoral(this.testList.Count());

            Assert.AreEqual(expected, result, "There should be n! permutations, where n = {0}", this.testList.Count());
        }

        private int Factoral(int n)
        {
            if (n <= 1)
            {
                return 1;
            }

            return n * this.Factoral(n - 1);
        }

        [TestMethod]
        public void Helpers_GetPermutations_AllUnique()
        {
            var result = this.testList.GetPermutations().ToList();

            for (var current = 0; current < result.Count; current++)
            {
                for (var compare = current + 1; compare < result.Count; compare++)
                {
                    Assert.AreEqual(result[current].Count, result[compare].Count, "All lists should have the same number of elements");
                    this.CheckOrderingIsDifferent(result[current], result[compare]);
                }
            }
        }

        private void CheckOrderingIsDifferent<T>(IList<T> first, IList<T> second)
        {
            var differenceDetected = false;
            for (var i = 0; i < first.Count; i++)
            {
                if (first[i].Equals(second[i]))
                {
                    differenceDetected = true;
                    break;
                }
            }

            Assert.IsTrue(differenceDetected, "No difference was found");
        }

        [TestMethod]
        public void Helpers_ToDictionary_ReturnsDictionary()
        {
            var pairs = this.GetEnumKVPairs();
            var result = pairs.ToDictionary();

            Assert.AreEqual(pairs.Count(), result.Count);

            foreach (var pair in pairs)
            {
                Assert.IsTrue(result.Contains(pair));
            }
        }

        private IEnumerable<KeyValuePair<Participant, Participant>> GetEnumKVPairs()
        {
            for (var i = 0; i < this.testList.Count; i++)
            {
                if (i < this.testList.Count - 1)
                {
                    yield return new KeyValuePair<Participant, Participant>(this.testList[i], this.testList[i + 1]);
                }
                else
                {
                    yield return new KeyValuePair<Participant, Participant>(this.testList[i], this.testList[0]);
                }
            }
        }

        [TestMethod]
        public void Helpers_MergeToKeyValuePair_ReturnsValidIEnumerable()
        {
            var numberList = new List<int>() { 1, 2, 3, 4, 5 };
            var result = numberList.MergeToKeyValuePair(numberList);

            Assert.AreEqual(numberList.Count, result.Count(), "Zipped list should eb same length");

            foreach (var pair in result)
            {
                Assert.AreEqual(pair.Key, pair.Value, "Values did not match");
            }
        }
    }
}
