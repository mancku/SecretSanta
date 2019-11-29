using SecretSanta.BindingModels;
using SecretSanta.Communications;
using System.Collections.Generic;
using System.Linq;

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
            var bannedPairings =
                (secretSantaEvent.ParticipantsInfo.BannedPairings ?? new List<BannedPairing>())
                .ToDictionary(k => k.PresentGiver, v => v.PresentReceiver);

            var result = this.SecretSantaGenerator.Generate(secretSantaEvent.ParticipantsInfo.Participants,
                bannedPairings,
                secretSantaEvent.ParticipantsInfo.ExcludeMutualPairings);

            this.CommunicationsService.SendSecretSantas(secretSantaEvent.LanguageCode, result, secretSantaEvent.CustomMessage);
        }
    }
}