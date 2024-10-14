using System;
using System.Text;

namespace App.Examples;

public abstract class AbstractExample : IExample
{
    static AbstractExample()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }

    public abstract string Description { get; }

    public virtual string Style => "[bold yellow on blue]{0}[/]";

    public abstract void Run();
}