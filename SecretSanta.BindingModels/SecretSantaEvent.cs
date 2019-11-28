using System.ComponentModel.DataAnnotations;

namespace SecretSanta.BindingModels
{
    public class SecretSantaEvent
    {
        [Required]
        public string LanguageCode { get; set; }

        [Required]
        public SecretSantaParticipants ParticipantsInfo { get; set; }

        [MinLength(10)]
        public string CustomMessage { get; set; }
    }
}