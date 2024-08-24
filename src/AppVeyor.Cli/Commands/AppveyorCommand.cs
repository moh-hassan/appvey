// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;

#nullable disable

[CliCommand(Description = "Appvey cli for Appveyor CI/CD")]
public class AppveyorCommand
{
    [CliCommand(Description = "Project command", Name = "project")]
    public class ProjectCommand
    {
        //project update ...
        [CliCommand(Description = "Update with more nested commands...", Name = "update")]
        public class UpdateCommand
        {
        }

        //[CliCommand(Description = "Delete with more nested commands...", Name = "delete")]
        //public class ProjectDeleteCommand
        //{
        //}
    }


    [CliCommand(Description = "Build with more nested commands...", Name = "build")]
    public class BuildCommand
    {
        [CliCommand(Description = "Start build with more nested commands...", Name = "start")]
        public class StartCommand
        {
        }

        [CliCommand(Description = "Download with more nested commands...", Name = "download")]
        public class DownloadCommand
        {
        }
    }

    //[CliCommand(Description = "Download command", Name = "download")]
    //public class DownloadCommand
    //{
    //    ////project update ...
    //    //[CliCommand(Description = "Download with more nested commands...", Name = "update")]
    //    //public class UpdateCommand
    //    //{
    //    //}
    //}
}

#nullable restore
