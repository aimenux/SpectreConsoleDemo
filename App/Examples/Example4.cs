using System;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace App.Examples;

public class Example4 : AbstractExample
{
    public override string Description => $"{GetType().Name} is about rendering widgets (table, rule, calendar, figlet, panel, tree, chart)";

    public override Task RunAsync(CancellationToken cancellationToken)
    {
        RenderTables();
        RenderRules();
        RenderCalendar();
        RenderFiglets();
        RenderPanel();
        RenderTree();
        RenderBarChart();

        return Task.CompletedTask;
    }

    private static void RenderTables()
    {
        AnsiConsole.WriteLine();

        var table = new Table()
            .Border(TableBorder.Square)
            .BorderColor(Color.Red)
            .AddColumn(new TableColumn("[u]CDE[/]").Footer("EDC").Centered())
            .AddColumn(new TableColumn("[u]FED[/]").Footer("DEF"))
            .AddColumn(new TableColumn("[u]IHG[/]").Footer("GHI"))
            .AddRow("Hello", "[red]World![/]", "")
            .AddRow("[blue]Bonjour[/]", "[white]le[/]", "[red]monde![/]")
            .AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "");

        AnsiConsole.Write(table);
    }

    private static void RenderRules()
    {
        AnsiConsole.WriteLine();

        var rules = new[]
        {
            (Color: "green", Justification: Justify.Left),
            (Color: "blue", Justification: Justify.Center),
            (Color: "red", Justification: Justify.Right),
        };

        foreach (var (color, justification) in rules)
        {
            AnsiConsole.Write(new Rule($"[{color}]Hello[/]") { Justification = justification });
            AnsiConsole.WriteLine();
        }
    }

    private static void RenderCalendar()
    {
        AnsiConsole.WriteLine();

        var today = DateTime.Today;
        var calendar = new Calendar(today.Year, today.Month)
            .AddCalendarEvent(today)
            .HighlightStyle(Spectre.Console.Style.Parse("bold green"));
        AnsiConsole.Write(calendar);
    }

    private static void RenderFiglets()
    {
        AnsiConsole.WriteLine();

        AnsiConsole.Write(
            new FigletText("Figlets")
                .LeftJustified()
                .Color(Color.Navy));

        AnsiConsole.WriteLine();
    }

    private static void RenderPanel()
    {
        AnsiConsole.WriteLine();

        var panel = new Panel("[green]Panels[/] wrap any content in a border")
            .Header("[yellow]Panel[/]")
            .Border(BoxBorder.Rounded)
            .Padding(1, 0);
        AnsiConsole.Write(panel);
    }

    private static void RenderTree()
    {
        AnsiConsole.WriteLine();

        var tree = new Tree("[yellow]Solution[/]");
        var examples = tree.AddNode("[green]Examples[/]");
        examples.AddNodes("Example1", "Example2", "Example3", "Example4");
        tree.AddNode("Program");
        AnsiConsole.Write(tree);
    }

    private static void RenderBarChart()
    {
        AnsiConsole.WriteLine();

        var chart = new BarChart()
            .Width(60)
            .Label("[underline]Fruits sold[/]")
            .AddItem("Apple", 12, Color.Red)
            .AddItem("Banana", 8, Color.Yellow)
            .AddItem("Orange", 15, Color.Orange1);
        AnsiConsole.Write(chart);
    }
}