using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAppsProject.Speech_Class
{
    internal class AppCommand
    {
        public string Name { get; set; }
        public string ExecutablePath { get; set; }
        public HashSet<string> Actions { get; set; }

        public AppCommand(string name, string executablePath, HashSet<string> actions)
        {
            Name = name;
            ExecutablePath = executablePath;
            Actions = actions;
        }
    }
}
