using System.Net;

namespace Web.Api.Middleware
{
    public class ProblemDetails
    {
        public string Type { get; set; } = "https://httpstatuses.io/500";
        public string Title { get; set; } = "An error occurred";
        public string Detail { get; set; } = string.Empty;
        public string Instance { get; set; } = string.Empty;
        public string TraceId { get; set; } = string.Empty;
        public int Status { get; set; }
        public Dictionary<string, object> Extensions { get; set; } = new();
    }

    public static class HttpProblemTypes
    {
        public const string ValidationError = "https://httpstatuses.io/400";
        public const string NotFound = "https://httpstatuses.io/404";
        public const string Conflict = "https://httpstatuses.io/409";
        public const string InternalServerError = "https://httpstatuses.io/500";
    }
}