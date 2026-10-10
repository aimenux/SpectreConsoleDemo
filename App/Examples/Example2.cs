using System;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace App.Examples;

public class Example2 : AbstractExample
{
    public override string Description => $"{GetType().Name} is about rendering prompts (choices, selections, secrets, confirmations)";

    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        await BasicPrompts(cancellationToken);
        await SecretsPrompts(cancellationToken);
        await ChoicesPrompts(cancellationToken);
        await SelectionPrompts(cancellationToken);
        await MultiSelectionPrompts(cancellationToken);
        await ConfirmationPrompts(cancellationToken);
    }

    private static async Task BasicPrompts(CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine();

        var name = await AnsiConsole.AskAsync<string>("What's your [blue]name[/]?", cancellationToken);
        
        AnsiConsole.Write($"Name is {name}");

        AnsiConsole.WriteLine();

        var age = await AnsiConsole.PromptAsync(
            new TextPrompt<int>("What's the age?")
                .Validate(value =>
                {
                    return value switch
                    {
                        < 10 => ValidationResult.Error("[red]Too young[/]"),
                        > 90 => ValidationResult.Error("[red]Too old[/]"),
                        _ => ValidationResult.Success(),
                    };
                }), cancellationToken);
        
        AnsiConsole.Write($"Age is {age}");
    }

    private static async Task SecretsPrompts(CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine();

        var password = await AnsiConsole.PromptAsync(
            new TextPrompt<string>("Enter [green]password[/]")
                .PromptStyle("red")
                .Secret(), cancellationToken);
        
        AnsiConsole.Write($"Password is {new string('*', password.Length)}");
    }

    private static async Task ChoicesPrompts(CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine();

        var choice = await AnsiConsole.PromptAsync(
            new TextPrompt<string>("What's your [green]favorite fruit[/]?", StringComparer.OrdinalIgnoreCase)
                .InvalidChoiceMessage("[red]That's not a valid fruit[/]")
                .DefaultValue("Orange")
                .AddChoice("Apple")
                .AddChoice("Banana")
                .AddChoice("Orange"), cancellationToken);
        
        AnsiConsole.Write($"Choice is {choice}");

        AnsiConsole.WriteLine();

        var optionalChoice = await AnsiConsole.PromptAsync(
            new TextPrompt<string>("[grey][[Optional]][/] [green]Favorite fruit[/]?")
                .AllowEmpty(), cancellationToken);
        
        AnsiConsole.Write($"OptionalChoice is {optionalChoice}");

        AnsiConsole.WriteLine();
    }

    private static async Task SelectionPrompts(CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine();

        var language = await AnsiConsole.PromptAsync(
            new SelectionPrompt<string>()
                .Title("What's your [green]favorite language[/]?")
                .PageSize(6)
                .WrapAround()
                .MoreChoicesText("[grey](Move up and down to reveal more languages)[/]")
                .Mode(SelectionMode.Leaf)
                .AddChoiceGroup("Compiled", "C#", "Rust", "Go")
                .AddChoiceGroup("Interpreted", "Python", "JavaScript", "Ruby"), cancellationToken);

        AnsiConsole.Write($"Language is {language}");

        AnsiConsole.WriteLine();
    }

    private static async Task MultiSelectionPrompts(CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine();

        var features = await AnsiConsole.PromptAsync(
            new MultiSelectionPrompt<string>()
                .Title("Which [green]features[/] do you want to enable?")
                .NotRequired()
                .PageSize(6)
                .InstructionsText("[grey](Press [blue]<space>[/] to toggle a feature, [green]<enter>[/] to accept)[/]")
                .AddChoices("Logging", "Caching", "Authentication")
                .AddChoiceGroup("Observability", "Metrics", "Tracing")
                .Select("Logging"), cancellationToken);

        AnsiConsole.Write($"Features are {(features.Count == 0 ? "none" : string.Join(", ", features))}");

        AnsiConsole.WriteLine();
    }

    private static async Task ConfirmationPrompts(CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine();

        var likesFruits = await AnsiConsole.ConfirmAsync("Do you [green]like fruits[/]?", cancellationToken: cancellationToken);

        AnsiConsole.Write($"Likes fruits is {likesFruits}");

        AnsiConsole.WriteLine();

        var wantsMore = await AnsiConsole.ConfirmAsync("Do you want [blue]more[/]?", defaultValue: false, cancellationToken: cancellationToken);

        AnsiConsole.Write($"Wants more is {wantsMore}");

        AnsiConsole.WriteLine();
    }
}