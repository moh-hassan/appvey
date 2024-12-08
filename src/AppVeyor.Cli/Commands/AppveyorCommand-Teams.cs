// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;

#nullable disable
using Api;

public partial class AppveyorCommand
{
    [CliCommand(Description = "Users command")]
    public class Users
    {
        [CliCommand(Description = "List all Users")]
        public class List : AppveyorCommandBase
        {
            protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
            {
                var result = await apiManager.GetUsersAsync(ct);
                return result;
            }
        }


        [CliCommand(Description = "Get user by UserId")]
        public class Get : AppveyorCommandBase
        {
            [CliArgument(Description = "User Id")]
            public string Id { get; set; }

            protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
            {
                var result = await apiManager.GetUserAsync(Id, ct);
                return result;
            }
        }

        [CliCommand(Description = "Add user")]
        public class Add : AppveyorCommandBase
        {
            [CliArgument(Description = "FullPath of json file")]
            public FileInfo JsonFile { get; set; }

            protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
            {
                var fileName = JsonFile.FullName;
                var json = await File.ReadAllTextAsync(fileName, ct);
                var result = await apiManager.AddUserAsync(json, ct);
                return result;
            }
        }

        //update users
        [CliCommand(Description = "Update user")]
        public class Update : AppveyorCommandBase
        {
            [CliArgument(Description = "FullPath or relative of the json file")]
            public FileInfo JsonFile { get; set; }

            protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
            {
                var fileName = JsonFile.FullName;
                var json = await File.ReadAllTextAsync(fileName, ct);
                var result = await apiManager.UpdateUserAsync(json, ct);
                return result;
            }
        }

        [CliCommand(Description = "Delete user", Aliases = ["del"])]
        public class Delete : AppveyorCommandBase
        {
            [CliArgument] public string Id { get; set; }

            protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
            {
                var result = await apiManager.DeleteUserAsync(Id, ct);
                return result;
            }
        }
    }

#if x

    // [CliCommand(Aliases = ["col"])]
    public class Collaborators
    {
        [CliCommand(Description = "List Collaborators")]
        public class List : AppveyorCommandBase
        {
            protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
            {
                var result = await apiManager.GetCollaboratorsAsync(ct);
                return result;
            }
        }

        [CliCommand(Description = "Get Collaborator")]
        public class Get : AppveyorCommandBase
        {
            [CliArgument] public string Id { get; set; }

            protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
            {
                var result = await apiManager.GetCollaboratorAsync(Id, ct);
                return result;
            }
        }


        //405 (Method Not Allowed). Allow: GET,PUT

        [CliCommand(Description = "Add Collaborator")]
        public class Add : AppveyorCommandBase
        {
            [CliOption] public string Email { get; set; }

            [CliOption] public string RoleId { get; set; }

            protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
            {
                var result = await apiManager.AddCollaboratorAsync(Email, RoleId, ct);
                return result;
            }
        }


        [CliCommand(Description = "Collaborator Update")]
        public class Update : AppveyorCommandBase
        {
            [CliOption] public string UserId { get; set; }

            [CliOption] public string RoleId { get; set; }

            protected override async Task<ResponseResult> RunApiAsync(
                ApiManager apiManager, CancellationToken ct)
            {
                //return await Task.FromResult(ResponseResult.Instance);
                Console.WriteLine("Collaborators Update ");
                var result = await apiManager.UpdateCollaboratorAsync(UserId, RoleId, ct);
                return result;
            }
        }

        [CliCommand(Description = "Delete Collaborators", Aliases = ["del"])]
        public class Delete : AppveyorCommandBase
        {
            [CliArgument(Description = "Get collaborator by Id")]
            public string Id { get; set; }

            protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
            {
                var result = await apiManager.DeleteCollaboratorAsync(Id, ct);
                return result;
            }
        }
    }

    // [CliCommand]
    public class Roles
    {
        [CliCommand(Description = "List roles")]
        public class List : AppveyorCommandBase
        {
            protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
            {
                var result = await apiManager.GetRolesAsync(ct);
                return result;
            }
        }

        [CliCommand(Description = "Get role by roleId")]
        public class Get : AppveyorCommandBase
        {
            [CliArgument] public string Id { get; set; }

            protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
            {
                var result = await apiManager.GetRoleAsync(Id, ct);
                return result;
            }
        }

        [CliCommand(Description = "Add role")]
        public class Add : AppveyorCommandBase
        {
        }

        [CliCommand(Description = "Update role")]
        public class Update : CredentialCommandBase
        {
        }

        [CliCommand(Description = "Delete role", Aliases = ["del"])]
        public class Delete : AppveyorCommandBase
        {
        }
    } // end of Roles
#endif

    [CliCommand(Description = "Run Appveyor Rest Api")]
    public class Http : AppveyorCommandBase
    {
        [CliOption(Description = "Http method. Allowed values: [get, post, put, delete] but Case insensitive", ValidationPattern = "^(?i)(get|put|post|delete)$")]
        public string Method { get; set; } = "get";

        [CliOption(Description = "json data. Can be @fileName", Required = false, AllowMultipleArgumentsPerToken = true)]
        public string Json { get; set; }

        [CliArgument(Description = "Realative url, start with /api", Required = true, ValidationPattern = "^/api/.*$", Name = "Url")]
        public string Url { get; set; }

        protected override async Task<ResponseResult> RunApiAsync(ApiManager apiManager, CancellationToken ct)
        {
            _ = Url ?? throw new ArgumentException("Url is required");
            var jsonString = string.IsNullOrEmpty(Json) ? "" : await File.ReadAllTextAsync(Json, ct);

            var result = string.IsNullOrEmpty(jsonString)
                ? await apiManager.RunHttpApiAsync(Url, Method, ct: ct).ConfigureAwait(false)
                : await apiManager.RunHttpApiAsync(Url, Method, jsonString, ct).ConfigureAwait(false);

            return result;
        }
    }
}

#nullable restore
