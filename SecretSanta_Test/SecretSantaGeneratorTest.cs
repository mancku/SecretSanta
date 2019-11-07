using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta;
using System.Collections.Generic;

namespace SecretSanta_Test
{
    [TestClass]
    public class SecretSantaGeneratorTest
    {
        private IList<Participant> participants;
        private IDictionary<Participant, Participant> banned;
        private SecretSantaGenerator secretSantaGenerator;

        [TestInitialize]
        public void SetUp()
        {
            this.participants = new List<Participant>()
            {
                new Participant() { FirstName = "A" },
                new Participant() { FirstName = "B" },
                new Participant() { FirstName = "C" },
                new Participant() { FirstName = "D" }
            };

            this.banned = new Dictionary<Participant, Participant>
            {
                { this.participants[0], this.participants[2] },
                { this.participants[1], this.participants[3] }
            };

            this.secretSantaGenerator = new SecretSantaGenerator();
        }

        [TestMethod]
        public void SecretSanta_Generate_ReturnsASet()
        {
            var result = this.secretSantaGenerator.Generate(this.participants);

            this.CheckForValidSantaList(result);
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

        private void CheckResultHasNoBannedPair(IDictionary<Participant, Participant> result)
        {
            foreach (var bannedPair in this.banned)
            {
                Assert.IsFalse(result.Contains(bannedPair));
            }
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
    }
}
