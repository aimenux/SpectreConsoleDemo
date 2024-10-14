using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace App.Examples;

public class Example3 : AbstractExample
{
    public override string Description => $"{GetType().Name} is about rendering progress bars (synchronous, asynchronous)";

    public override void Run()
    {
        SynchronousProgressBars();
        AsynchronousProgressBars().GetAwaiter().GetResult();
        AnotherAsynchronousProgressBars().GetAwaiter().GetResult();
    }

    private static void SynchronousProgressBars()
    {
        AnsiConsole.Progress()
            .Start(ctx =>
            {
                var task1 = ctx.AddTask("[green]Task one[/]");
                var task2 = ctx.AddTask("[green]Task two[/]");

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
                var task1 = ctx.AddTask("[green]Task one[/]");
                var task2 = ctx.AddTask("[green]Task two[/]");

                while (!ctx.IsFinished)
                {
                    await Task.Delay(100);
                    task1.Increment(2);
                    task2.Increment(1);
                }
            });
    }

    private static async Task AnotherAsynchronousProgressBars()
    {
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var works = Enumerable.Range(0, 10).Select(x => new Work($"Work-{x}", ctx)).ToList();
                var progressTasks = works.Select(x => x.ProgressTask).ToList();
                var tasks = works.Select(x => x.RunAsync()).ToList();
                var mainTask = Task.WhenAll(tasks);

                while (!ctx.IsFinished)
                {
                    await Task.Delay(20);
                    foreach (var progressTask in progressTasks)
                    {
                        progressTask.Increment(1);
                    }
                }

                await mainTask;
            });
    }

    private sealed class Work
    {
        public string Name { get; set; }
        public ProgressTask ProgressTask { get; }
        public async Task RunAsync() => await Task.Delay(2000);

        public Work(string name, ProgressContext context)
        {
            Name = name;
            ProgressTask = context.AddTask($"[green]{name} is running[/]");
        }
    }
}