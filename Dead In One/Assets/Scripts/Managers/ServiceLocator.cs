using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private static Dictionary<Type, MonoBehaviour> services = new();

    public static void Register<T>(T service) where T : MonoBehaviour
    {
        services[typeof(T)] = service;
    }

    public static T Get<T>() where T : MonoBehaviour
    {
        return services[typeof(T)] as T;
    }

    public static T TryGet<T>() where T : Component
    {
        Type requestedType = typeof(T);

        if (services.TryGetValue(requestedType, out MonoBehaviour cached))
            return cached as T;

        MonoBehaviour found = FindAnyObjectByType<T>() as MonoBehaviour;

        if (found == null)
            return null;


        Type currentType = found.GetType();
        while (currentType != null
        && typeof(MonoBehaviour).IsAssignableFrom(currentType)
        && currentType != typeof(MonoBehaviour))
        {
            if (!services.ContainsKey(currentType))
                services[currentType] = found;

            currentType = currentType.BaseType;
        }

        foreach (Type iface in found.GetType().GetInterfaces())
        {
            if (typeof(Component).IsAssignableFrom(iface) && !services.ContainsKey(iface))
                services[iface] = found;
        }

        if (!services.ContainsKey(requestedType))
            services[requestedType] = found;

        return found as T;
    }
}
