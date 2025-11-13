using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple Impl Dependency Container / Service Locator
/// </summary>
public class DIContainer
{
    public static DIContainer Instance => _diFactory.Value;
    private static readonly Lazy<DIContainer> _diFactory = new(() => new DIContainer());
    
    private readonly Dictionary<Type, object> _bindings = new();
    
    public T Resolve<T>()
    {
        if (_bindings.TryGetValue(typeof(T), out var instance))
            return (T)instance;
        
        throw new InvalidOperationException("Type not found in container");
    }
    
    public void Resolve<T>(out T result)
    {
        if (_bindings.TryGetValue(typeof(T), out var instance))
        {
            result = (T) instance;
            return;
        }
        
        throw new InvalidOperationException("Type not found in container");
    }
    
    public void BindInstance<T>(T instance)
    {
        if (!_bindings.TryAdd(typeof(T), instance))
            throw new InvalidOperationException("Type already bound in container");
        
        Debug.Log("[DIContainer] Bound instance of type: " + typeof(T));
    }
    
    public void UnbindInstance<T>()
    {
        if (!_bindings.Remove(typeof(T)))
            throw new InvalidOperationException("Type not found in container");
        
        Debug.Log("[DIContainer] Unbound instance of type: " + typeof(T));
    }
}