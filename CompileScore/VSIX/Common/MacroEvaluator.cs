﻿using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CompileScore
{
    public class MacroEvaluator
    { 
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public void Clear()
        {
            dict.Clear();
        }

        public string Evaluate(string input)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return Regex.Replace(input, @"(\$\([a-zA-Z0-9_]+\))", delegate (Match m)
            {
                if (dict.ContainsKey(m.Value))
                {
                    return dict[m.Value];
                }

                string macroValue = ComputeMacro(m.Value);

                if (macroValue != null)
                {
                    dict[m.Value] = macroValue;
                    return macroValue;
                }

                return m.Value;
            });
        }

        private string ComputeMacro(string macroStr)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (macroStr == @"$(SolutionDir)")
            {
                return EditorUtils.GetSolutionPath();
            }
            else if (macroStr == @"$(SolutionName)")
            {
                //TODO~  ramonv ~ to be implemented
                return null;
            }
            else if (macroStr == @"$(Configuration)")
            {
                return SolutionEventsListener.Instance.ConfigurationName;
            }
            else if (macroStr == "$(Platform)")
            {
                return SolutionEventsListener.Instance.PlatformName;
            }
            
            //TODO ~ ramonv ~ add detail level 

            return null;
        }
    }
}