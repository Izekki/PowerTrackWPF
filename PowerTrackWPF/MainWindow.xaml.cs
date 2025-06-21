using PowerTrackWPF.Helpers;
using PowerTrackWPF.Views;
using System.Windows;
using System.Windows.Input;

namespace PowerTrackWPF
{
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty WelcomeMessageProperty =
            DependencyProperty.Register(
                "WelcomeMessage",
                typeof(string),
                typeof(MainWindow),
                new PropertyMetadata(string.Empty));

        public string WelcomeMessage
        {
            get => (string)GetValue(WelcomeMessageProperty);
            set => SetValue(WelcomeMessageProperty, value);
        }

        public MainWindow()
        {
            InitializeComponent();
            WelcomeMessage = $"Bienvenido, {SessionManager.Nombre}";
            MainContent.Content = new DispositivosPage();
        }

        private void Dispositivos_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DispositivosPage();
        }

        private void Consumo_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ConsumoPage();
        }

        private void Perfil_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ProfilePage();
        }

        private void LogoutImage_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("¿Estás seguro de que deseas cerrar sesión?", "Cerrar sesión", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                SessionManager.ClearSession();
                var login = new LoginView();
                login.Show();
                this.Close();
            }
        }

    }
}