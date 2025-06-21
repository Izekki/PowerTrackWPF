using PowerTrackWPF.Models;
using PowerTrackWPF.Services;
using PowerTrackWPF.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace PowerTrackWPF.ViewModels
{
    public class EditDeviceViewModel : INotifyPropertyChanged
    {
        private readonly IDeviceService _deviceService; // campo privado
        private readonly Window _window;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private Dispositivo _device;
        public Dispositivo Device
        {
            get => _device;
            set
            {
                _device = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TipoDispositivoModel> DeviceTypes { get; }

        public EditDeviceViewModel(Dispositivo device, IDeviceService deviceService, Window window)
        {
            Device = device;
            _deviceService = deviceService;
            _window = window;

            SaveCommand = new RelayCommand(async _ => await SaveAsync());
            CancelCommand = new RelayCommand(_ =>
            {
                _window.DialogResult = false;
                _window.Close();
            });

            DeviceTypes = new ObservableCollection<TipoDispositivoModel>
            {
                new() { Id = 0, Nombre = "Sin Imagen" },
                new() { Id = 1, Nombre = "Televisor" },
                new() { Id = 2, Nombre = "Computadora de Escritorio" },
                new() { Id = 3, Nombre = "Laptop" },
                new() { Id = 4, Nombre = "Aire Acondicionado" },
                new() { Id = 5, Nombre = "Refrigerador" },
                new() { Id = 6, Nombre = "Microondas" },
                new() { Id = 7, Nombre = "Consola de Videojuegos" },
                new() { Id = 8, Nombre = "Arrocera" },
                new() { Id = 9, Nombre = "Modem" },
                new() { Id = 10, Nombre = "Lavadora" },
                new() { Id = 11, Nombre = "Proyector" },
                new() { Id = 12, Nombre = "Impresora" },
                new() { Id = 13, Nombre = "Secadora de Pelo" },
                new() { Id = 14, Nombre = "Bocinas" },
                new() { Id = 15, Nombre = "Teléfono Fijo" },
                new() { Id = 16, Nombre = "Ventilador" },
                new() { Id = 17, Nombre = "Plancha" }
            };
        }

        private async Task SaveAsync()
        {
            if (!ValidateInputs()) return;

            var dto = new EditDeviceDto
            {
                name = Device.Nombre?.Trim(),
                ubicacion = Device.Ubicacion?.Trim(),
                id_grupo = Device.GrupoId,
                mac_address = string.IsNullOrWhiteSpace(Device.MacAddress) ? null : Device.MacAddress.Trim()
            };

            var result = await _deviceService.UpdateDispositivoAsync(Device.Id, dto);

            if (result != null && result.Success)
            {
                MessageBox.Show(result.Message ?? "Dispositivo actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                _window.DialogResult = true;
                _window.Close();
            }
            else
            {
                MessageBox.Show(result?.Message ?? "Error al actualizar el dispositivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(Device.Nombre) ||
                string.IsNullOrWhiteSpace(Device.Ubicacion) ||
                string.IsNullOrWhiteSpace(Device.MacAddress))
            {
                MessageBox.Show("Nombre, ubicación y dirección MAC son obligatorios.");
                return false;
            }

            const string pattern = @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$";
            if (!Regex.IsMatch(Device.MacAddress.Trim(), pattern))
            {
                MessageBox.Show("Formato de dirección MAC inválido.\nUsa: 00:00:00:00:00:00");
                return false;
            }

            return true;
        }

        public async Task<ApiResponseForEditDevice> UpdateTipoDispositivoAsync(int tipoId)
        {
            return await _deviceService.UpdateTipoDispositivoAsync(Device.Id, tipoId);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
