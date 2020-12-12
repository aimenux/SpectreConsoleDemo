using Spectre.Console;
using System;

namespace SpectreDemo.Examples
{
    public class Example2 : AbstractExample
    {
        public override string Description => $"{GetType().Name} is about rendering prompts (choices, secrets, calendars)";

        public override void Run()
        {
            BasicPrompts();
            SecretsPrompts();
            ChoicesPrompts();
        }

        private static void BasicPrompts()
        {
            AnsiConsole.WriteLine();

            var name = AnsiConsole.Ask<string>("What's your [blue]name[/]?");

            AnsiConsole.WriteLine();

            var age = AnsiConsole.Prompt(
                new TextPrompt<int>("What's the age?")
                    .Validate(age =>
                    {
                        return age switch
                        {
                            < 10 => ValidationResult.Error("[red]Too young[/]"),
                            > 90 => ValidationResult.Error("[red]Too old[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));
        }

        private static void SecretsPrompts()
        {
            AnsiConsole.WriteLine();

            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]password[/]")
                    .PromptStyle("red")
                    .Secret());
        }

        private static void ChoicesPrompts()
        {
            AnsiConsole.WriteLine();

            var choice = AnsiConsole.Prompt(
                new TextPrompt<string>("What's your [green]favorite fruit[/]?")
                    .InvalidChoiceMessage("[red]That's not a valid fruit[/]")
                    .DefaultValue("Orange")
                    .AddChoice("Apple")
                    .AddChoice("Banana")
                    .AddChoice("Orange"));

            AnsiConsole.WriteLine();

            var optionalChoice = AnsiConsole.Prompt(
                new TextPrompt<string>("[grey][[Optional]][/] [green]Favorite fruit[/]?")
                .AllowEmpty());

            AnsiConsole.WriteLine();
        }
    }
}
