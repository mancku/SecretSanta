using SecretSanta.BindingModels;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;

namespace SecretSanta.Communications.Email
{
    public class SendGridService : ISenderService
    {
        private SendGridClient SendGrid { get; }
        private EmailAddress SenderEmailAddress { get; }
        private string TemplateId { get; }
        public bool CanBeUsed { get; }

        public SendGridService(string apiKey, string senderEmailAddress, string templateId)
        {
            this.CanBeUsed = !string.IsNullOrWhiteSpace(apiKey) && !string.IsNullOrWhiteSpace(senderEmailAddress);
            this.SendGrid = new SendGridClient(apiKey);
            this.SenderEmailAddress = new EmailAddress(senderEmailAddress);
            this.TemplateId = templateId;
        }

        public void Send<T>(string languageCode, T sender, T receiver)
            where T : Participant
        {
            this.SenderEmailAddress.Name = "SECRET SANTA";

            var msg = new SendGridMessage
            {
                TemplateId = this.TemplateId,
                From = this.SenderEmailAddress,

                // Not needed if using templates, as the template forces us to set a subject
                //Subject = "Some subject"
            };

            msg.AddTo(new EmailAddress(sender.Email, sender.Name));

            msg.SetTemplateData(new
            {
                receiverName = receiver.Name,
                senderName = sender.Name,
            });
            msg.AddCustomArg("InternalId", Guid.NewGuid().ToString());

            var response = this.SendGrid.SendEmailAsync(msg).Result;
        }
    }
}
