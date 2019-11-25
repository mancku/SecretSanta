using SecretSanta.BindingModels;

namespace SecretSanta
{
    public interface ISecretSantaService
    {
        void ExecuteSecretSanta(SecretSantaEvent secretSantaEvent);
    }
}