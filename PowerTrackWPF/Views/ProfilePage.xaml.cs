using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PowerTrackClienteWPF.ViewModels;

namespace PowerTrackClienteWPF
{
    /// <summary>
    /// Lógica de interacción para ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Window
    {
        private readonly PorfileViewModel _viewModel;
        public ProfilePage()
        {
            InitializeComponent();
            _viewModel = new PorfileViewModel();
            DataContext = _viewModel;
        }

        private void PasswordBox_CurrentPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is PorfileViewModel vm)
                vm.SetCurrentPassword(((PasswordBox)sender).Password);
        }

        private void PasswordBox_NewPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is PorfileViewModel vm)
                vm.SetNewPassword(((PasswordBox)sender).Password);
        }

        private void PasswordBox_ConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is PorfileViewModel vm)
                vm.SetConfirmPassword(((PasswordBox)sender).Password);
        }

    }
}
