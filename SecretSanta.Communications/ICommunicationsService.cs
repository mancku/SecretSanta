using SecretSanta.BindingModels;
using System.Collections.Generic;

namespace SecretSanta.Communications
{
    public interface ICommunicationsService
    {
        void SendSecretSantas<T>(string languageCode, IDictionary<T, T> matches)
            where T : Participant;
    }
}