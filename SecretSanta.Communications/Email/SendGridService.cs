using SendGrid;
using SendGrid.Helpers.Mail;
using System;

namespace SecretSanta.Communications.Email
{
    public class SendGridService : SenderService
    {
        private SendGridClient SendGrid { get; }
        private EmailAddress SenderEmailAddress { get; }

        public SendGridService()
            : base("Email", "SendGridSettings",
                "ApiKey",
                "SenderEmailAddress")
        {
            if (this.CanBeUsed)
            {
                this.SendGrid = new SendGridClient(this.Configuration["ApiKey"]);
                this.SenderEmailAddress = new EmailAddress(this.Configuration["SenderEmailAddress"]);
            }
        }

        protected override void SendToParticipant<T>(string languageCode, T sender, T receiver, string customMessage)
        {
            var translations = this.GetTranslationConfiguration<SendGridTranslation>(languageCode);
            this.SenderEmailAddress.Name = translations.SenderName;

            var msg = new SendGridMessage
            {
                TemplateId = translations.TemplateId,
                From = this.SenderEmailAddress,

                // Not needed if using templates, as the template forces us to set a subject
                //Subject = "Some subject"
            };

            msg.AddTo(new EmailAddress(sender.Email, sender.Name));

            msg.SetTemplateData(new
            {
                receiverName = receiver.Name,
                senderName = sender.Name,
                customMessage = customMessage ?? string.Empty
            });
            msg.AddCustomArg("InternalId", Guid.NewGuid().ToString());

            var response = this.SendGrid.SendEmailAsync(msg).Result;
        }
    }
}
