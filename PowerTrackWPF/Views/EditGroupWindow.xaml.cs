using PowerTrackWPF.Models;
using PowerTrackWPF.Services;
using PowerTrackWPF.ViewModels;
using System.Windows;

namespace PowerTrackWPF.Views
{
    public partial class EditGroupWindow : Window
    {
        public EditGroupWindow(Grupo grupo, IGroupService groupService)
        {
            InitializeComponent();
            var vm = new EditGroupViewModel(grupo, groupService, this);
            vm.EditCompleted += success =>
            {
                if (success)
                {
                    this.DialogResult = true;
                    this.Close();
                }
            };
            DataContext = vm;
        }
    }
}
