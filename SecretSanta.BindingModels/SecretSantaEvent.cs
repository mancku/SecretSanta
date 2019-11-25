using System.ComponentModel.DataAnnotations;

namespace SecretSanta.BindingModels
{
    public class SecretSantaEvent
    {
        [Required]
        public string LanguageCode { get; set; }

        [Required]
        public SecretSantaParticipants GenerationInfo { get; set; }
    }
}