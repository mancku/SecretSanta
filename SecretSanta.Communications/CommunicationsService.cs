using SecretSanta.BindingModels;
using SecretSanta.Communications.SMS;
using System.Collections.Generic;
using SecretSanta.Communications.Email;

namespace SecretSanta.Communications
{
    public class CommunicationsService : ICommunicationsService
    {
        private NexmoService SmsCommunicationService { get; }
        private SendGridService EmailCommunicationService { get; }
        public CommunicationsService(SenderServiceResolver senderServiceResolver)
        {
            this.SmsCommunicationService = (NexmoService)senderServiceResolver(SenderType.SMS);
            this.EmailCommunicationService = (SendGridService)senderServiceResolver(SenderType.Email);
        }

        public void SendSecretSantas<T>(string languageCode, IDictionary<T, T> matches)
            where T : Participant
        {
            foreach (var match in matches)
            {
                this.SendWithSendGrid(languageCode, match);
                this.SendWithNexmo(languageCode, match);

                System.Threading.Thread.Sleep(1100);
            }
        }

        private void SendWithSendGrid<T>(string languageCode, KeyValuePair<T, T> match)
            where T : Participant
        {
            if (this.EmailCommunicationService != null && this.EmailCommunicationService.CanBeUsed && !string.IsNullOrWhiteSpace(match.Key.Email))
            {
                this.EmailCommunicationService.Send(languageCode, match.Key, match.Value);
            }
        }

        private void SendWithNexmo<T>(string languageCode, KeyValuePair<T, T> match)
            where T : Participant
        {
            if (this.SmsCommunicationService != null && this.SmsCommunicationService.CanBeUsed && !string.IsNullOrWhiteSpace(match.Key.PhoneNumber))
            {
                this.SmsCommunicationService.Send(languageCode, match.Key, match.Value);
            }
        }
    }
}