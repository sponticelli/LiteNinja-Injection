using System;
#if MULTI_THREADED_INJECTION
using System.Collections.Concurrent;
#else
using System.Collections.Generic;
#endif
using System.Linq;
using System.Reflection;

namespace LiteNinja.Injection
{
    /// <summary>
    /// The Injector class is responsible for managing dependencies within the application.
    /// It provides functionality to bind objects to their respective types and inject these objects into fields
    /// marked with the InjectAttribute attribute.
    /// It also supports hierarchical injection through parent Injectors.
    /// Define MULTI_THREADED_INJECTION to enable multi-threaded injection.
    /// </summary>
    public class Injector
    {
        private readonly Injector _parentInjector;

#if !MULTI_THREADED_INJECTION
        private static readonly Dictionary<Type, Lazy<FieldInfo[]>> CachedFieldInfos = new();
        private readonly Dictionary<Type, object> _objects = new();
#else
        private static readonly ConcurrentDictionary<Type, Lazy<FieldInfo[]>> CachedFieldInfos = new();
        private readonly ConcurrentDictionary<Type, object> _objects = new();
#endif

		/// <summary>
	    /// Creates a new Injector instance with an optional parent Injector.
		/// </summary>
        public Injector(Injector parent = null)
        {
            _parentInjector = parent;
            Bind(this);
        }

		/// <summary>
		/// Binds an object to its type.
	    /// </summary>
        public void Bind<T>(T obj)
        {
            var type = typeof(T);
            _objects[type] = obj;
        }
        
		/// <summary>
		/// Unbinds an object from its type.
		/// </summary>
        public void Unbind<T>()
        {
            var type = typeof(T);
            if (_objects.ContainsKey(type))
            {
                _objects.Remove(type);
            }
        }

        public void PostBindings()
        {
            foreach (var @object in _objects)
            {
                Inject(@object.Value);
            }
        }

        public void Inject(object obj)
        {
            var array = Reflect(obj.GetType()).Value;
            var num = array.Length;
            for (var i = 0; i < num; i++)
            {
                var fieldInfo = array[i];
                var value = Get(fieldInfo.FieldType);
                fieldInfo.SetValue(obj, value);
            }
        }

        private object Get(Type type)
        {
            if (_objects.TryGetValue(type, out var value)) return value;
            if (_parentInjector != null)
            {
                return _parentInjector.Get(type);
            }

            throw new InjectorException("Could not get " + type.FullName + " from injector", type);
        }

        public T GetInstance<T>()
        {
            return (T)Get(typeof(T));
        }

        private static Lazy<FieldInfo[]> Reflect(Type type)
        {
#if !MULTI_THREADED_INJECTION
            if (CachedFieldInfos.TryGetValue(type, out var lazy))
            {
                return lazy;
            }

            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public |
                                        BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            var value = new Lazy<FieldInfo[]>(() => (from fieldInfo in fields
                let customAttributes = fieldInfo.GetCustomAttributes(typeof(InjectAttribute), false)
                where customAttributes.Length > 0
                select fieldInfo).ToArray());
            CachedFieldInfos[type] = value;
            return value;
#else
            return CachedFieldInfos.GetOrAdd(type, t => new Lazy<FieldInfo[]>(() =>
            {
                var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public |
                                         BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                return (from fieldInfo in fields
                    let customAttributes = fieldInfo.GetCustomAttributes(typeof(InjectAttribute), false)
                    where customAttributes.Length > 0
                    select fieldInfo).ToArray();
            }));
#endif
        }
    }
}