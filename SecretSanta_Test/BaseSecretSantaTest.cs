﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta;
using SecretSanta.BindingModels;
using System;
using System.Collections.Generic;

namespace SecretSanta_Test
{
    [TestClass]
    public abstract class BaseSecretSantaTest
    {
        protected IList<Participant> participants;
        protected SecretSantaGenerator secretSantaGenerator;
        protected readonly Random Randomizer = new Random(DateTime.Now.Millisecond);

        [TestInitialize]
        public void SetUp()
        {
            this.participants = new List<Participant>();
            for (var i = 0; i < this.Randomizer.Next(5, 9); i++)
            {
                this.participants.Add(new Participant
                {
                    Name = $"{Faker.Name.First()} {i}",
                    Email = Faker.Internet.FreeEmail(),
                    PhoneNumber = Faker.Phone.Number()
                });
            }

            this.secretSantaGenerator = new SecretSantaGenerator();

            this.SpecificTestSetup();
        }

        protected abstract void SpecificTestSetup();
    }
}
