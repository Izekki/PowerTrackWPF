using PowerTrackWPF.Commands;
using PowerTrackWPF.Helpers;
using PowerTrackWPF.Models;
using PowerTrackWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PowerTrackWPF.ViewModels
{
    public class CreateGroupViewModel : INotifyPropertyChanged
    {
        private readonly IGroupService _groupService;
        private readonly IDeviceService _deviceService;
        private readonly Window _window;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private string _groupName = string.Empty;
        public string GroupName
        {
            get => _groupName;
            set { _groupName = value; OnPropertyChanged(); }
        }

        public ObservableCollection<SelectableDevice> DevicesAvailable { get; set; } = new();
        public ObservableCollection<int> SelectedDeviceIds { get; set; } = new();

        public CreateGroupViewModel(IGroupService groupService, IDeviceService deviceService, Window window)
        {
            _groupService = groupService;
            _deviceService = deviceService;
            _window = window;

            LoadUnassignedDevicesAsync();

            SaveCommand = new RelayCommand(async _ => await SaveAsync());
            CancelCommand = new RelayCommand(_ => _window.Close());
        }

        private async Task LoadUnassignedDevicesAsync()
        {
            var devices = await _deviceService.GetDispositivosAsync(SessionManager.UserId);
            var unassigned = devices.Where(d => string.IsNullOrEmpty(d.GrupoNombre));

            DevicesAvailable.Clear();
            foreach (var d in unassigned)
                DevicesAvailable.Add(new SelectableDevice(d));
        }


        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(GroupName))
            {
                MessageBox.Show("El nombre del grupo es obligatorio.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedIds = DevicesAvailable
                .Where(d => d.IsSelected)
                .Select(d => d.Device.Id)
                .ToArray();

            if (selectedIds.Length == 0)
            {
                MessageBox.Show("Debes seleccionar al menos un dispositivo.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var grupo = new CreateGroupDto
            {
                Name = GroupName,
                Devices = selectedIds,
                UsuarioId = SessionManager.UserId
            };

            var result = await _groupService.CreateGrupoAsync(grupo);

            if (result.Success)
            {
                MessageBox.Show("Grupo creado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                _window.DialogResult = true;
                _window.Close();
            }
            else
            {
                MessageBox.Show(result.Message ?? "Error al crear el grupo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}