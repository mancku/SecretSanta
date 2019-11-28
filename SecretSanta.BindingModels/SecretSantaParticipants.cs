using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecretSanta.BindingModels
{
    public class SecretSantaParticipants
    {
        [MinLength(3, ErrorMessage = "There should be at least 3 participants")]
        public IEnumerable<Participant> Participants { get; set; }
        public IEnumerable<BannedPairing> BannedPairings { get; set; }
        public bool ExcludeMutualPairings { get; set; }
    }
}
