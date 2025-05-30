using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class AddSubclassAttribute : PropertyAttribute
{
    public Type BaseType { get; }

    public AddSubclassAttribute(Type baseType)
    {
        BaseType = baseType;
    }
}
