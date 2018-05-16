﻿#if KUBERNETES
using System;
using System.Collections.Specialized;
using System.IO;
using Alphaleonis.Win32.Filesystem;
using Calamari.Hooks;
using Calamari.Integration.FileSystem;
using Calamari.Integration.Processes;
using Calamari.Integration.Scripting;
using Calamari.Integration.Scripting.WindowsPowerShell;
using Calamari.Kubernetes;
using Calamari.Tests.Helpers;
using NUnit.Framework;
using File = System.IO.File;
using Path = System.IO.Path;

namespace Calamari.Tests.KubernetesFixtures
{
    [TestFixture]
    [Ignore("NotYet")]
    public class KubernetesContextScriptWrapperFixture
    {
        const string ClusterTokenEnvironmentVariable = "OCTOPUS_K8S_TOKEN";
        const string CluserServerEnvironmentVariable = "OCTOPUS_K8S_SERVER";

        //See "GitHub Test Account"
        private static readonly string ClusterUri = Environment.GetEnvironmentVariable(CluserServerEnvironmentVariable);
        static readonly string ClusterToken = Environment.GetEnvironmentVariable(ClusterTokenEnvironmentVariable);
        
        [Test]
        [Category(TestEnvironment.CompatibleOS.Windows)]
        public void PowershellKubeCtlScripts()
        {
            var wrapper = new KubernetesContextScriptWrapper(new CalamariVariableDictionary());
            TestScript(wrapper, "Test-Script.ps1");
            //TestScript(new KubernetesPowershellScriptEngine(), "Test-Script.ps1");
        }
        
        [Test]

        [Category(TestEnvironment.CompatibleOS.Nix)]
        public void BashKubeCtlScripts()
        {
            //TestScript(new KubernetesBashScriptEngine(), "Test-Script.sh");
        }

        private void TestScript(IScriptWrapper wrapper, string scriptName)
        {
            using (var dir = TemporaryDirectory.Create())
            using (var temp = new TemporaryFile(Path.Combine(dir.DirectoryPath, scriptName)))
            {
                File.WriteAllText(temp.FilePath, "kubectl get nodes");
                var output = ExecuteScript(wrapper, temp.FilePath, new CalamariVariableDictionary()
                {
                    ["OctopusKubernetesServer"] = "https://ec2-18-232-75-57.compute-1.amazonaws.com",
                    ["OctopusKubernetesToken"] = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJrdWJlcm5ldGVzL3NlcnZpY2VhY2NvdW50Iiwia3ViZXJuZXRlcy5pby9zZXJ2aWNlYWNjb3VudC9uYW1lc3BhY2UiOiJrdWJlLXN5c3RlbSIsImt1YmVybmV0ZXMuaW8vc2VydmljZWFjY291bnQvc2VjcmV0Lm5hbWUiOiJrdWJlcm5ldGVzLWRhc2hib2FyZC10b2tlbi00d3FtbSIsImt1YmVybmV0ZXMuaW8vc2VydmljZWFjY291bnQvc2VydmljZS1hY2NvdW50Lm5hbWUiOiJrdWJlcm5ldGVzLWRhc2hib2FyZCIsImt1YmVybmV0ZXMuaW8vc2VydmljZWFjY291bnQvc2VydmljZS1hY2NvdW50LnVpZCI6ImFhZGU2NTk1LTUzNDktMTFlOC1hZDhkLTBlZTJhNzg3ZGFjZSIsInN1YiI6InN5c3RlbTpzZXJ2aWNlYWNjb3VudDprdWJlLXN5c3RlbTprdWJlcm5ldGVzLWRhc2hib2FyZCJ9.CKeCGqg0sLsZyi4YaH7r02guclddKbDnjvuwzt_2rQQ3aZ_pQWrpjIjo83ebx1UsS6vt5Jq81W7Sxyrh81asq6xA74JU1UTMMcx2WIn3-dfaG9f3pBwGEi_mE7CkyUMMrZpC4R-_VU-YfAhfkvb68-koGrM4V0nFRGphEcbfwXDBcpE9qiiYr4dTdVdoOK2LnAMkyEqsGE9lqNBTq9HBakHMKT8nOWg4yzO76Kcvlqv_JzJCfxDnRv7AAteanq5Y2eYvlag5l0wECpi7cme9Tb4oykmt5I974SxGcu3AwxwagErSGHAOzTlJoOiJh07WFCXDerzga4Vmg9ElKlLX1A",
                    ["OctopusKubernetesInsecure"] = "true"

                });
                output.AssertSuccess();
                output.AssertOutput("ASKROB");
            }
        }

        private CalamariResult ExecuteScript(IScriptWrapper wrapper, string scriptName, CalamariVariableDictionary variables)
        {
            var capture = new CaptureCommandOutput();
            var runner = new CommandLineRunner(capture);
            wrapper.NextWrapper = new TerminalScriptWrapper(new PowerShellScriptEngine());
            var result = wrapper.ExecuteScript(new Script(scriptName), variables, runner, new StringDictionary());
            //var result = psse.Execute(new Script(scriptName), variables, runner);
            return new CalamariResult(result.ExitCode, capture);
        }
    }
}
#endif