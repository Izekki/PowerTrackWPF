using System.Windows;

namespace PowerTrackWPF.Views
{
    public partial class DeleteConfirmModal : Window
    {
        public DeleteConfirmModal(string title, string message, string v)
        {
            InitializeComponent();

            TitleText.Text = title;
            MessageText.Text = message;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e) => DialogResult = true;

        private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}