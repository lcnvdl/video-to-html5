using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTH.Logic
{
    public abstract class ConsoleProgram
    {
        protected abstract string ProgramX86 { get; }
        protected abstract string ProgramX64 { get; }

        public ConsoleProgram()
        {

        }

        protected void Execute(string args, string workingDir = "")
        {
            string app;

            if(Environment.Is64BitOperatingSystem)
            {
                app = ProgramX64;
            }
            else
            {
                app = ProgramX86;
            }

            var process = new Process();
            process.StartInfo = new ProcessStartInfo(app, args);
            if(!string.IsNullOrEmpty(workingDir))
                process.StartInfo.WorkingDirectory = workingDir;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }
    }
}
