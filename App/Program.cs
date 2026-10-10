using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Examples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace App;

public static class Program
{
    static Program()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
    }

    public static async Task Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();

        using var serviceScope = host.Services.CreateScope();

        using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
        var cancellationToken = cts.Token;
        
        var example = await GetSelectedExampleAsync(serviceScope, cancellationToken);
        if (example is not null)
        {
            await example.RunAsync(cancellationToken);
        }

        Console.WriteLine("Press any key to exit !");
        Console.ReadKey();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureLogging(loggingBuilder => loggingBuilder.AddConsoleLogger())
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IExample, Example1>();
                services.AddSingleton<IExample, Example2>();
                services.AddSingleton<IExample, Example3>();
                services.AddSingleton<IExample, Example4>();
                services.AddSingleton<IExample, Example5>();
                services.AddSingleton<IExample, Example6>();
            });

    private static void AddConsoleLogger(this ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.AddSimpleConsole(options =>
        {
            options.IncludeScopes = true;
            options.TimestampFormat = "[HH:mm:ss:fff] ";
        });
    }

    private static async Task<IExample> GetSelectedExampleAsync(IServiceScope serviceScope, CancellationToken cancellationToken)
    {
        var examples = serviceScope.ServiceProvider.GetServices<IExample>().ToList();

        for (var i = 0; i < examples.Count; i++)
        {
            var description = Markup.Escape(examples[i].Description);
            AnsiConsole.MarkupLine($"{i + 1} - {string.Format(examples[i].Style, description)}");
        }

        var choice = await AnsiConsole.PromptAsync(
            new TextPrompt<int>("Which example do you want to run (0 for exit) ?")
                .InvalidChoiceMessage("[red]That's not a valid choice[/]")
                .DefaultValue(0)
                .AddChoices(Enumerable.Range(0, examples.Count + 1)), cancellationToken);

        return choice == 0 ? null : examples[choice - 1];
    }
}