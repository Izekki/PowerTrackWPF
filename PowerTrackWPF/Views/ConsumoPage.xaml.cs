using PowerTrackWPF.ViewModels;
using PowerTrackWPF.Services;
using System.Windows.Controls;

namespace PowerTrackWPF.Views
{
    public partial class ConsumoPage : UserControl
    {
        public ConsumoViewModel ViewModel { get; set; }

        public ConsumoPage()
        {
            InitializeComponent();
            var service = new ConsumeService();
            ViewModel = new ConsumoViewModel(service);
            DataContext = ViewModel;
        }
    }
}