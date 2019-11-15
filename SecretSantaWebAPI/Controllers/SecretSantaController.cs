using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretSanta;

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
    }
}
