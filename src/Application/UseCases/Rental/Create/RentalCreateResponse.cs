using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.UseCases.Rental.Create
{
    public class RentalCreateResponse
    {
        [JsonPropertyName("identificador")]
        public Guid Id { get; set; }
    }
}
