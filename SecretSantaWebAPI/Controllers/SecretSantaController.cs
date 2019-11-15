using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretSanta;
using SecretSantaBindingModels;
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
        [Route("ExecuteSecretSanta")]
        public IEnumerable<string> ExecuteSecretSanta(IEnumerable<Participant> participants)
        {
            return this.SecretSantaGenerator.Generate(participants, true)
                .Select(x => $"{x.Key.FullName} --> {x.Value.FullName}")
                .OrderBy(x => x);
        }
    }
}
