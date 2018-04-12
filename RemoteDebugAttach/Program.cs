using EnvDTE;
using EnvDTE80;
using Shared;
using System;

namespace RemoteDebugAttach
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string solutionName = Ask(args, 0, "Solution name?");
            string transportName = Ask(args, 1, "Transport name?");
            string target = Ask(args, 2, "Target machine?");
            string processName = Ask(args, 3, "Process Name?");


            var instances = Msdev.GetIDEInstances(true);
            var dte = (DTE2)instances.Find(d => d.Solution.FullName.EndsWith(solutionName, StringComparison.InvariantCultureIgnoreCase));
            var debugger = dte.Debugger as Debugger2;
            var transports = debugger.Transports;
            Transport transport = null;
            foreach(Transport loopTransport in transports)
            {
                if(loopTransport.Name.Equals(transportName, StringComparison.InvariantCultureIgnoreCase)) // "Remote (no authentication)")
                {
                    transport = loopTransport;
                    break;
                }
            }

            Processes processes = debugger.GetProcesses(transport, target); // "172.24.50.15:4022");
            foreach(Process process in processes)
            {
                if(process.Name.EndsWith(processName, StringComparison.InvariantCultureIgnoreCase))
                {
                    process.Attach();
                }
            }
        }

        static string Ask(string[] args, int index, string question)
        {
            if(args.Length <= index)
            {
                Console.WriteLine(question);
                return Console.ReadLine();
            }

            return args[index];
        }
    }
}
