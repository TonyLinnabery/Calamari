﻿﻿using Calamari.Deployment.Journal;
using Calamari.Shared;
using Calamari.Shared.Commands;
﻿using System.Linq;

namespace Calamari.Deployment.Conventions
{
    public class ContributePreviousInstallationConvention : Calamari.Shared.Commands.IConvention
    {
        readonly IDeploymentJournal journal;

        public ContributePreviousInstallationConvention(IDeploymentJournal journal)
        {
            this.journal = journal;
        }

        public void Run(IExecutionContext deployment)
        {
            var policySet = deployment.Variables.Get(SpecialVariables.RetentionPolicySet);
            
            var previous = journal.GetLatestInstallation(policySet);
            
            string previousExtractedFrom;
            string previousExtractedTo;
            string previousVersion;
            string previousCustom;
            if (previous == null)
            {
                previousExtractedTo = previousExtractedFrom = previousVersion = previousCustom = "";
            }
            else
            {
                // This is assuming this convention is used only in steps with only one package
                var previousPackage = previous.Packages.FirstOrDefault();
                
                previousExtractedTo = previous.ExtractedTo;
                previousExtractedFrom = previousPackage?.DeployedFrom ?? "";
                previousVersion = previousPackage?.PackageVersion ?? "";
                previousCustom = previous.CustomInstallationDirectory;
            }

            deployment.Variables.Set(SpecialVariables.Tentacle.PreviousInstallation.OriginalInstalledPath, previousExtractedTo);
            deployment.Variables.Set(SpecialVariables.Tentacle.PreviousInstallation.CustomInstallationDirectory, previousCustom);
            deployment.Variables.Set(SpecialVariables.Tentacle.PreviousInstallation.PackageFilePath, previousExtractedFrom);
            deployment.Variables.Set(SpecialVariables.Tentacle.PreviousInstallation.PackageVersion, previousVersion);
        }

    }
}