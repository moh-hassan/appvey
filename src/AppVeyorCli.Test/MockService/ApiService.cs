// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Test.Mocks;

using System;
using WireMock.Server;
using WireMock.Settings;
using static Api.ApiEndpoints;

internal partial class ApiService
{
    public static string BaseHttpUrl = MockBaseApi;
    public static WireMockServer Server = null!;

    public static void StartServer()
    {
        Server = WireMockServer.Start(new WireMockServerSettings
        {
            Urls = [BaseHttpUrl], // Set your desired base URL
            StartAdminInterface = true,
            SaveUnmatchedRequests = true,
            ReadStaticMappings = true,
            WatchStaticMappings = true,
            //WatchStaticMappingsInSubdirectories = true,
            // AllowCSharpCodeMatcher=true,
            //AllowBodyForAllHttpMethods,
        });
    }

    public static void StopServer()
    {
        Server.Stop();
        Console.WriteLine($"Server is stopped");
    }
}
