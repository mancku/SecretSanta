using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SecretSanta.BindingModels;

namespace SecretSanta.Communications
{
    public enum SenderType
    {
        Email = 1,
        SMS = 2,
    }

    public delegate ISenderService SenderServiceResolver(SenderType senderType);
    public interface ISenderService
    {
        bool CanBeUsed { get; }
        void Send<T>(string languageCode, T sender, T receiver)
            where T : Participant;
    }

    public abstract class SenderService : ISenderService
    {
        protected IConfiguration Configuration;
        public bool CanBeUsed { get; }


        protected SenderService(IConfiguration configuration, params string[] keysToDetermineIfCanBeUsed)
        {
            this.Configuration = configuration;
            this.CanBeUsed = this.CheckCanBeUsed(keysToDetermineIfCanBeUsed);
        }

        public abstract void Send<T>(string languageCode, T sender, T receiver)
            where T : Participant;

        private bool CheckCanBeUsed(params string[] configs)
        {
            return !configs.Any(x => string.IsNullOrWhiteSpace(this.Configuration[x]));
        }
    }
}