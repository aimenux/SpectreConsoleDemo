using Spectre.Console;
using System;
using System.Threading.Tasks;

namespace SpectreDemo.Examples
{
    public class Example3 : AbstractExample
    {
        public override string Description => $"{GetType().Name} is about rendering progress bars (synchronous, asynchronous)";

        public override void Run()
        {
            SynchronousProgressBars();
            AsynchronousProgressBars().GetAwaiter().GetResult();
        }

        private static void SynchronousProgressBars()
        {
            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    var task1 = ctx.AddTask("[green]Reticulating splines[/]");
                    var task2 = ctx.AddTask("[green]Folding space[/]");

                    while (!ctx.IsFinished)
                    {
                        task1.Increment(0.5);
                        task2.Increment(0.1);
                    }
                });
        }

        private static async Task AsynchronousProgressBars()
        {
            await AnsiConsole.Progress()
                .StartAsync(async ctx =>
                {
                    var task1 = ctx.AddTask("[green]Reticulating splines[/]");
                    var task2 = ctx.AddTask("[green]Folding space[/]");

                    while (!ctx.IsFinished)
                    {
                        await Task.Delay(100);
                        task1.Increment(2);
                        task2.Increment(1);
                    }
                });
        }
    }
}
