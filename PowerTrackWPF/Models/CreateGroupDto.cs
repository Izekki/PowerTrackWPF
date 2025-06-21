using System.Text.Json.Serialization;

namespace PowerTrackWPF.Models
{
    public class CreateGroupDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("devices")]
        public int[] Devices { get; set; }

        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }
    }
}