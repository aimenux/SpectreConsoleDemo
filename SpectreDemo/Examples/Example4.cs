using Spectre.Console;
using System;

namespace SpectreDemo.Examples
{
    public class Example4 : AbstractExample
    {
        public override string Description => $"{GetType().Name} is about rendering widgets (table, rule, calendar)";

        public override void Run()
        {
            RenderTables();
            RenderRules();
            RenderCalendar();
            RenderFiglets();
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

            var rule1 = new Rule("[green]Hello[/]");
            rule1.Alignment = Justify.Left;
            AnsiConsole.Write(rule1);

            AnsiConsole.WriteLine();

            var rule2 = new Rule("[blue]Hello[/]");
            rule2.Alignment = Justify.Center;
            AnsiConsole.Write(rule2);

            AnsiConsole.WriteLine();

            var rule3 = new Rule("[red]Hello[/]");
            rule3.Alignment = Justify.Right;
            AnsiConsole.Write(rule3);
        }

        private static void RenderCalendar()
        {
            AnsiConsole.WriteLine();

            var calendar = new Calendar(DateTime.Now.Year, DateTime.Now.Month);
            AnsiConsole.Write(calendar);
        }

        private static void RenderFiglets()
        {
            AnsiConsole.WriteLine();

            AnsiConsole.Write(
                new FigletText("Figlets")
                    .LeftAligned()
                    .Color(Color.Navy));

            AnsiConsole.WriteLine();
        }
    }
}
