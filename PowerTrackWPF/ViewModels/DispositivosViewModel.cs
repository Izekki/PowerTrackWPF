using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PowerTrackWPF.Models;




namespace PowerTrackWPF.ViewModels
{
    public class DispositivosViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Dispositivo> Dispositivos { get; set; } = new();
        public ObservableCollection<Grupo> Grupos { get; set; } = new();

        public DispositivosViewModel()
        {
            Dispositivos.Add(new Dispositivo { Nombre = "Sens 6 Rolero", Ubicacion = "Sala de control" });
            Grupos.Add(new Grupo
            {
                Nombre = "Grupo Dispositivos",
                Dispositivos = new ObservableCollection<Dispositivo>
                {
                    new Dispositivo { Nombre = "Dispositivo 5", Ubicacion = "Lobby"},
                    new Dispositivo { Nombre = "Gran pipe", Ubicacion = "fr"}
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
