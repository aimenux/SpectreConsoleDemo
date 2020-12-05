namespace SpectreDemo.Examples
{
    public abstract class AbstractExample : IExample
    {
        public virtual string Description => GetType().Name;

        public virtual string Style => "[bold yellow on blue]{0}[/]";

        public abstract void Run();
    }
}
