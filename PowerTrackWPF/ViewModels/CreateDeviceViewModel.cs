using PowerTrackWPF.Commands;
using PowerTrackWPF.Helpers;
using PowerTrackWPF.Models;
using PowerTrackWPF.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PowerTrackWPF.ViewModels
{
    public class CreateDeviceViewModel : INotifyPropertyChanged
    {
        private readonly IDeviceService _deviceService;
        private readonly IGroupService _groupService;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private string _nombre;
        private string _ubicacion;
        private string _macAddress;
        private int? _grupoId;

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); }
        }

        public string Ubicacion
        {
            get => _ubicacion;
            set { _ubicacion = value; OnPropertyChanged(); }
        }

        public string MacAddress
        {
            get => _macAddress;
            set { _macAddress = value; OnPropertyChanged(); }
        }

        public int? GrupoId
        {
            get => _grupoId;
            set { _grupoId = value; OnPropertyChanged(); }
        }

        private List<Grupo> _gruposDisponibles;
        public List<Grupo> GruposDisponibles
        {
            get => _gruposDisponibles;
            set { _gruposDisponibles = value; OnPropertyChanged(); }
        }

        private readonly Window _window;

        public CreateDeviceViewModel(IDeviceService deviceService, IGroupService groupService, Window window)
        {
            _deviceService = deviceService;
            _groupService = groupService;
            _window = window;

            LoadGruposAsync();

            // Limpiar los campos
            Nombre = string.Empty;
            Ubicacion = string.Empty;
            MacAddress = string.Empty;
            GrupoId = null;

            SaveCommand = new RelayCommand(async _ =>
            {
                if (!ValidateInputs())
                    return;

                var dispositivo = new
                {
                    nombre = Nombre,
                    ubicacion = Ubicacion,
                    usuario_id = SessionManager.UserId,
                    id_grupo = GrupoId,
                    mac = MacAddress.Trim()
                };

                var result = await _deviceService.CreateDispositivoAsync(dispositivo);
                Console.WriteLine($"Result.Success = {result.Success}");

                if (result.Success)
                {
                    MessageBox.Show(
                        result.Message ?? "Dispositivo creado correctamente.",
                        "Éxito",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                    Console.WriteLine("Exito - Cerrando ventana");
                    _window.DialogResult = true;
                    _window.Close();
                }
                else
                {
                    MessageBox.Show(
                        result.Message ?? "No se pudo crear el dispositivo.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    Console.WriteLine("cancel");

                }

                Console.WriteLine("Despues del succes");

            });
            CancelCommand = new RelayCommand(_ =>
            {
                _window.DialogResult = false;
                _window.Close();
            });
        }

        private async Task LoadGruposAsync()
        {
            var grupos = await _groupService.GetGruposAsync(SessionManager.UserId);
            GruposDisponibles = grupos.ToList();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(Nombre) ||
                string.IsNullOrWhiteSpace(Ubicacion) ||
                string.IsNullOrWhiteSpace(MacAddress))
            {
                MessageBox.Show("Nombre, ubicación y dirección MAC son obligatorios.");
                return false;
            }

            const string pattern = @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$";
            if (!Regex.IsMatch(MacAddress.Trim(), pattern))
            {
                MessageBox.Show("Formato de dirección MAC inválido.\nUsa: 00:00:00:00:00:00");
                return false;
            }

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}