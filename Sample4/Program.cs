using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Sample3;

namespace Sample4
{
    class Program
    {
        public bool ShouldExit { get; set; }
        public int ExitCode { get; set; }

        static void Main(string[] args)
        {
            var me = new Program();
            var myHost = new MyHost(me);

            using (var rs = RunspaceFactory.CreateRunspace(myHost))
            {
                // Open the runspace.
                rs.Open();

                // Create a PowerShell object to run the script.
                using (var ps = PowerShell.Create())
                {
                    ps.Runspace = rs;

                    var script = "exit (2+2)";
                    ps.AddScript(script);
                    ps.Invoke(script);
                }

                // Check the flags and see if they were set propertly.
                Console.WriteLine(
                    "ShouldExit={0} (should be True); ExitCode={1} (should be 4)",
                    me.ShouldExit,
                    me.ExitCode);

                // close the runspace to free resources.
                rs.Close();
            }

            Console.ReadKey();
        }
    }
}
