// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Strings
{
    internal const string Build_start_recent = "Start build of branch most recent commit.\nExample 1:\n" +
        "\tappvey build start recent -s myproject --browse\n" +
        "Example 2 using environment variables with = separator:\n" +
        "\tappvey build start recent -a my-account -t my-token -s my-project --browse  var1=value1 var2=value2\n" +
        "Example 3 using file:\n" +
        "\tappvey build start recent -s myproject --browse @myenv.txt";

}

internal class CommandBase_Constant
{
}
