﻿using System.Collections.Specialized;
using Calamari.Integration.Processes;

namespace Calamari.Integration.Scripting
{
    public interface IScriptEngine  
    {
        ScriptSyntax[] GetSupportedTypes();
        CommandResult Execute(
            Script script, 
            CalamariVariableDictionary variables, 
            ICommandLineRunner commandLineRunner, 
            StringDictionary environmentVars = null);
    }
}