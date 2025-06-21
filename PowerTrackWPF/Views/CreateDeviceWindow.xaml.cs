using PowerTrackWPF.Services;
using PowerTrackWPF.ViewModels;
using System.Windows;

namespace PowerTrackWPF.Views
{
    public partial class CreateDeviceWindow : Window
    {
        public CreateDeviceWindow()
        {
            InitializeComponent();
        }
        public CreateDeviceWindow(IDeviceService deviceService, IGroupService groupService)
        {
            InitializeComponent();
            this.DataContext = new CreateDeviceViewModel(deviceService, groupService, this);
        }
    }
}