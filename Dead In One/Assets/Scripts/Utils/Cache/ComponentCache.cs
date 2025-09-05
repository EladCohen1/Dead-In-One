using System;
using System.Collections.Generic;
using UnityEngine;

public class ComponentCache : MonoBehaviour
{
    private Dictionary<Type, Component> _cache = new();

    public T Get<T>() where T : Component
    {
        Type requestedType = typeof(T);

        if (_cache.TryGetValue(requestedType, out Component cached))
            return cached as T;

        Component found = GetComponent<T>();

        if (found == null)
            return null;


        Type currentType = found.GetType();
        while (currentType != null
        && typeof(Component).IsAssignableFrom(currentType)
        && currentType != typeof(MonoBehaviour))
        {
            if (!_cache.ContainsKey(currentType))
                _cache[currentType] = found;

            currentType = currentType.BaseType;
        }

        foreach (Type iface in found.GetType().GetInterfaces())
        {
            if (typeof(Component).IsAssignableFrom(iface) && !_cache.ContainsKey(iface))
                _cache[iface] = found;
        }

        if (!_cache.ContainsKey(requestedType))
            _cache[requestedType] = found;

        return found as T;
    }
}
