using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PowerTrackWPF.Commands;
using PowerTrackWPF.Models;
using PowerTrackWPF.Services;

namespace PowerTrackWPF.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private string _nombre;
        private string _correo;
        private int _idProveedor;
        private string CurrentPassword, NewPassword, ConfirmPassword;

        public string Nombre { get => _nombre; set { _nombre = value; OnPropertyChanged(); } }
        public string Correo { get => _correo; set { _correo = value; OnPropertyChanged(); } }
        public int IdProveedor { get => _idProveedor; set { _idProveedor = value; OnPropertyChanged(); } }

        public ObservableCollection<Proveedor> Proveedor { get; set; } = new();
        public ObservableCollection<Dispositivo> Dispositivo { get; set; } = new();
        public ObservableCollection<Grupo> Grupo { get; set; } = new();

        public ProfileViewModel()
        {
            _userService = new UserService();
            SaveCommand = new RelayCommand(async _ => await SaveAsync());
            CancelCommand = new RelayCommand(async _ => await LoadUserData());
            _ = LoadUserData(); // fire-and-forget, aunque idealmente se manejaría el await
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
                    IdProveedor = profile.IdProveedor;

                    Proveedor.Clear();
                    foreach (var prov in profile.ProveedoresDisponibles)
                        Proveedor.Add(prov);

                    Dispositivo.Clear();
                    foreach (var d in profile.Dispositivos)
                        Dispositivo.Add(d);

                    Grupo.Clear();
                    foreach (var g in profile.Grupos)
                        Grupo.Add(g);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar perfil: {ex.Message}");
            }
        }

        public async Task SaveAsync()
        {
            if (!string.IsNullOrEmpty(CurrentPassword) && NewPassword != ConfirmPassword)
            {
                MessageBox.Show("Las contraseñas no coinciden");
                return;
            }

            try
            {
                var result = await _userService.UpdateProfileAsync(new ProfileUpdateDto
                {
                    Nombre = Nombre,
                    Correo = Correo,
                    IdProveedor = IdProveedor,
                    CurrentPassword = CurrentPassword,
                    NewPassword = NewPassword
                });

                MessageBox.Show(result ? "Perfil actualizado" : "Error al actualizar el perfil");
                await LoadUserData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar perfil: {ex.Message}");
            }
        }

        public void SetCurrentPassword(string pwd) => CurrentPassword = pwd;
        public void SetNewPassword(string pwd) => NewPassword = pwd;
        public void SetConfirmPassword(string pwd) => ConfirmPassword = pwd;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

