using SecretSanta.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta
{
    public class SecretSantaGenerator : ISecretSantaGenerator
    {
        public IDictionary<T, T> Generate<T>(IEnumerable<T> participants, bool excludeMutualPairing = false)
        {
            return this.Generate(participants, new Dictionary<T, T>(), excludeMutualPairing);
        }

        public IDictionary<T, T> Generate<T>(IEnumerable<T> participants, IDictionary<T, T> bannedPairings, bool excludeMutualPairing = false)
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

        public IEnumerable<IDictionary<T, T>> GenerateAll<T>(IEnumerable<T> participants, bool excludeMutualPairing = false)
        {
            return this.GenerateAll(participants, new Dictionary<T, T>(), excludeMutualPairing);
        }

        public IEnumerable<IDictionary<T, T>> GenerateAll<T>(IEnumerable<T> participants, IDictionary<T, T> bannedPairings, bool excludeMutualPairing = false)
        {
            return this.GenerateResults(participants, bannedPairings, false, excludeMutualPairing);
        }

        private IEnumerable<IDictionary<T, T>> GenerateResults<T>(IEnumerable<T> participants, IDictionary<T, T> bannedPairings,
            bool getJustOneResult, bool excludeMutualPairing = false)
        {
            participants = participants.ToList();
            var to = participants.GetShuffle();
            foreach (var permutation in participants.GetShuffle().GetPermutations())
            {
                var permutationsDictionary = to.MergeToKeyValuePair(permutation).ToDictionary();

                if (this.PairingIsValid(bannedPairings, permutationsDictionary, excludeMutualPairing))
                {
                    yield return permutationsDictionary;
                    if (getJustOneResult)
                    {
                        yield break;
                    }
                }
            }
        }

        private bool PairingIsValid<T>(IDictionary<T, T> bannedPairings, IDictionary<T, T> permutations, bool excludeMutualPairing)
        {
            var result = !permutations.Any(r => r.Key.Equals(r.Value) || bannedPairings.Contains(r));

            if (!excludeMutualPairing)
            {
                return result;
            }

            return !permutations
                       .Select(kvp => permutations.Any(x => x.Key.Equals(kvp.Value) && x.Value.Equals(kvp.Key)))
                       .Any(mutualPairing => mutualPairing)
                   && result;
        }
    }
}
