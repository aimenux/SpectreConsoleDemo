using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using SpectreDemo.Examples;
using System;
using System.IO;
using System.Linq;

namespace SpectreDemo
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var example = GetExampleFromPromptChoice(serviceScope);
                example?.Run();
            }

            Console.WriteLine("Press any key to exit !");
            Console.ReadKey();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.AddCommandLine(args);
                    config.AddEnvironmentVariables();
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    loggingBuilder.AddConsoleLogger();
                    loggingBuilder.AddNonGenericLogger();
                    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddSingleton<IExample, Example1>();
                    services.AddSingleton<IExample, Example2>();
                    services.AddSingleton<IExample, Example3>();
                    services.AddSingleton<IExample, Example4>();
                });

        private static void AddConsoleLogger(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.TimestampFormat = "[HH:mm:ss:fff] ";
            });
        }

        private static void AddNonGenericLogger(this ILoggingBuilder loggingBuilder)
        {
            var services = loggingBuilder.Services;
            services.AddSingleton(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                return loggerFactory.CreateLogger("SpectreDemo");
            });
        }

        private static IExample GetExampleFromPromptChoice(IServiceScope serviceScope)
        {
            var choice = AnsiConsole.Prompt(
                    new TextPrompt<int>("Which example do you want to run (0 for exit) ?")
                        .InvalidChoiceMessage("[red]That's not a valid choice[/]")
                        .DefaultValue(0)
                        .AddChoice(1)
                        .AddChoice(2)
                        .AddChoice(3)
                        .AddChoice(4));

            if (choice == 0) return null;
            var examples = serviceScope.ServiceProvider.GetServices<IExample>().ToList();
            return examples.SingleOrDefault(x => x.GetType().Name.EndsWith($"{choice}"));
        }
    }
}
