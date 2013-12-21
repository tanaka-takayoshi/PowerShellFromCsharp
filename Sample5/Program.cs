using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Sample5
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionInfo = new WSManConnectionInfo()
            {
                Scheme = "https",
                ComputerName = ConfigurationManager.AppSettings["ComputerName"],
                Port = int.Parse(ConfigurationManager.AppSettings["Port"]),
                Credential = new PSCredential(ConfigurationManager.AppSettings["UserName"],
                                                ConfigurationManager.AppSettings["Password"]
                                                .Aggregate(new SecureString(), (s, c) =>
                                                {
                                                    s.AppendChar(c);
                                                    return s;
                                                })),
                SkipCACheck = true
            };
            using (var rs = RunspaceFactory.CreateRunspace(connectionInfo))
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
                }
            }
            Console.ReadKey();
        }
    }
}
