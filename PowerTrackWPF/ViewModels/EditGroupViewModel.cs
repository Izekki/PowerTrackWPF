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
using System;

namespace PowerTrackWPF.ViewModels
{
    public class EditGroupViewModel : INotifyPropertyChanged
    {
        private readonly IGroupService _groupService;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private Grupo _grupo;
        public Grupo Grupo
        {
            get => _grupo;
            set
            {
                _grupo = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SelectableDevice> _devicesInGroup;
        public ObservableCollection<SelectableDevice> DevicesInGroup
        {
            get => _devicesInGroup;
            set
            {
                _devicesInGroup = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SelectableDevice> _devicesOutGroup;
        public ObservableCollection<SelectableDevice> DevicesOutGroup
        {
            get => _devicesOutGroup;
            set
            {
                _devicesOutGroup = value;
                OnPropertyChanged();
            }
        }

        private string _groupName;
        public string GroupName
        {
            get => _groupName;
            set
            {
                _groupName = value;
                OnPropertyChanged();
            }
        }

        private readonly Window _window;

        // Evento para notificar éxito al exterior
        public event Action<bool> EditCompleted;

        public EditGroupViewModel(Grupo grupo, IGroupService groupService, Window window)
        {
            Grupo = grupo;
            _groupService = groupService;
            _window = window;

            LoadDataAsync(grupo.Id, SessionManager.UserId);

            SaveCommand = new RelayCommand(async _ => await SaveAsync());
            CancelCommand = new RelayCommand(_ =>
            {
                _window.DialogResult = false;
                _window.Close();
            });
        }

        private async Task LoadDataAsync(int groupId, int userId)
        {
            var result = await _groupService.GetGroupDevicesAsync(userId, groupId);

            if (result.Success)
            {
                GroupName = Grupo.Nombre;
                DevicesInGroup = result.Data.DevicesInGroup;
                DevicesOutGroup = result.Data.DevicesOutGroup;
            }
            else
            {
                MessageBox.Show(result.Message ?? "Error al cargar dispositivos del grupo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(GroupName))
            {
                MessageBox.Show("El nombre del grupo no puede estar vacío.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selected = DevicesInGroup.Union(DevicesOutGroup)
                          .Where(d => d.IsSelected)
                          .Select(d => d.Device.Id)
                          .ToArray();

            var dto = new UpdateGroupDto
            {
                Id = Grupo.Id,
                Name = GroupName,
                Devices = selected,
                UsuarioId = SessionManager.UserId
            };

            var result = await _groupService.UpdateGroupAsync(dto);

            if (result != null && result.Success)
            {
                MessageBox.Show(result.Message ?? "Grupo actualizado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                // Actualizar localmente el nombre del grupo para reflejar cambio
                Grupo.Nombre = GroupName;
                OnPropertyChanged(nameof(Grupo));

                // Notificar éxito al exterior
                EditCompleted?.Invoke(true);

                _window.DialogResult = true;
                _window.Close();
            }
            else
            {
                MessageBox.Show(result?.Message ?? "Error al actualizar el grupo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
