using System;

namespace LiteNinja.Injection
{
    public class InjectorException : Exception
    {
        public Type InjectedType { get; }
        public InjectorException(string message, Type injectedType) : base(message)
        {
            InjectedType = injectedType;
        }
    }
}