using PowerTrackWPF.Commands;
using PowerTrackWPF.Helpers;
using PowerTrackWPF.Models;
using PowerTrackWPF.Services;
using PowerTrackWPF.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PowerTrackWPF.ViewModels
{
    public class DispositivosViewModel : INotifyPropertyChanged
    {
        private readonly IDeviceService _deviceService;
        private readonly IGroupService _groupService;

        public ObservableCollection<Dispositivo> Dispositivos { get; set; } = new();
        public ObservableCollection<Grupo> Grupos { get; set; } = new();

        public ICommand LoadCommand { get; }
        public ICommand AddDeviceCommand { get; }
        public ICommand AddGroupCommand { get; }
        public ICommand EditDeviceCommand { get; }
        public ICommand EditGroupCommand { get; }
        public ICommand DeleteDeviceCommand { get; }
        public ICommand DeleteGroupCommand { get; }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
                FilterItems();
            }
        }

        public DispositivosViewModel(IDeviceService deviceService, IGroupService groupService)
        {
            _deviceService = deviceService;
            _groupService = groupService;

            LoadCommand = new RelayCommand(async _ => await LoadData());
            AddDeviceCommand = new RelayCommand(AddNewDevice);
            AddGroupCommand = new RelayCommand(AddNewGroup);
            EditDeviceCommand = new RelayCommand(EditDevice);
            EditGroupCommand = new RelayCommand(EditGroup);
            DeleteDeviceCommand = new RelayCommand(DeleteDevice);
            DeleteGroupCommand = new RelayCommand(DeleteGroup);

            _ = LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                var allDevices = await _deviceService.GetDispositivosAsync(SessionManager.UserId);
                var allGroups = await _groupService.GetGruposAsync(SessionManager.UserId);

                Grupos.Clear();
                foreach (var g in allGroups)
                    Grupos.Add(g);

                Dispositivos.Clear();
                foreach (var d in allDevices.Where(d => string.IsNullOrEmpty(d.GrupoNombre)))
                    Dispositivos.Add(d);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar datos: {ex.Message}");
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
        }

        private void FilterItems()
        {
            // Implementar filtro si se desea
        }

        private void AddNewDevice(object obj)
        {
            var createWindow = new CreateDeviceWindow(_deviceService, _groupService);
            var result = createWindow.ShowDialog();

            if (result == true)
                _ = LoadData();
        }

        private void AddNewGroup(object obj)
        {
            var createWindow = new CreateGroupWindow(_groupService, _deviceService);
            var result = createWindow.ShowDialog();
            if (result == true)
                _ = LoadData();
        }

        private void EditDevice(object obj)
        {
            if (obj is Dispositivo dispositivo)
            {
                var editWindow = new EditDeviceWindow(dispositivo, _deviceService);
                var result = editWindow.ShowDialog();
                if (result == true)
                    _ = LoadData();
            }
        }

        private void EditGroup(object obj)
        {
            if (obj is Grupo grupo)
            {
                var editWindow = new EditGroupWindow(grupo, _groupService);

                // Importante: el VM se crea en la ventana, no aquí para no duplicar carga
                var result = editWindow.ShowDialog();

                if (result == true)
                    _ = LoadData();
            }
        }

        private async void DeleteGroup(object obj)
        {
            if (obj is Grupo grupo)
            {
                var confirm = new DeleteConfirmModal(
                    "Eliminar Grupo",
                    $"¿Estás seguro de que quieres eliminar el grupo \"{grupo.Nombre}\"? Esta acción no se puede deshacer.",
                    "danger");

                if (confirm.ShowDialog() == true)
                {
                    bool deleted = await _groupService.DeleteGrupoAsync(grupo.Id, SessionManager.UserId);
                    if (deleted)
                        Grupos.Remove(grupo);

                    await LoadData();
                }
            }
        }

        private async void DeleteDevice(object obj)
        {
            if (obj is Dispositivo dispositivo)
            {
                var confirm = new DeleteConfirmModal(
                    "Eliminar Dispositivo",
                    $"¿Estás seguro de que quieres eliminar el dispositivo \"{dispositivo.Nombre}\"?",
                    "danger");

                if (confirm.ShowDialog() == true)
                {
                    bool deleted = await _deviceService.DeleteDispositivoAsync(dispositivo.Id, SessionManager.UserId);
                    if (deleted)
                        Dispositivos.Remove(dispositivo);

                    await LoadData();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
