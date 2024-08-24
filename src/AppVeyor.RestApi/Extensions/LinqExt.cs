// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Extensions;

using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public static class LinqExt
{
    public static IEnumerable<JObject> ConvertToJObjects(this IEnumerable<object> objects)
    {
        return objects.Select(JObject.FromObject);
    }

    public static string[] AppendArgs(this string[] first, string[] second)
    {
        return first.Concat(second).ToArray();
    }
}
