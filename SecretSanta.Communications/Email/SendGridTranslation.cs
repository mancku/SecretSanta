namespace SecretSanta.Communications.Email
{
    public class SendGridTranslation : ICommunicationTranslation
    {
        public string LanguageCode { get; set; }
        public string SenderName { get; set; }
        public string TemplateId { get; set; }
    }
}