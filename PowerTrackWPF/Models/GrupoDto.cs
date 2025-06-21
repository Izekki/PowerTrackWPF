using System.Collections.Generic;
using System.Text.Json.Serialization;
using PowerTrackWPF.Models;

namespace PowerTrackWPF.Models
{
    public class GrupoDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Nombre { get; set; }

        [JsonPropertyName("devices")]
        public List<DispositivoSimpleDto> Dispositivos { get; set; } = new();
    }
}