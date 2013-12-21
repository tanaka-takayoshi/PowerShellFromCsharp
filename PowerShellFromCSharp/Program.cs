using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace PowerShellFromCSharp
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
                        .Select(result => (dynamic)result.BaseObject)
                        .Select(result => string.Format("{0} {1}", result.Status, result.ServiceName))
                        .ToList()
                        .ForEach(Console.WriteLine);
                }
            }
        }
    }
}
