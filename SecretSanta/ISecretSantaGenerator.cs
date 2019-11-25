using System;
using System.Collections.Generic;
using SecretSanta.BindingModels;

namespace SecretSanta
{
    public interface ISecretSantaGenerator
    {
        IDictionary<T, T> Generate<T>(IEnumerable<T> participants, bool excludeMutualPairing = false)
            where T : Participant, IEquatable<T>;
        IDictionary<T, T> Generate<T>(IEnumerable<T> participants, IDictionary<T, T> bannedPairings,
            bool excludeMutualPairing = false)
            where T : Participant, IEquatable<T>;
        IEnumerable<IDictionary<T, T>> GenerateAll<T>(IEnumerable<T> participants, bool excludeMutualPairing = false)
            where T : Participant, IEquatable<T>;
        IEnumerable<IDictionary<T, T>> GenerateAll<T>(IEnumerable<T> participants, IDictionary<T, T> bannedPairings,
            bool excludeMutualPairing = false)
            where T : Participant, IEquatable<T>;

    }
}