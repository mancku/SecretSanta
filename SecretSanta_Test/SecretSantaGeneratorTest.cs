using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta_Test
{
    [TestClass]
    public class SecretSantaGeneratorTest
    {
        private IList<Participant> participants;
        private IDictionary<Participant, Participant> banned;
        private SecretSantaGenerator secretSantaGenerator;
        private readonly Random Randomizer = new Random(DateTime.Now.Millisecond);

        [TestInitialize]
        public void SetUp()
        {
            this.participants = new List<Participant>();
            for (var i = 0; i < this.Randomizer.Next(5, 9); i++)
            {
                this.participants.Add(new Participant
                {
                    FirstName = $"{Faker.Name.First()} {i}",
                    LastName = Faker.Name.Last(),
                    EMailAddress = Faker.Internet.FreeEmail(),
                    PhoneNumber = Faker.Phone.Number()
                });
            }

            this.banned = new Dictionary<Participant, Participant>
            {
                {this.participants[0], this.participants[2]},
                {this.participants[1], this.participants[3]}
            };

            this.secretSantaGenerator = new SecretSantaGenerator();
        }

        [TestMethod]
        public void SecretSanta_Generate_ReturnsASet()
        {
            var result = this.secretSantaGenerator.Generate(this.participants);
            this.CheckForValidSantaList(result);
        }

        [TestMethod]
        public void SecretSanta_GernerateAll_ReturnsAllSets()
        {
            foreach (var list in this.secretSantaGenerator.GenerateAll(this.participants))
            {
                this.CheckForValidSantaList(list);
            }
        }

        [TestMethod]
        public void SecretSanta_Generate_WithBanned_ReturnsASet()
        {
            var result = this.secretSantaGenerator.Generate(this.participants, this.banned);

            this.CheckForValidSantaList(result);
            this.CheckResultHasNoBannedPair(result);
        }

        [TestMethod]
        public void SecretSanta_GenerateAll_WithBanned_ReturnsAllSets()
        {
            var result = this.secretSantaGenerator.GenerateAll(this.participants, this.banned);

            foreach (var list in result)
            {
                this.CheckForValidSantaList(list);
                this.CheckResultHasNoBannedPair(list);
            }
        }

        [TestMethod]
        public void SecretSanta_Excluded_Is_Working()
        {
            var excludedMutualPairing = this.secretSantaGenerator
                .GenerateAll(this.participants, true)
                .ToList();

            foreach (var dictionary in excludedMutualPairing)
            {
                foreach (var kvp in dictionary)
                {
                    var isMutualPairing = dictionary.Any(x => x.Key.Equals(kvp.Value) && x.Value.Equals(kvp.Key));
                    Assert.IsFalse(isMutualPairing);
                }
            }
        }

        private void CheckResultHasNoBannedPair(IDictionary<Participant, Participant> result)
        {
            foreach (var bannedPair in this.banned)
            {
                Assert.IsFalse(result.Contains(bannedPair));
            }
        }

        private void CheckForValidSantaList(IDictionary<Participant, Participant> santaList)
        {
            foreach (var sender in santaList.Keys)
            {
                Assert.IsTrue(this.participants.Contains(sender), "A participant was not included as a gifter");
            }

            foreach (var reciever in santaList.Values)
            {
                Assert.IsTrue(this.participants.Contains(reciever), "A participant was not included as a giftee");
            }

            foreach (var pair in santaList)
            {
                Assert.AreNotEqual(pair.Key, pair.Value, "A participant should never have to gift to themselves");
            }
        }
    }
}
