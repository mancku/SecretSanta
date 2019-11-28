using Microsoft.Extensions.Configuration;
using Nexmo.Api;
using Nexmo.Api.Request;
using SecretSanta.BindingModels;

namespace SecretSanta.Communications.SMS
{
    public class NexmoService : SenderService
    {
        private Client NexmoClient { get; }

        public NexmoService()
            : base("SMS", "NexmoSettings",
                "ApiKey",
                "ApiSecret")
        {

            if (this.CanBeUsed)
            {
                this.NexmoClient = new Client(new Credentials
                {
                    ApiKey = this.Configuration["ApiKey"],
                    ApiSecret = this.Configuration["ApiSecret"],
                });
            }
        }
        protected override void SendToParticipant<T>(string languageCode, T sender, T receiver, string customMessage)
        {
            var translation = this.GetTranslationConfiguration<NexmoTranslation>(languageCode);
            var message = string.Format(translation.Message, receiver.Name);
            var results = this.NexmoClient.SMS.Send(new Nexmo.Api.SMS.SMSRequest
            {
                from = translation.SenderName,
                to = sender.PhoneNumber,
                text = message,
            });
        }
    }
}