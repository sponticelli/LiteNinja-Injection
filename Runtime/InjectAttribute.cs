using System;
using UnityEngine.Scripting;

namespace LiteNinja.Injection
{
    /// <summary>
    /// The InjectAttribute attribute is used to mark fields that should be injected with dependencies.
    /// The InjectAttribute class is a custom attribute that extends Unity's PreserveAttribute.
    /// It extends the PreserveAttribute to ensure that it is not stripped by IL2CPP during the conversion process.
    /// It is used to mark fields that should be injected with dependencies.
    /// This attribute should be used in conjunction with the Injector class, which manages the actual injection of dependencies.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectAttribute : PreserveAttribute
    {
    }
}