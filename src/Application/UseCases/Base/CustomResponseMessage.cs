using System.Text.Json.Serialization;

namespace Application.UseCases.Base
{
    public class CustomResponseMessage
    {        
        [JsonPropertyName("mensagem")]
        public string Message { get; set; }

        public static List<CustomResponseMessage> FromMessage(string message)
        {
            return new List<CustomResponseMessage>
            {
                new CustomResponseMessage 
                {
                    Message = message
                }
            };
        }
    }
}
