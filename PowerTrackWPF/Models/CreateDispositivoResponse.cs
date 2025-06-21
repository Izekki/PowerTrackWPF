using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrackWPF.Models
{
    public class CreateDispositivoResponse
    {
        public string Message { get; set; }
        public int Dispositivo_Id { get; set; }
        public int Sensor_Id { get; set; }
    }
}