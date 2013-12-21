using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace Sample2
{
    class Program
    {
        /// <summary>
        /// Get-Process chrome | Sort-Object -Descending CPU
        /// </summary>
        /// <param name="args"></param>
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
                    ps.AddCommand("Get-Process");
                    ps.AddArgument("chrome");
                    ps.AddCommand("Sort-Object");
                    ps.AddParameter("Descending");
                    ps.AddArgument("CPU");
                    var results = ps.Invoke();
                   // Console.WriteLine(string.Join("\r\n", results.First().Properties.Select(p => p.Name)));
                    results.Select(result => string.Format("{0} {1} {2}",
                            result.Properties["Id"].Value,
                            result.Properties["ProcessName"].Value,
                            // この行で必要※1
                            result.Members["CPU"].Value))
                        .ToList()
                        .ForEach(Console.WriteLine);

                    //これでもプロパティ取得できます
                    results.Select(result => result.BaseObject)
                        .Cast<Process>()
                        .Select(result => string.Format("{0} {1} {2}", 
                            result.Id, 
                            result.ProcessName, 
                            result.TotalProcessorTime.TotalSeconds))
                        .ToList()
                        .ForEach(Console.WriteLine);
                }
            }
            Console.ReadKey();
        }

    }
}
