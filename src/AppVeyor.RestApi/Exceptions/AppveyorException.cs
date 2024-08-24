// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Exceptions;

public class AppveyorException : Exception
{
    public AppveyorException()
    {
    }

    public AppveyorException(string message) : base(message)
    {
    }

    public AppveyorException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
