using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PowerTrackWPF.Models
{
    public class GroupDevicesResponseDto
    {
        [JsonPropertyName("inGroup")]
        public List<DispositivoDto> InGroup { get; set; } = new();

        [JsonPropertyName("outGroup")]
        public List<DispositivoDto> OutGroup { get; set; } = new();
    }

    public class DispositivoDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("dispositivo_nombre")]
        public string DispositivoNombre { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre
        {
            get => DispositivoNombre;
            set => DispositivoNombre = value;
        }

        [JsonPropertyName("ubicacion")]
        public string Ubicacion { get; set; }

        [JsonPropertyName("usuario_id")]
        public int UsuarioId { get; set; }

        [JsonPropertyName("id_grupo")]
        public int? IdGrupo { get; set; }

        [JsonPropertyName("id_tipo_dispositivo")]
        public int IdTipoDispositivo { get; set; }

        [JsonPropertyName("id_sensor")]
        public int IdSensor { get; set; }

        [JsonPropertyName("grupo_nombre")]
        public string GrupoNombre { get; set; }

        [JsonPropertyName("mac_address")]
        public string MacAddress { get; set; }
    }
}
