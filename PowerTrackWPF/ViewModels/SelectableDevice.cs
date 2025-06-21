using PowerTrackWPF.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PowerTrackWPF.ViewModels
{
    public class SelectableDevice : INotifyPropertyChanged
    {
        public Dispositivo Device { get; set; }

        public string Nombre => Device?.Nombre ?? "Sin nombre";

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public SelectableDevice(Dispositivo dispositivo)
        {
            Device = dispositivo;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
