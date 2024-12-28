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
}

#nullable restore
