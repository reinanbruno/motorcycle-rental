using System.Net;
using System.Text.Json.Serialization;

namespace Application.UseCases.Base
{
    public class CustomResponse
    {
        [JsonIgnore]
        public HttpStatusCode HttpStatusCode { get; init; }

        [JsonIgnore]
        public bool Success { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Response { get; init; }
    }
}
