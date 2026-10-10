using System.Threading;
using System.Threading.Tasks;

namespace App.Examples;

public interface IExample
{
    string Description { get; }
    string Style { get; }
    Task RunAsync(CancellationToken cancellationToken);
}