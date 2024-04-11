using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAppsProject.Speech_Class
{
    internal class AppCommand
    {
        public string ExecutablePath { get; set; }
        public List<string> SupportedActions { get; set; }

        public AppCommand(string executablePath, List<string> supportedActions)
        {
            ExecutablePath = executablePath;
            SupportedActions = supportedActions;
        }
    }
}
