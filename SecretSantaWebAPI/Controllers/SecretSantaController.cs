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
        private readonly ISecretSantaService SecretSantaService;

        public SecretSantaController(ILogger<SecretSantaController> logger, ISecretSantaGenerator secretSantaGenerator, ISecretSantaService secretSantaService)
        {
            this._logger = logger;
            this.SecretSantaGenerator = secretSantaGenerator;
            this.SecretSantaService = secretSantaService;
        }

        [HttpPost]
        [Route("GenerateSecretSanta")]
        public IEnumerable<string> GenerateSecretSanta(SecretSantaParticipants secretSantaGeneration)
        {
            return this.SecretSantaGenerator.Generate(secretSantaGeneration.Participants,
                    secretSantaGeneration.ExcludeMutualPairings)
                .Select(x => $"{x.Key.Name} --> {x.Value.Name}")
                .OrderBy(x => x);
        }

        [HttpPost]
        [Route("ExecuteSecretSanta")]
        public void ExecuteSecretSanta(SecretSantaEvent secretSantaEvent)
        {
            this.SecretSantaService.ExecuteSecretSanta(secretSantaEvent);
        }
    }
}