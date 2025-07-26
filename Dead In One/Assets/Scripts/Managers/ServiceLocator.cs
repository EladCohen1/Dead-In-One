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
}
