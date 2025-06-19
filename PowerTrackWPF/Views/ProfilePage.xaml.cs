using System.Windows;
using System.Windows.Controls;
using PowerTrackWPF.ViewModels;

namespace PowerTrackWPF
{
    /// <summary>
    /// Lógica de interacción para ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : UserControl
    {
        private readonly ProfileViewModel _viewModel;

        public ProfilePage()
        {
            InitializeComponent();
            _viewModel = new ProfileViewModel();
            DataContext = _viewModel;
        }

        private void PasswordBox_CurrentPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProfileViewModel vm)
                vm.SetCurrentPassword(((PasswordBox)sender).Password);
        }

        private void PasswordBox_NewPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProfileViewModel vm)
                vm.SetNewPassword(((PasswordBox)sender).Password);
        }

        private void PasswordBox_ConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProfileViewModel vm)
                vm.SetConfirmPassword(((PasswordBox)sender).Password);
        }
    }
}
