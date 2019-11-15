using System.Collections.Generic;

namespace SecretSanta
{
    public interface ISecretSantaGenerator
    {
        IDictionary<T, T> Generate<T>(IList<T> participants, bool excludeMutualPairing = false);
        IDictionary<T, T> Generate<T>(IList<T> participants, IDictionary<T, T> bannedPairings,
            bool excludeMutualPairing = false);
        IEnumerable<IDictionary<T, T>> GenerateAll<T>(IList<T> participants, bool excludeMutualPairing = false);
        IEnumerable<IDictionary<T, T>> GenerateAll<T>(IList<T> participants, IDictionary<T, T> bannedPairings,
            bool excludeMutualPairing = false);
    }
}