# LiteNinja Dependency Injection

LiteNinja Injection is a lightweight dependency injection framework for Unity3D. It provides a simple and efficient way to manage dependencies within your application.

## Key Components

- `Injector`: The main class responsible for managing dependencies. It provides functionality to bind objects to their respective types and inject these objects into fields marked with the `InjectAttribute` attribute.

- `InjectAttribute`: A custom attribute used to mark fields that should be injected with dependencies.

- `AContext`: An abstract base class that provides a context for dependency injection. It holds an instance of the Injector class which is responsible for managing dependencies.

- `InjectorException`: An exception that is thrown when the `Injector` cannot resolve a dependency.

## Usage

### 1. Creating an Injector

You can create an instance of the `Injector` class as follows:

```csharp
Injector injector = new Injector();
```

You can also create a child `Injector` that inherits bindings from a parent `Injector`:

```csharp
Injector childInjector = new Injector(parentInjector);
```

### 2. Binding Dependencies

You can bind an object to its type using the `Bind<T>` method:

```csharp
injector.Bind<MyClass>(new MyClass());
```

### 3. Injecting Dependencies

To inject dependencies into an object, use the `Inject` method:

```csharp
MyClass myObject = new MyClass();
injector.Inject(myObject);
```

This will inject dependencies into all fields of `myObject` that are marked with the `InjectAttribute`.

### 4. Retrieving Instances

You can retrieve an instance of a bound type using the `GetInstance<T>` method:

```csharp
MyClass myObject = injector.GetInstance<MyClass>();
```

### 5. Using the Inject Attribute

Mark fields that should be injected with the `InjectAttribute`:

```csharp
public class MyClass
{
    [Inject]
    private MyDependency _myDependency;
}
```

When `MyClass` is injected, `_myDependency` will be filled with an instance of `MyDependency` that is retrieved from the `Injector`.

## Thread Safety

By defining `MULTI_THREADED_INJECTION`, the `Injector` class will use `ConcurrentDictionary` instead of `Dictionary` for storing bindings, making it safe to use in a multi-threaded environment.

## Exception Handling

If the `Injector` cannot resolve a dependency, it will throw an `InjectorException`. You can catch this exception and handle it as appropriate for your application.

## Note

Remember to call `PostBindings` after all bindings have been made. This will ensure that all bound objects are themselves injected.

```csharp
injector.PostBindings();
```

That's it! You're now ready to start using LiteNinja Injection in your Unity3d applications.