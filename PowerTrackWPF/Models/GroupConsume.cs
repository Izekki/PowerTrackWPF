using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrackWPF.Models
{
    public class GroupConsume : INotifyPropertyChanged
    {
        public int? GrupoId { get; set; }
        public string Nombre { get; set; } = "";
        public double ConsumoTotalKWh { get; set; }
        public ObservableCollection<DeviceConsume> Dispositivos { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}