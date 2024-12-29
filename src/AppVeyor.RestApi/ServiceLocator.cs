// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> Services = new();

    public static void RegisterService<T>(T? service) where T : class
    {
        Services[typeof(T)] = service ?? throw new ArgumentNullException($"ServiceLocator Exception: service '{nameof(service)}' is not found.");
    }

    public static T GetService<T>([CallerMemberName] string callerName = "") where T : class
    {
        return Services[typeof(T)] as T
               ?? throw new InvalidOperationException($"Service of type {typeof(T)} is not registered.");
    }
}
