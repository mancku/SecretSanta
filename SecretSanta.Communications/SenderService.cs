using Microsoft.Extensions.Configuration;
using SecretSanta.BindingModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SecretSanta.Communications
{
    public abstract class SenderService : ISenderService
    {
        private const string DEFAULT_LANGUAGE = "en";

        protected IConfiguration Configuration;
        public bool CanBeUsed { get; }

        protected SenderService(string configFilePath, string configFileName, params string[] configKeysToDetermineIfCanBeUsed)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(environment))
            {
                this.CanBeUsed = false;
                return;
            }

            this.CreateConfiguration(configFilePath, configFileName, environment);
            this.CanBeUsed = this.CheckCanBeUsed(configKeysToDetermineIfCanBeUsed);
        }

        private void CreateConfiguration(string configFilePath, string configFileName, string environment)
        {
            var environmentSuffix = string.IsNullOrWhiteSpace(environment)
                ? string.Empty
                : $".{environment}";
            var path = Path.Combine(configFilePath, $"{configFileName}{environmentSuffix}.json");

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(path, false, true);
            this.Configuration = configurationBuilder.Build();
        }

        public void Send<T>(string languageCode, T sender, T receiver, string customMessage)
            where T : Participant
        {
            languageCode = !string.IsNullOrWhiteSpace(languageCode)
                ? languageCode
                : DEFAULT_LANGUAGE;

            this.SendToParticipant(languageCode, sender, receiver, customMessage);
        }

        protected abstract void SendToParticipant<T>(string languageCode, T sender, T receiver, string customMessage)
            where T : Participant;

        protected T GetTranslationConfiguration<T>(string languageCode)
            where T : ICommunicationTranslation
        {
            var translationConfigurations = this.Configuration.GetSection("Translations").Get<IEnumerable<T>>().ToList();
            var result = translationConfigurations.FirstOrDefault(x => x.LanguageCode == languageCode);
            if (result == null)
            {
                result = translationConfigurations.FirstOrDefault(x => x.LanguageCode == languageCode.Substring(0, 2));
                if (result == null)
                {
                    result = translationConfigurations.FirstOrDefault(x => x.LanguageCode == DEFAULT_LANGUAGE);
                }
            }

            return result;
        }

        private bool CheckCanBeUsed(params string[] configs)
        {
            return !configs.Any(x => string.IsNullOrWhiteSpace(this.Configuration[x]));
        }
    }
}