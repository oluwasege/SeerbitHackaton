using log4net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Xml;

namespace SeerbitHackaton.Core.Utils
{
    public static class Log4NetHelper
    {
        public static IHostBuilder ConfigureLog4net(this IHostBuilder webHost)
        {
            var log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));

            var loggerRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(loggerRepository, log4netConfig["log4net"]);

            webHost.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddLog4Net();
            });

            return webHost;
        }
    }
}
