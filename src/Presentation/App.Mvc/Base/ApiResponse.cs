using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

public class ApiResponse
{
    public object Data { get; private set; }

    public List<string> Errors { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public ApiResponse()
    {
        Errors = new List<string>();
    }

    public ApiResponse SetError(string error)
    {
        Errors.Add(error);
        return this;
    }

    public T GetData<T>()
        => (T)Convert.ChangeType(this.Data, typeof(T));

    public ApiResponse SetData<T>(T obj)
    {
        this.Data = (T)Convert.ChangeType(obj, typeof(T));
        return this;
    }

    public ApiResponse SetData(object obj)
    {
        this.Data = obj;
        return this;
    }

    public void SetStatusCode(HttpStatusCode status)
        => StatusCode = status;

    public async Task ResponseContext(HttpContext context)
    {
        string content = this.ToString();
        context.Response.StatusCode = (int)StatusCode;
        context.Response.ContentType = "application/json";
        context.Response.ContentLength = content.Length;
        await context.Response.WriteAsync(content);
    }

    public ApiResponse Exception(ResponseException exception)
    {
        StatusCode = exception.StatusCode;
        Errors.Add(exception.Message);
        return this;
    }

    public override string ToString()
        => JsonConvert.SerializeObject(this, JsonSerializer());

    public static implicit operator string(ApiResponse response)
        => JsonConvert.SerializeObject(response, JsonSerializer());

    private static JsonSerializerSettings JsonSerializer()
    {
        return new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new DefaultContractResolver
            {
                IgnoreIsSpecifiedMembers = true,
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };
    }

    public class ResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public ResponseException(string message) : base(message) { }

        public ResponseException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}