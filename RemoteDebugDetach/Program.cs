using EnvDTE;
using EnvDTE80;
using Shared;
using System;
using System.Runtime.InteropServices;

namespace RemoteDebugDetach
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string solutionName = Ask(args, 0, "Solution name");
            string processName = Ask(args, 1, "Process Name?");

            var instances = Msdev.GetIDEInstances(true);

            var dte = (DTE2)instances.Find(d => d.Solution.FullName.EndsWith(solutionName, StringComparison.InvariantCultureIgnoreCase));
            var debugger = dte.Debugger as Debugger2;

            Processes processes = debugger.DebuggedProcesses;
            foreach (Process2 process in processes)
            {
                if (process.Name.EndsWith(processName, StringComparison.InvariantCultureIgnoreCase))
                {
                    process.Detach(false);
                }
            }
        }

        static string Ask(string[] args, int index, string question)
        {
            if (args.Length <= index)
            {
                Console.WriteLine(question);
                return Console.ReadLine();
            }

            return args[index];
        }
    }
}
