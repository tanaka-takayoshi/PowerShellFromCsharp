using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace Sample3
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var rs = RunspaceFactory.CreateRunspace())
            {
                // これが必要※1
                Runspace.DefaultRunspace = rs;
                rs.Open();
                using (var ps = PowerShell.Create())
                {
                    ps.Runspace = rs;
                   // ps.AddScript(File);

                    var results = ps.Invoke();
                    //これでもプロパティ取得できます
                    results.Select(result => result.BaseObject)
                        .Cast<Process>()
                        .Select(result => string.Format("{0} {1} {2}", result.Id, result.ProcessName, result.TotalProcessorTime.TotalMilliseconds / 1000))
                        .ToList()
                        .ForEach(Console.WriteLine);
                }
            }
            Console.ReadKey();
        }
    }
}
