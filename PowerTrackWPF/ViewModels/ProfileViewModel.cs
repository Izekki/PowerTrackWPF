using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PowerTrackWPF.Commands;
using PowerTrackWPF.Models;
using PowerTrackWPF.Services;
using PowerTrackWPF.Helpers;

namespace PowerTrackWPF.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;

        public event Action<string> ProfileUpdated;

        public ICommand GuardarPerfilCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ToggleEditCommand { get; }

        // Campos ligados a UI
        private string _nombre;
        private string _correo;
        private int _idProveedor;

        // Campos para contraseña (NO ligados directamente a UI con Bind, se pasan desde code-behind)
        private string CurrentPassword = "";
        private string NewPassword = "";
        private string ConfirmPassword = "";

        // Colecciones para listas en UI
        public ObservableCollection<Proveedor> Proveedores { get; set; } = new();
        public ObservableCollection<Dispositivo> Dispositivo { get; set; } = new();
        public ObservableCollection<Grupo> Grupo { get; set; } = new();

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); }
        }
        public string Correo
        {
            get => _correo;
            set { _correo = value; OnPropertyChanged(); }
        }
        public int IdProveedor
        {
            get => _idProveedor;
            set
            {
                if (_idProveedor != value)
                {
                    _idProveedor = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ProveedorSeleccionado));
                }
            }
        }

        public Proveedor ProveedorSeleccionado => Proveedores.FirstOrDefault(p => p.Id == IdProveedor);

        private bool _isEditing = false;
        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; OnPropertyChanged(); }
        }

        public ProfileViewModel()
        {
            _userService = new UserService();

            GuardarPerfilCommand = new RelayCommand(async _ => await GuardarTodoAsync());
            CancelCommand = new RelayCommand(async _ =>
            {
                await LoadUserData();
                IsEditing = false;
            });
            ToggleEditCommand = new RelayCommand(_ => IsEditing = !IsEditing);

            _ = LoadUserData();
        }

        private async Task LoadUserData()
        {
            try
            {
                var profile = await _userService.GetProfileAsync();
                if (profile != null)
                {
                    Nombre = profile.Nombre;
                    Correo = profile.Correo;

                    Proveedores.Clear();
                    foreach (var p in profile.ProveedoresDisponibles ?? new List<Proveedor>())
                        Proveedores.Add(p);

                    IdProveedor = profile.IdProveedor;
                    OnPropertyChanged(nameof(ProveedorSeleccionado));

                    Dispositivo.Clear();
                    foreach (var d in profile.Dispositivos ?? new List<Dispositivo>())
                        Dispositivo.Add(d);

                    Grupo.Clear();
                    foreach (var g in profile.Grupos ?? new List<Grupo>())
                        Grupo.Add(g);

                    // Limpiar campos de contraseña
                    CurrentPassword = NewPassword = ConfirmPassword = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar perfil: {ex.Message}");
            }
        }

        private async Task GuardarTodoAsync()
        {
            try
            {
                // Validar datos básicos
                if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Correo) || IdProveedor <= 0)
                {
                    MessageBox.Show("Nombre, correo y proveedor son obligatorios.");
                    return;
                }

                // 1) Actualizar perfil
                await _userService.UpdateProfileAsync(new ProfileUpdateDto
                {
                    Nombre = Nombre,
                    Correo = Correo,
                    IdProveedor = IdProveedor
                });

                // 2) Verificar si hay intento de cambiar contraseña
                bool cambioPwd = !string.IsNullOrWhiteSpace(CurrentPassword) ||
                                 !string.IsNullOrWhiteSpace(NewPassword) ||
                                 !string.IsNullOrWhiteSpace(ConfirmPassword);

                if (cambioPwd)
                {
                    if (string.IsNullOrWhiteSpace(CurrentPassword) || string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmPassword))
                    {
                        MessageBox.Show("Completa todos los campos de contraseña para cambiarla.");
                        return;
                    }
                    if (NewPassword != ConfirmPassword)
                    {
                        MessageBox.Show("Las nuevas contraseñas no coinciden.");
                        return;
                    }

                    await _userService.ChangePasswordAsync(CurrentPassword, NewPassword);
                    MessageBox.Show("Contraseña actualizada correctamente.");
                }

                // Recargar datos (para refrescar UI)
                await LoadUserData();

                ProfileUpdated?.Invoke(Nombre);
                IsEditing = false;

                MessageBox.Show("Perfil actualizado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar perfil: {ex.Message}");
            }
        }

        // Métodos para recibir valores de contraseña desde el code-behind (por seguridad)
        public void SetCurrentPassword(string pwd) => CurrentPassword = pwd;
        public void SetNewPassword(string pwd) => NewPassword = pwd;
        public void SetConfirmPassword(string pwd) => ConfirmPassword = pwd;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
