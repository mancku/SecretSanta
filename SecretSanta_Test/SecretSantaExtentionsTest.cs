using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta_Test
{
    [TestClass]
    public class SecretSantaExtentionsTest : BaseSecretSantaTest
    {
        protected IDictionary<Participant, Participant> banned;

        [TestMethod]
        public void Helpers_GetShuffle_AllReturned_1000Tries()
        {
            for (var i = 0; i < 1000; i++)
            {
                var result = this.participants.GetShuffle();

                foreach (var a in this.participants)
                {
                    Assert.IsTrue(result.Contains(a));
                }
            }
        }

        [TestMethod]
        public void Helpers_GetPermutations_AllPermutationsReturned()
        {
            var result = this.participants.GetPermutations().Count();
            var expected = this.Factoral(this.participants.Count());

            Assert.AreEqual(expected, result, "There should be n! permutations, where n = {0}", this.participants.Count());
        }

        [TestMethod]
        public void Helpers_GetPermutations_AllUnique()
        {
            var result = this.participants.GetPermutations().ToList();

            for (var currentIndex = 0; currentIndex < result.Count; currentIndex++)
            {
                for (var nextIndex = currentIndex + 1; nextIndex < result.Count; nextIndex++)
                {
                    var current = result[currentIndex];
                    var next = result[nextIndex];
                    Assert.AreEqual(current.Count, next.Count, "All lists should have the same number of elements");
                    this.CheckOrderingIsDifferent(current, next);
                }
            }
        }

        [TestMethod]
        public void Helpers_ToDictionary_ReturnsDictionary()
        {
            var pairs = this.GetEnumKVPairs().ToList();
            var result = pairs.ToDictionary();

            Assert.AreEqual(pairs.Count(), result.Count);

            foreach (var pair in pairs)
            {
                Assert.IsTrue(result.Contains(pair));
            }
        }

        [TestMethod]
        public void Helpers_MergeToKeyValuePair_ReturnsValidIEnumerable()
        {
            var numberListKeys = new List<int>();
            var numberListValues = new List<int>();
            for (var i = 0; i < this.Randomizer.Next(5, 10); i++)
            {
                numberListKeys.Add(this.Randomizer.Next(1, 10));
                numberListValues.Add(this.Randomizer.Next(1, 10));
            }
            var result = numberListKeys.MergeToKeyValuePair(numberListValues).ToList();

            Assert.AreEqual(numberListKeys.Count, numberListValues.Count, result.Count(), "Zipped list should eb same length");

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i].Key, numberListKeys[i], "Values did not match");
                Assert.AreEqual(result[i].Value, numberListValues[i], "Values did not match");
            }
        }

        private void CheckOrderingIsDifferent<T>(IList<T> current, IList<T> next)
        {
            var differenceDetected = current.Where((element, index) => !element.Equals(next[index])).Any();
            Assert.IsTrue(differenceDetected, "No difference was found");
        }

        private IEnumerable<KeyValuePair<Participant, Participant>> GetEnumKVPairs()
        {
            for (var i = 0; i < this.participants.Count; i++)
            {
                if (i < this.participants.Count - 1)
                {
                    yield return new KeyValuePair<Participant, Participant>(this.participants[i], this.participants[i + 1]);
                }
                else
                {
                    yield return new KeyValuePair<Participant, Participant>(this.participants[i], this.participants[0]);
                }
            }
        }

        private int Factoral(int n)
        {
            if (n <= 1)
            {
                return 1;
            }

            return n * this.Factoral(n - 1);
        }

        protected override void SpecificTestSetup()
        {
            this.banned = new Dictionary<Participant, Participant>
            {
                {this.participants[0], this.participants[2]},
                {this.participants[1], this.participants[3]}
            };
        }
    }
}
