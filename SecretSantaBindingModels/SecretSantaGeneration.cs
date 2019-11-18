using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSantaBindingModels
{
    public class SecretSantaGeneration
    {
        [MinLength(3, ErrorMessage = "There should be at least 3 participants")]
        public IEnumerable<Participant> Participants { get; set; }
        public bool ExcludeMutualPairings { get; set; }
    }
}
