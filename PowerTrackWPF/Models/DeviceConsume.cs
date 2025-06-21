using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrackWPF.Models
{
    public class DeviceConsume : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int? GrupoId { get; set; }

        public string Nombre { get; set; } = "";
        public double ConsumoActualKWh { get; set; }
        public double CostoPorMedicionMXN { get; set; }
        public double EstimacionCostoDiario { get; set; }
        public double EstimacionCostoMensual { get; set; }
        public TarifaDetalle CargoTarifas { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class TarifaDetalle
    {
        public double CargoFijo { get; set; }
        public double CargoVariable { get; set; }
        public double CargoDistribucion { get; set; }
        public double CargoCapacidad { get; set; }
    }
}