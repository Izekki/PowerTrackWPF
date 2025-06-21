using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrackWPF.Models
{
    public class Dispositivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public string GrupoNombre { get; set; }
        public string MacAddress { get; set; }
        public int UsuarioId { get; set; }
        public int? GrupoId { get; set; }
        public int TipoDispositivoId { get; set; }
        public int SensorId { get; set; }

        public string TipoDispositivoNombre => TipoDispositivoId switch
        {
            0 => "Sin Imagen",
            1 => "Televisor",
            2 => "Computadora de Escritorio",
            3 => "Laptop",
            4 => "Aire Acondicionado",
            5 => "Refrigerador",
            6 => "Microondas",
            7 => "Consola de Videojuegos",
            8 => "Arrocera",
            9 => "Modem",
            10 => "Lavadora",
            11 => "Proyector",
            12 => "Impresora",
            13 => "Secadora de Pelo",
            14 => "Bocinas",
            15 => "Teléfono Fijo",
            16 => "Ventilador",
            17 => "Plancha",
            _ => "Desconocido"
        };


    }
}
