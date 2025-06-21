using System.Text.Json.Serialization;

namespace PowerTrackWPF.Models
{
    public class EditDeviceDto
    {
        public string name { get; set; }
        public string ubicacion { get; set; }
        public int? id_grupo { get; set; }
        public string mac_address { get; set; }
    }
}