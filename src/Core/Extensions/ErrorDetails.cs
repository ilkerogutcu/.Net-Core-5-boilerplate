using Newtonsoft.Json;

namespace Core.Extensions
{
    public class ErrorDetails
    {
        public ErrorDetails(string message, int statusCode = 500)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public int StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }
    }
}