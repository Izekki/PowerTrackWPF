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
using Microsoft.Win32;

namespace PowerTrackClienteWPF
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {

        private bool passwordVisible = false;
        private string DOMAIN_URL = Environment.GetEnvironmentVariable("VITE_BACKEND_URL") ?? "http://localhost:3000";

        public LoginView()
        {
            InitializeComponent();
        }

        private void TogglePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            passwordVisible = !passwordVisible;
            EyeIcon.Source = new BitmapImage(new Uri(passwordVisible ? "Assets/eye-slash-icon.png" : "Assets/eye-icon.png", UriKind.Relative));
            MessageBox.Show("Por simplicidad, no se implementó visibilidad de contraseña aquí. Usa PasswordBox y muestra el valor si necesario.");
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Todos los campos son obligatorios", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var formData = new { email, password };
            var json = JsonSerializer.Serialize(formData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            try
            {
                var response = await client.PostAsync($"{DOMAIN_URL}/login", content);
                var responseBody = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<LoginResponse>(responseBody);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Inicio de sesión exitoso", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Guardar en memoria si deseas
                    // Navegar a siguiente ventana
                }
                else
                {
                    MessageBox.Show(data?.message ?? "Error al iniciar sesión", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Error de conexión con el servidor", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoToRegister_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*var registerWindow = new RegisterView(); 
            registerWindow.Show();
            this.Close();*/
        }

        private void RecoverPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*var recoverWindow = new RecoverPasswordView(); 
            recoverWindow.Show();
            this.Close();*/
        }

        private class LoginResponse
        {
            public string message { get; set; }
            public string token { get; set; }
            public int userId { get; set; }
            public string nombre { get; set; }
        }

    }
}
