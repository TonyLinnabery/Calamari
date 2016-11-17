﻿using System;
using System.IO;
using System.Linq;
using Calamari.Deployment;
using Calamari.Deployment.Conventions;
using Calamari.Integration.FileSystem;

namespace Calamari.Integration.Vhd
{
    public class MountVhdConvention : IInstallConvention
    {
        private readonly ICalamariFileSystem fileSystem;

        public MountVhdConvention(ICalamariFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void Install(RunningDeployment deployment)
        {
            if (!Vhd.FeatureIsOn(deployment))
                return;

            var vhdPath = Vhd.FindSingleVhdInFolder(fileSystem, deployment.CurrentDirectory);

            Vhd.Mount(vhdPath, Vhd.GetMountPoint(deployment));
            deployment.Variables.Set(SpecialVariables.Vhd.MountPoint, vhdPath);
        }
    }
}