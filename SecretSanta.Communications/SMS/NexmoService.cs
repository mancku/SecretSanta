using SecretSanta.BindingModels;
using System.Collections.Generic;
using Nexmo.Api;
using Nexmo.Api.Request;

namespace SecretSanta.Communications.SMS
{
    public class NexmoService : ISenderService
    {
        private Client NexmoClient { get; }
        public bool CanBeUsed { get; }

        public NexmoService(string apiKey, string apiSecret)
        {
            this.CanBeUsed = !string.IsNullOrWhiteSpace(apiKey) && !string.IsNullOrWhiteSpace(apiSecret);

            this.NexmoClient = new Client(new Credentials
            {
                ApiKey = apiKey,
                ApiSecret = apiSecret,
            });
        }

        public void Send<T>(string languageCode, T sender, T receiver)
            where T : Participant
        {
            var message = this.GetTextMessageText<T>(languageCode, receiver.Name);
            var results = this.NexmoClient.SMS.Send(new global::Nexmo.Api.SMS.SMSRequest
            {
                from = languageCode == "es" ? "INVISIBLE" : "SECRETSANTA",
                to = sender.PhoneNumber,
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