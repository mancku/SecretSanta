using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta_Test
{
    [TestClass]
    public class SecretSantaExtentionsTest
    {

        private readonly Random Randomizer = new Random(DateTime.Now.Millisecond);

        private IList<Participant> testList;

        [TestInitialize]
        public void SetUp()
        {
            this.testList = new List<Participant>();
            for (var i = 0; i < this.Randomizer.Next(5, 9); i++)
            {
                this.testList.Add(new Participant { FirstName = $"{Faker.Name.First()} {i}", LastName = Faker.Name.Last() });
            }
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
            var pairs = this.GetEnumKVPairs().ToList();
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
    }
}
