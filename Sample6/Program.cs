using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Sample6
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
            using (var rsPool = RunspaceFactory.CreateRunspacePool(1, 2, connectionInfo))
            {
                rsPool.Open();

                var gpsCommand = PowerShell.Create().AddCommand("Get-Process");
                gpsCommand.RunspacePool = rsPool;
                var gpsCommandAsyncResult = gpsCommand.BeginInvoke();

                var getServiceCommand = PowerShell.Create().AddCommand("Get-Service");
                getServiceCommand.RunspacePool = rsPool;
                var getServiceCommandAsyncResult = getServiceCommand.BeginInvoke();

                var gpsCommandOutput = gpsCommand.EndInvoke(gpsCommandAsyncResult);
                gpsCommandOutput.Select(result => string.Format("{0} {1}", result.Properties["ID"].Value, result.Properties["Name"].Value))
                        .ToList()
                        .ForEach(Console.WriteLine);

                var getServiceCommandOutput = getServiceCommand.EndInvoke(getServiceCommandAsyncResult);
                getServiceCommandOutput.Select(result => string.Format("{0} {1}", result.Properties["Status"].Value, result.Properties["Name"].Value))
                        .ToList()
                        .ForEach(Console.WriteLine);

                rsPool.Close();
            }
            Console.ReadKey();
        }
    }
}
