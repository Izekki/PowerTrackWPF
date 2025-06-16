namespace PowerTrackClienteWPF.Models
{
    public class ProfileDto
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public int IdProveedor { get; set; }
        public List<Proveedor> ProveedoresDisponibles { get; set; }
        public List<Dispositivo> Dispositivos { get; set; }
        public List<Grupo> Grupos { get; set; }
    }

    public class ProfileUpdateDto
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public int IdProveedor { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

}
