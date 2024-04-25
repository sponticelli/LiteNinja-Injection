namespace LiteNinja.Injection
{
    /// <summary>
    /// The AContext class is an abstract base class that provides a context for dependency injection.
    /// It holds an instance of the Injector class which is responsible for managing dependencies.
    /// The Injector instance can be initialized with a parent Injector, allowing for hierarchical dependency management.
    /// </summary>
    public abstract class AContext
    { 
        public readonly Injector Injector;
        public AContext(AContext parent)
        {
            Injector = parent != null ? new Injector(parent.Injector) : new Injector();
        }
    }
}