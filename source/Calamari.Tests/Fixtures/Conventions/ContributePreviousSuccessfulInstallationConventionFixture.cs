﻿using System;
<<<<<<< HEAD
using Calamari.Commands;
=======
using System.Collections.Generic;
>>>>>>> vNext
using Calamari.Deployment;
using Calamari.Deployment.Conventions;
using Calamari.Deployment.Journal;
using Calamari.Integration.Processes;
using Calamari.Shared;
using NSubstitute;
using NUnit.Framework;

namespace Calamari.Tests.Fixtures.Conventions
{
    [TestFixture]
    public class ContributePreviousSuccessfulInstallationConventionFixture
    {
        IDeploymentJournal journal;
        JournalEntry previous;
        CalamariVariableDictionary variables;

        [SetUp]
        public void SetUp()
        {
            journal = Substitute.For<IDeploymentJournal>();
            journal.GetLatestSuccessfulInstallation(Arg.Any<string>()).Returns(_ => previous);
            variables = new CalamariVariableDictionary();
        }

        [Test]
        public void ShouldAddVariablesIfPreviousInstallation()
        {
            previous = new JournalEntry("123", "tenant", "env", "proj", "rp01", DateTime.Now, "C:\\App", "C:\\MyApp", true, 
                new List<DeployedPackage>{new DeployedPackage("pkg", "0.0.9", "C:\\PackageOld.nupkg")});
            RunConvention();
            Assert.That(variables.Get(SpecialVariables.Tentacle.PreviousSuccessfulInstallation.OriginalInstalledPath), Is.EqualTo("C:\\App"));
        }

        [Test]
        public void ShouldAddEmptyVariablesIfNoPreviousInstallation()
        {
            previous = null;
            RunConvention();
            Assert.That(variables.Get(SpecialVariables.Tentacle.PreviousSuccessfulInstallation.OriginalInstalledPath), Is.EqualTo(""));
        }

        void RunConvention()
        {
            var convention = new ContributePreviousSuccessfulInstallationConvention(journal);
            convention.Run(new CalamariExecutionContext("C:\\Package.nupkg", variables));
        }
    }
}
