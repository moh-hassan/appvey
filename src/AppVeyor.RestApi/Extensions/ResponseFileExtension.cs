// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api.Extensions;

internal static class FileExtension
{
    public static FileInfo MakeUnique(this FileInfo fileInfo)
    {
        var dir = fileInfo.DirectoryName;
        var fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
        var fileExt = fileInfo.Extension;

        for (var i = 1; ; ++i)
        {
            if (!fileInfo.Exists)
                return fileInfo;

            var newFileName = $"{fileName}-{i}{fileExt}";
            fileInfo = dir != null
                ? new FileInfo(Path.Combine(dir, newFileName))
                : new FileInfo(newFileName);
        }
    }

    public static string ReadFile(this FileInfo fileInfo)
    {
        if (!fileInfo.Exists)
        {
            throw new FileNotFoundException($"File not found: {fileInfo.FullName}");
        }

        return File.ReadAllText(fileInfo.FullName);
    }
}
