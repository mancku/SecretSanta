using SecretSanta.BindingModels;
using SecretSanta.Communications;
using SecretSanta.Communications.SMS;

namespace SecretSanta
{
    public class SecretSantaService : ISecretSantaService
    {
        private ISecretSantaGenerator SecretSantaGenerator { get; }
        private ICommunicationsService CommunicationsService { get; }

        public SecretSantaService(ISecretSantaGenerator secretSantaGenerator, ICommunicationsService communicationsService)
        {
            this.SecretSantaGenerator = secretSantaGenerator;
            this.CommunicationsService = communicationsService;
        }
        public void ExecuteSecretSanta(SecretSantaEvent secretSantaEvent)
        {
            var result = this.SecretSantaGenerator.Generate(secretSantaEvent.GenerationInfo.Participants,
                secretSantaEvent.GenerationInfo.ExcludeMutualPairings);

            this.CommunicationsService.SendSecretSantas(secretSantaEvent.LanguageCode, result);
        }
    }
}