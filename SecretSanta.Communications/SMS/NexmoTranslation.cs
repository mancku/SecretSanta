namespace SecretSanta.Communications.SMS
{
    public class NexmoTranslation : ICommunicationTranslation
    {
        public string LanguageCode { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
    }
}