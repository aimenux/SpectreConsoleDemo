using System;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace App.Examples;

public class Example5 : AbstractExample
{
    public override string Description => $"{GetType().Name} is about rendering live displays (status spinners, live tables)";

    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        await StatusSpinners(cancellationToken);
        await LiveTable(cancellationToken);
    }

    private static async Task StatusSpinners(CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine();

        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(Spectre.Console.Style.Parse("green bold"))
            .StartAsync("Initializing...", async ctx =>
            {
                await Task.Delay(1500, cancellationToken);
                AnsiConsole.MarkupLine("[grey]Configuration loaded[/]");

                ctx.Status("Connecting to the server...");
                ctx.Spinner(Spinner.Known.Star);
                ctx.SpinnerStyle(Spectre.Console.Style.Parse("yellow"));
                await Task.Delay(1500, cancellationToken);
                AnsiConsole.MarkupLine("[grey]Connected[/]");

                ctx.Status("Processing data...");
                ctx.Spinner(Spinner.Known.Arc);
                ctx.SpinnerStyle(Spectre.Console.Style.Parse("blue"));
                await Task.Delay(1500, cancellationToken);
            });

        AnsiConsole.MarkupLine("[green]Done![/]");
    }

    private static async Task LiveTable(CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("Time")
            .AddColumn("Event")
            .AddColumn("Status");

        await AnsiConsole.Live(table)
            .StartAsync(async ctx =>
            {
                for (var i = 1; i <= 5; i++)
                {
                    table.AddRow(DateTime.Now.ToString("HH:mm:ss"), $"Event-{i}", "[yellow]running[/]");
                    ctx.Refresh();
                    await Task.Delay(600, cancellationToken);

                    table.UpdateCell(i - 1, 2, "[green]done[/]");
                    ctx.Refresh();
                    await Task.Delay(300, cancellationToken);
                }
            });
    }
}