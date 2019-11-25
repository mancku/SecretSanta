using System.Collections.Generic;
using SecretSanta.BindingModels;

namespace SecretSanta.Communications
{
    public interface ICommunicationService
    {
        bool CanBeUsed { get; }
        void Send<T>(string languageCode, KeyValuePair<T, T> matches)
            where T : Participant;
    }
}