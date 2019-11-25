using SecretSanta.BindingModels;
using SecretSanta.Communications.SMS;
using System.Collections.Generic;

namespace SecretSanta.Communications
{
    public class CommunicationsService : ICommunicationsService
    {
        private ICommunicationService CommunicationClient { get; }
        public CommunicationsService(ICommunicationService communicationClient)
        {
            this.CommunicationClient = communicationClient;
        }
        public void SendSecretSantas<T>(string languageCode, IDictionary<T, T> matches)
            where T : Participant
        {
            foreach (var match in matches)
            {
                this.SendWithNexmo(languageCode, match);

                System.Threading.Thread.Sleep(1100);
            }
        }

        private void SendWithNexmo<T>(string languageCode, KeyValuePair<T, T> match) where T : Participant
        {
            if (this.CommunicationClient.CanBeUsed && !string.IsNullOrWhiteSpace(match.Key.PhoneNumber))
            {
                this.CommunicationClient.Send(languageCode, match);
            }
        }
    }
}