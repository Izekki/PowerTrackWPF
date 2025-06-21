using System.Text.Json.Serialization;

namespace PowerTrackWPF.Models
{
    public class DispositivoSimpleDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("ubicacion")]
        public string Ubicacion { get; set; }
    }
}
