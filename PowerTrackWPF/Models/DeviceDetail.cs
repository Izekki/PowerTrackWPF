using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerTrackWPF.Models;

namespace PowerTrackWPF.Models
{
    public class DeviceDetail
    {
        public string FechaMedicion { get; set; }
        public double PotenciaW { get; set; }
        public double ConsumoActualKWh { get; set; }
        public double CostoPorMedicion { get; set; }
        public double EstimacionCostoDiario { get; set; }
        public double EstimacionCostoMensual { get; set; }
        public string Unidad { get; set; } = "kWh";
        public string Proveedor { get; set; } = "CFE";
        public TarifaDetalleDto DetalleTarifas { get; set; }
    }

}
