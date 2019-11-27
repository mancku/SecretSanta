using SecretSanta.BindingModels;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Nexmo.Api;
using Nexmo.Api.Request;

namespace SecretSanta.Communications.SMS
{
    public class NexmoService : SenderService
    {
        private Client NexmoClient { get; }

        public NexmoService(IConfiguration configuration)
            : base(configuration,
                "Communications:Nexmo:ApiKey",
                "Communications:Nexmo:ApiSecret")
        {
            this.NexmoClient = new Client(new Credentials
            {
                ApiKey = this.Configuration["Communications:Nexmo:ApiKey"],
                ApiSecret = this.Configuration["Communications:Nexmo:ApiSecret"],
            });
        }

        public override void Send<T>(string languageCode, T sender, T receiver)
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