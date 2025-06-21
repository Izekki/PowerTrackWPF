using PowerTrackWPF.Services;
using PowerTrackWPF.ViewModels;
using System.Windows;

namespace PowerTrackWPF.Views
{
    public partial class CreateGroupWindow : Window
    {
        public CreateGroupWindow()
        {
            InitializeComponent();
        }
        public CreateGroupWindow(IGroupService groupService, IDeviceService deviceService)
        {
            InitializeComponent();
            this.DataContext = new CreateGroupViewModel(groupService, deviceService, this);
            
        }
    }
}
