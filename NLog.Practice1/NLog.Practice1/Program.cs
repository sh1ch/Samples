using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Practice1;
using NLog.Practice1.Sample;
using System;
using System.Xml.Linq;

namespace Nlog.Practice1;

internal class Program
{
    static void Main(string[] args)
    {
        ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("sample", typeof(SampleLayoutRenderer));
        ConfigurationItemFactory.Default.Targets.RegisterDefinition("sample2", typeof(SampleTargetWithLayout));

        //Test1();
        //Test2();
        //Test3();
        Test4();
    }

    static void Test1()
    {
        var logger = LogManager.GetCurrentClassLogger();

        try
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            using var servicesProvider = new ServiceCollection()
                .AddTransient<Runner>()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog(config);
                })
                .BuildServiceProvider();

            var runner = servicesProvider.GetRequiredService<Runner>();

            runner.DoAction("てすと");

            Console.WriteLine("Press ANY key to exit");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Stopped program because of exception");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    static void Test2()
    {
        var logger = LoggerFactory
            .Create(builder => builder.AddNLog())
            .CreateLogger<Program>();

        logger.LogInformation("Program has started.");
        Console.ReadKey();
    }

    static void Test3()
    {
        var logger = LogManager.GetCurrentClassLogger();

        logger.Info("test.");
    }

    static void Test4()
    {
        var logger = LogManager.GetCurrentClassLogger();

        try
        {
            throw new Exception("Test exception");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Got exception.");  // NLog 4.0 and newer
        }
    }
}

