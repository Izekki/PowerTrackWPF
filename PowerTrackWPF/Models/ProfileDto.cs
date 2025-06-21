using PowerTrackWPF.Models;
using System.Text.Json.Serialization;


namespace PowerTrackWPF.Models
{
    public class ProfileDto
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }

        [JsonPropertyName("id_proveedor")]
        public int IdProveedor { get; set; }

        [JsonPropertyName("proveedores_disponibles")]
        public List<Proveedor> ProveedoresDisponibles { get; set; }
        public List<Dispositivo> Dispositivos { get; set; }
        public List<Grupo> Grupos { get; set; }
    }

    public class ProfileUpdateDto
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("correo")]
        public string Correo { get; set; }

        [JsonPropertyName("id_proveedor")]
        public int IdProveedor { get; set; }

        [JsonPropertyName("currentPassword")]
        public string CurrentPassword { get; set; }

        [JsonPropertyName("newPassword")]
        public string NewPassword { get; set; }
    }

}
