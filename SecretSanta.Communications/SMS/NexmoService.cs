using SecretSanta.BindingModels;
using System.Collections.Generic;
using Nexmo.Api;
using Nexmo.Api.Request;

namespace SecretSanta.Communications.SMS
{
    public class NexmoService : ICommunicationService
    {
        private Client NexmoClient { get; }
        public NexmoService(string apiKey, string apiSecret)
        {
            this.CanBeUsed = !string.IsNullOrWhiteSpace(apiKey) && !string.IsNullOrWhiteSpace(apiSecret);

            this.NexmoClient = new Client(new Credentials
            {
                ApiKey = apiKey,
                ApiSecret = apiSecret,
            });
        }

        public bool CanBeUsed { get; }

        public void Send<T>(string languageCode, KeyValuePair<T, T> match)
            where T : Participant
        {
            var message = this.GetTextMessageText<T>(languageCode, match.Value.Name);
            var results = this.NexmoClient.SMS.Send(new global::Nexmo.Api.SMS.SMSRequest
            {
                from = languageCode == "es" ? "INVISIBLE" : "SECRETSANTA",
                to = match.Key.PhoneNumber,
                text = message
            });
        }

        private string GetTextMessageText<T>(string languageCode, string presentReceiverName)
            where T : Participant
        {
            var message = languageCode == "es"
                ? "Tu regalo será para:"
                : "Your present will be for:";

            return $"{message} {presentReceiverName}";
        }
    }
}