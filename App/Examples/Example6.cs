using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace App.Examples;

public class Example6 : AbstractExample
{
    public override string Description => $"{GetType().Name} is about rendering layouts (columns, grid, padder, align, layout)";

    public override Task RunAsync(CancellationToken cancellationToken)
    {
        RenderColumns();
        RenderGrid();
        RenderPadderAndAlign();
        RenderLayout();

        return Task.CompletedTask;
    }

    private static void RenderColumns()
    {
        AnsiConsole.WriteLine();

        AnsiConsole.Write(new Rule("[yellow]Columns[/]").LeftJustified());
        AnsiConsole.Write(new Columns(
            new Panel("[green]Apple[/]").Expand(),
            new Panel("[yellow]Banana[/]").Expand(),
            new Panel("[orange1]Orange[/]").Expand()));
    }

    private static void RenderGrid()
    {
        AnsiConsole.WriteLine();

        AnsiConsole.Write(new Rule("[yellow]Grid[/]").LeftJustified());

        var grid = new Grid();
        grid.AddColumn(new GridColumn { Alignment = Justify.Right });
        grid.AddColumn(new GridColumn { Alignment = Justify.Left });

        grid.AddRow("Amount:", "$1,234.56");
        grid.AddRow("Tax:", "$123.46");
        grid.AddRow("Total:", "[bold green]$1,358.02[/]");

        AnsiConsole.Write(grid);
    }

    private static void RenderPadderAndAlign()
    {
        AnsiConsole.WriteLine();

        AnsiConsole.Write(new Rule("[yellow]Padder and Align[/]").LeftJustified());

        AnsiConsole.Write(new Padder(new Panel("[blue]Padded panel[/]"), new Padding(4, 1, 4, 1)));
        AnsiConsole.Write(Align.Center(new Panel("[green]Centered panel[/]")));
        AnsiConsole.Write(Align.Right(new Panel("[red]Right-aligned panel[/]")));
    }

    private static void RenderLayout()
    {
        AnsiConsole.WriteLine();

        AnsiConsole.Write(new Rule("[yellow]Layout[/]").LeftJustified());

        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Left"),
                new Layout("Right").SplitRows(
                    new Layout("Top"),
                    new Layout("Bottom")));

        layout["Left"].Update(Cell("Left", "blue"));
        layout["Top"].Update(Cell("Top right", "green"));
        layout["Bottom"].Update(Cell("Bottom right", "red"));

        AnsiConsole.Write(new Panel(layout) { Height = 12 }.Expand());
    }

    private static Panel Cell(string text, string color) =>
        new Panel(Align.Center(new Markup($"[{color}]{text}[/]"), VerticalAlignment.Middle)).Expand();
}