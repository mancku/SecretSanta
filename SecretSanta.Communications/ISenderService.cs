using System.Collections.Generic;
using SecretSanta.BindingModels;

namespace SecretSanta.Communications
{
    public enum SenderType
    {
        Email = 1,
        SMS = 2,
    }

    public delegate ISenderService SenderServiceResolver(SenderType senderType);
    public interface ISenderService
    {
        bool CanBeUsed { get; }
        void Send<T>(string languageCode, T sender, T receiver)
            where T : Participant;
    }
}