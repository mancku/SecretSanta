using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;

namespace SecretSanta.Communications.Email
{
    public class SendGridService : SenderService
    {
        private SendGridClient SendGrid { get; }
        private EmailAddress SenderEmailAddress { get; }

        public SendGridService(IConfiguration configuration)
            : base(configuration,
                "Communications:SendGrid:ApiKey",
                "Communications:SendGrid:SenderEmailAddress")
        {
            this.SendGrid = new SendGridClient(this.Configuration["Communications:SendGrid:ApiKey"]);
            this.SenderEmailAddress = new EmailAddress(this.Configuration["Communications:SendGrid:SenderEmailAddress"]);
        }

        public override void Send<T>(string languageCode, T sender, T receiver)
        {
            this.SenderEmailAddress.Name = "SECRET SANTA";

            var msg = new SendGridMessage
            {
                TemplateId = this.Configuration["Communications:SendGrid:TemplateId"],
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
