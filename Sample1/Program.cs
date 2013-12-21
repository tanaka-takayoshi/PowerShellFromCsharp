using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace Sample1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var rs = RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (var ps = PowerShell.Create())
                {
                    ps.Runspace = rs;
                    ps.AddCommand("Get-Service");
                    ps.Invoke()
                        .Select(result => string.Format("{0} {1}", result.Properties["Status"].Value, result.Properties["Name"].Value))
                        .ToList()
                        .ForEach(Console.WriteLine);

                    //これでもプロパティ取得できます
                    ps.Invoke()
                        .Select(result => result.BaseObject)
                        .Cast<dynamic>()
                        .Select(result => string.Format("{0} {1}", result.Status, result.ServiceName))
                        .ToList()
                        .ForEach(Console.WriteLine);
                }
            }
            Console.ReadKey();
        }


        static void Main2(string[] args)
        {
            var psInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = "Get-ChildItem",
                RedirectStandardOutput = true
            };
            var p = Process.Start(psInfo);
            Console.WriteLine(p.StandardOutput.ReadToEndAsync().Result);
            Console.ReadKey();
        }
    }
}
