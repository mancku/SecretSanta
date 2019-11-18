using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretSanta;
using SecretSanta.BindingModels;
using System.Collections.Generic;
using System.Linq;

namespace SecretSantaWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecretSantaController : ControllerBase
    {

        private readonly ILogger<SecretSantaController> _logger;
        private readonly ISecretSantaGenerator SecretSantaGenerator;

        public SecretSantaController(ILogger<SecretSantaController> logger, ISecretSantaGenerator secretSantaGenerator)
        {
            this._logger = logger;
            this.SecretSantaGenerator = secretSantaGenerator;
        }

        [HttpPost]
        [Route("GenerateSecretSanta")]
        public IEnumerable<string> GenerateSecretSanta(SecretSantaGeneration secretSantaGeneration)
        {
            return this.SecretSantaGenerator.Generate(secretSantaGeneration.Participants,
                    secretSantaGeneration.ExcludeMutualPairings)
                .Select(x => $"{x.Key.FullName} --> {x.Value.FullName}")
                .OrderBy(x => x);
        }

        [HttpPost]
        [Route("ExecuteSecretSanta")]
        public void ExecuteSecretSanta(SecretSantaGeneration secretSantaGeneration)
        {
            this.SecretSantaGenerator.Generate(secretSantaGeneration.Participants,
                    secretSantaGeneration.ExcludeMutualPairings);
        }
    }
}
