using SecretSanta.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta
{
    public class SecretSantaGenerator : ISecretSantaGenerator
    {
        public IDictionary<T, T> Generate<T>(IList<T> participants, bool excludeMutualPairing = false)
        {
            return this.Generate(participants, new Dictionary<T, T>(), excludeMutualPairing);
        }

        public IDictionary<T, T> Generate<T>(IList<T> participants, IDictionary<T, T> bannedPairings, bool excludeMutualPairing = false)
        {
            var results = this.GenerateResults(participants, bannedPairings, true, excludeMutualPairing);
            try
            {
                return results.Single();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("No valid santa list can be generated");
            }
        }

        public IEnumerable<IDictionary<T, T>> GenerateAll<T>(IList<T> participants, bool excludeMutualPairing = false)
        {
            return this.GenerateAll(participants, new Dictionary<T, T>(), excludeMutualPairing);
        }

        public IEnumerable<IDictionary<T, T>> GenerateAll<T>(IList<T> participants, IDictionary<T, T> bannedPairings, bool excludeMutualPairing = false)
        {
            return this.GenerateResults(participants, bannedPairings, false, excludeMutualPairing);
        }

        private IEnumerable<IDictionary<T, T>> GenerateResults<T>(IList<T> participants, IDictionary<T, T> bannedPairings,
            bool getJustOneResult, bool excludeMutualPairing = false)
        {
            var to = participants.GetShuffle();

            foreach (var from in participants.GetShuffle().GetPermutations())
            {
                var result = to.MergeToKeyValuePair(from).ToList();

                if (this.PairingIsValid(bannedPairings, result, excludeMutualPairing))
                {
                    yield return result.ToDictionary();
                    if (getJustOneResult)
                    {
                        yield break;
                    }
                }
            }
        }

        private bool PairingIsValid<T>(IDictionary<T, T> bannedPairings, IEnumerable<KeyValuePair<T, T>> result, bool excludeMutualPairing)
        {
            return result.All(r => !r.Key.Equals(r.Value)
                                   && !bannedPairings.Contains(r)
                                   && !(excludeMutualPairing && this.IsMutualPairing(r)));
        }

        private bool IsMutualPairing<T>(KeyValuePair<T, T> r)
        {
            return r.Equals(new KeyValuePair<T, T>(r.Value, r.Key));
        }
    }
}
