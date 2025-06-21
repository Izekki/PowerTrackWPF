using PowerTrackWPF.Helpers;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PowerTrackWPF
{
    public partial class LoginView : Window
    {
        private bool passwordVisible = false;
        private readonly string DOMAIN_URL = Config.GetBackendUrl();

        public LoginView()
        {
            InitializeComponent();
        }

        private void TogglePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            passwordVisible = !passwordVisible;
            MessageBox.Show("Por simplicidad, la visibilidad de la contraseña no fue implementada.");
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
                    MessageBox.Show($"Bienvenido, {data.nombre}", "Inicio exitoso", MessageBoxButton.OK, MessageBoxImage.Information);

                    SessionManager.SetSession(data.token, data.userId, data.nombre);

                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(data?.message ?? "Credenciales incorrectas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión con el servidor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoToRegister_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var registerWindow = new RegisterView();
            registerWindow.Show();
            this.Close();
        }

        private void RecoverPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://powertrack.up.railway.app/",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo abrir el navegador: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
