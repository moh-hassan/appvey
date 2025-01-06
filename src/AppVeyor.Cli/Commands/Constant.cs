// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Cli.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Constant
{
    internal const string Build_start_recent = "Start build of branch most recent commit.\nExample 1:\n appvey build start recent -s myproject --browse\nExample 2 using environment variables with = separator:\nappvey build start recent -s myproject --browse  var1=value1 var2=value2\nExample 3 using file:\nappvey build start recent -s myproject --browse @myenv.txt";

}

internal class CommandBase_Constant
{
}
