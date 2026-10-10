using System.Threading;
using System.Threading.Tasks;

namespace App.Examples;

public abstract class AbstractExample : IExample
{
    public abstract string Description { get; }

    public virtual string Style => "[bold yellow on blue]{0}[/]";

    public abstract Task RunAsync(CancellationToken cancellationToken);
}