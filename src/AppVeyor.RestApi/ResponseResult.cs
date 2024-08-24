// Copyright (c) Mohamed Hassan. All rights reserved. See License.md in the project root for license information.

namespace AppVeyor.Api;

using System.Net;
using Extensions;
using RestApi.Extensions;

public class ResponseResult
{
    public bool IsSuccess { get; set; }
    public HttpStatusCode? StatusCode { get; set; }
    public string? StatusCodeDescription { get; set; }
    public string ResponseString { get; set; } = string.Empty;
    public string? ContentType { get; set; }
    public string Error { get; private set; } = string.Empty;
    public bool IsJsonHeader => ContentType == "application/json";
    public string Request { get; private set; } = string.Empty;

    private ResponseResult()
    {
    }

    public static ResponseResult Default()
    {
        return new ResponseResult
        {
            IsSuccess = true,
            StatusCode = (HttpStatusCode?)200,
            StatusCodeDescription = "200 (OK)",
        };
    }

    public static async Task<ResponseResult> CreateAsync(HttpResponseMessage? response)
    {
        if (response == null)
        {
            return new ResponseResult
            {
                IsSuccess = false,
                StatusCode = null,
                StatusCodeDescription = "Undefined StatusCode",
                Error = "No response from server"
            };
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = new ResponseResult
        {
            IsSuccess = response.IsSuccessStatusCode,
            ContentType = response.Content.Headers.ContentType?.MediaType,
            StatusCode = response.StatusCode,
            StatusCodeDescription = $"{(int)response.StatusCode} ({response.StatusCode})",
            ResponseString = response.IsSuccessStatusCode ? content : string.Empty,
            Error = response.IsSuccessStatusCode ? string.Empty : content,
            Request = response.RequestMessage?.Method + " " + response.RequestMessage?.RequestUri?.AbsolutePath,
        };

        // check if the response is html and IsSuccess is true
        if (result.IsSuccess && result.ContentType == "text/html")
        {
            result.IsSuccess = false;
            result.Error = "HTML response. Invalid resource";
            result.StatusCodeDescription = "Invalid resource";
        }

        return result;
    }

    private string GetDescription(HttpStatusCode? statusCode)
    {
        if (statusCode == null) return "Undefined StatusCode";
        return $"{(int)statusCode} ({statusCode})";
    }

    public int ShowResult()
    {
        if (IsSuccess)
        {
            WriteSuccess($"Success Response. The StatusCode: {StatusCodeDescription}");
            return 0;
        }

        var errorMessage = $"Error: HTTP request failed with status code {StatusCodeDescription}";
        WriteError($"{errorMessage}\n {Error}");
        return 2;
    }

    public void SaveResponse(FileInfo file)
    {
        if (!IsSuccess) return;

        var fileName = file.FullName;
        if (file.Exists)
        {
            fileName = file.MakeUnique().FullName;
            Console.WriteLine($"File is existing. Renaming file to {fileName}.");
        }

        File.WriteAllText(fileName, ResponseString.JsonFormat());
        WriteSuccess($"Response saved to {fileName}");
    }

    public override string ToString()
    {
        var text = $"StatusCode: {StatusCodeDescription}\n";
        text += IsSuccess ? ResponseString! : Error;
        return text;
    }
}
