using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace PowerTrackWPF.Models
{
    public class UpdateGroupDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("devices")]
        public int[] Devices { get; set; }

        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }
    }
}