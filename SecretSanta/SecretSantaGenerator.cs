using SecretSanta.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta
{
    public class SecretSantaGenerator
    {
        public IDictionary<T, T> Generate<T>(IList<T> participants)
        {
            return this.Generate(participants, new Dictionary<T, T>());
        }

        public IDictionary<T, T> Generate<T>(IList<T> participants, IDictionary<T, T> bannedPairings)
        {
            var to = participants.GetShuffle();

            foreach (var from in participants.GetShuffle().GetPermutations())
            {
                var result = to.ZipToKV(from);

                if (this.PairingIsValid(bannedPairings, result))
                {
                    return result.ToDictionary();
                }
            }

            throw new ApplicationException("No valid santa list can be generated");
        }

        private bool PairingIsValid<T>(IDictionary<T, T> bannedPairings, IEnumerable<KeyValuePair<T, T>> result)
        {
            foreach (var r in result)
            {
                if (r.Key.Equals(r.Value) || bannedPairings.Contains(r))
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerable<IDictionary<T, T>> GenerateAll<T>(IList<T> participants)
        {
            return this.GenerateAll(participants, new Dictionary<T, T>());
        }

        public IEnumerable<IDictionary<T, T>> GenerateAll<T>(IList<T> participants, IDictionary<T, T> bannedPairings)
        {
            var to = participants.GetShuffle();

            foreach (var from in participants.GetShuffle().GetPermutations())
            {
                var result = to.ZipToKV(from);

                if (this.PairingIsValid(bannedPairings, result))
                {
                    yield return result.ToDictionary();
                }
            }
        }
    }
}
