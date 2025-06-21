using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace PowerTrackWPF.Models
{
    public class EditedDeviceResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("ubicacion")]
        public string Ubicacion { get; set; }

        [JsonPropertyName("usuario_id")]
        public int UsuarioId { get; set; }

        [JsonPropertyName("id_grupo")]
        public int? IdGrupo { get; set; }

        [JsonPropertyName("id_sensor")]
        public int IdSensor { get; set; }

        [JsonPropertyName("id_tipo_dispositivo")]
        public int IdTipoDispositivo { get; set; }
    }
}
