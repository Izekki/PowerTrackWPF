using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PowerTrackClienteWPF
{
    /// <summary>
    /// Lógica de interacción para RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window
    {
        private bool showPassword = false;
        private bool showConfirmPassword = false;
        private string DOMAIN_URL = Environment.GetEnvironmentVariable("VITE_BACKEND_URL") ?? "http://localhost:3000";

        public RegisterView()
        {
            InitializeComponent();
            LoadProveedores();

        }

        private async void LoadProveedores()
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync($"{DOMAIN_URL}/supplier");
                var json = await response.Content.ReadAsStringAsync();
                var proveedores = JsonSerializer.Deserialize<List<Proveedor>>(json);

                ProveedorComboBox.ItemsSource = proveedores;
                ProveedorComboBox.DisplayMemberPath = "nombre";
                ProveedorComboBox.SelectedValuePath = "id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener proveedores:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string nombre = NombreTextBox.Text;
            string correo = CorreoTextBox.Text;
            string contraseña = PasswordBox.Password;
            string confirmarContraseña = ConfirmPasswordBox.Password;
            var proveedor = ProveedorComboBox.SelectedValue;

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(contraseña) || string.IsNullOrWhiteSpace(confirmarContraseña) || proveedor == null)
            {
                MessageBox.Show("Todos los campos son obligatorios", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (contraseña != confirmarContraseña)
            {
                MessageBox.Show("Las contraseñas no coinciden", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var formData = new
            {
                nombre = nombre,
                correo = correo,
                contraseña = contraseña,
                confirmarContraseña = confirmarContraseña,
                proveedor = proveedor.ToString()
            };

            try
            {
                var json = JsonSerializer.Serialize(formData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var client = new HttpClient();
                var response = await client.PostAsync($"{DOMAIN_URL}/user/register", content);
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse>(responseBody);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Usuario registrado con éxito", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result?.message ?? "Error al registrarse", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Error de conexión con el servidor", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            showPassword = !showPassword;
            EyeIcon.Source = new BitmapImage(new Uri(showPassword ? "/Assets/eye-slash-icon.png" : "/Assets/eye-icon.png", UriKind.Relative));
            MessageBox.Show("WPF no permite cambiar dinámicamente entre PasswordBox y TextBox de forma sencilla.\nPuedes usar una librería externa o alternar manualmente si es necesario.", "Aviso");
        }

        private void ToggleConfirmPassword_Click(object sender, RoutedEventArgs e)
        {
            showConfirmPassword = !showConfirmPassword;
            ConfirmEyeIcon.Source = new BitmapImage(new Uri(showConfirmPassword ? "/Assets/eye-slash-icon.png" : "/Assets/eye-icon.png", UriKind.Relative));
            MessageBox.Show("No se implementa el cambio real de tipo por limitación en WPF.\nSe requiere un control personalizado si es necesario.", "Aviso");
        }

        private void GoBack_Click(object sender, MouseButtonEventArgs e)
        {
            var login = new LoginView();
            login.Show();
            this.Close();
        }

        public class Proveedor
        {
            public int id { get; set; }
            public string nombre { get; set; }
        }

        public class ApiResponse
        {
            public string message { get; set; }
        }
    }
}
