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

    public static void UnRegister<T>(T service) where T : MonoBehaviour
    {
        if (service is T)
            services.Remove(typeof(T));
    }

    public static T Get<T>() where T : MonoBehaviour
    {
        if (services.TryGetValue(typeof(T), out MonoBehaviour result))
            return result as T;

        T service = FindAnyObjectByType<T>();
        if (service)
            services[typeof(T)] = service;

        return service;
    }

    public static bool TryGet<T>(out T service) where T : MonoBehaviour
    {
        if (services.TryGetValue(typeof(T), out var obj) && obj is T typed)
        {
            service = typed;
            return true;
        }

        service = Get<T>();
        if (service)
            return true;

        service = null;
        return false;
    }

    public static void CloseAllMenus(MonoBehaviour exluded)
    {
        foreach (var (type, UIMenu) in services)
        {
            if (exluded == UIMenu)
                continue;
            if (UIMenu is IClosable closable)
                closable.Close();
        }
    }
}
