using PowerTrackWPF.Services;
using PowerTrackWPF.ViewModels;
using System.Windows.Controls;

namespace PowerTrackWPF.Views
{
    public partial class DispositivosPage : UserControl
    {
        public DispositivosPage() : this(new DeviceService(), new GroupService()) { }

        public DispositivosPage(IDeviceService deviceService, IGroupService groupService)
        {
            InitializeComponent();
            DataContext = new DispositivosViewModel(deviceService, groupService);
        }


        private void AddNewDevice(object obj)
        {
            var createWindow = new CreateDeviceWindow(new DeviceService(), new GroupService());
            if (createWindow.ShowDialog() == true)
            {
                // Recargar datos si se guardó
                if (DataContext is DispositivosViewModel vm && vm.LoadCommand.CanExecute(null))
                {
                    vm.LoadCommand.Execute(null);
                }
            }
        }
    }
}
