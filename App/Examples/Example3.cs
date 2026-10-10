using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace App.Examples;

public class Example3 : AbstractExample
{
    public override string Description => $"{GetType().Name} is about rendering progress bars (synchronous, asynchronous, columns, indeterminate)";

    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        SynchronousProgressBars(cancellationToken);
        await AsynchronousProgressBars(cancellationToken);
        await AnotherAsynchronousProgressBars(cancellationToken);
        await ProgressColumns(cancellationToken);
        await IndeterminateProgressBars(cancellationToken);
    }

    private static void SynchronousProgressBars(CancellationToken cancellationToken)
    {
        AnsiConsole.Progress()
            .Start(ctx =>
            {
                var task1 = ctx.AddTask("[green]Work-0[/]");
                var task2 = ctx.AddTask("[green]Work-1[/]");
                while (!ctx.IsFinished)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Thread.Sleep(100);
                    task1.Increment(2);
                    task2.Increment(1);
                }
            });
    }

    private static async Task AsynchronousProgressBars(CancellationToken cancellationToken)
    {
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task1 = ctx.AddTask("[green]Work-0[/]");
                var task2 = ctx.AddTask("[green]Work-1[/]");
                while (!ctx.IsFinished)
                {
                    await Task.Delay(100, cancellationToken);
                    task1.Increment(2);
                    task2.Increment(1);
                }
            });
    }

    private static async Task AnotherAsynchronousProgressBars(CancellationToken cancellationToken)
    {
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var works = Enumerable.Range(0, 10).Select(x => new Work($"Work-{x}", ctx)).ToList();
                await Task.WhenAll(works.Select(x => x.RunAsync(cancellationToken)));
            });
    }

    private static async Task ProgressColumns(CancellationToken cancellationToken)
    {
        await AnsiConsole.Progress()
            .Columns(
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new RemainingTimeColumn(),
                new SpinnerColumn(),
                new StateColumn())
            .StartAsync(async ctx =>
            {
                var download = ctx.AddTask("[green]Downloading[/]");
                var install = ctx.AddTask("[blue]Installing[/]", autoStart: false);

                while (!ctx.IsFinished)
                {
                    await Task.Delay(50, cancellationToken);

                    if (download.IsFinished && !install.IsStarted)
                    {
                        install.StartTask();
                    }

                    download.Increment(1.5);

                    if (install.IsStarted)
                    {
                        install.Increment(1);
                    }
                }
            });
    }

    private static async Task IndeterminateProgressBars(CancellationToken cancellationToken)
    {
        await AnsiConsole.Progress()
            .Columns(
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new SpinnerColumn(),
                new StateColumn())
            .StartAsync(async ctx =>
            {
                var connect = ctx.AddTask("[yellow]Connecting (unknown duration)[/]");
                connect.IsIndeterminate = true;
                var transfer = ctx.AddTask("[green]Transferring[/]", autoStart: false);

                await Task.Delay(2000, cancellationToken);

                connect.IsIndeterminate = false;
                connect.Value = connect.MaxValue;
                transfer.StartTask();

                while (!transfer.IsFinished)
                {
                    await Task.Delay(30, cancellationToken);
                    transfer.Increment(2);
                }
            });
    }

    private sealed class StateColumn : ProgressColumn
    {
        public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
        {
            if (task.IsFinished)
            {
                return new Markup("[green]done[/]");
            }

            if (!task.IsStarted)
            {
                return new Markup("[grey]queued[/]");
            }

            return task.IsIndeterminate ? new Markup("[yellow]waiting[/]") : new Markup("[blue]running[/]");
        }

        public override int? GetColumnWidth(RenderOptions options) => 7;
    }

    private sealed class Work
    {
        private readonly string _name;
        private readonly ProgressTask _progressTask;
        
        public Work(string name, ProgressContext context)
        {
            _name = name;
            _progressTask = context.AddTask($"[green]{name} is running[/]");
        }
        
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!_progressTask.IsFinished)
            {
                await Task.Delay(20, cancellationToken);
                _progressTask.Increment(Random.Shared.Next(1, 5));
            }
        }
    }
}