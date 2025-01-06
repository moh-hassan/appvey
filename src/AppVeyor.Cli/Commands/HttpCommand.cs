// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppVeyor.Api;
#nullable disable
public partial class AppveyorCommand
{
    [CliCommand(Description = "Run Appveyor Rest Api")]
    public class HttpCommand : AppveyorCommandBase
    {
        protected override string Title => "Run Appveyor Rest Api ...";
        protected override string Tag => "http";

        [CliOption(Description = "Http method but Case insensitive", ValidationPattern = "^(?i)(get|put|post|delete)$")]
        public HttpRequest Method { get; set; } = HttpRequest.Get;

        [CliOption(Description = "json file name (without @ prefix)", Required = false, AllowMultipleArgumentsPerToken = true)]
        public FileInfo Json { get; set; }

        [CliArgument(Description = "Realative url, start with /api", Required = true, ValidationPattern = "^/api/.*$", Name = "Url")]
        public string Url { get; set; }

        protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
        {
            _ = Url ?? throw new ArgumentException("Url is required");
            var jsonString = Json == null
                ? ""
                : await File.ReadAllTextAsync(Json.FullName, ct);

            var result = Json == null
                ? await apiManager.RunHttpApiAsync(Url, Method, ct: ct).ConfigureAwait(false)
                : await apiManager
                   .RunHttpApiAsync(Url, Method, jsonString, WhatIf, ct).ConfigureAwait(false);

            return result;
        }
    }
}
#nullable restore
