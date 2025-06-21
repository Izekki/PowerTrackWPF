using PowerTrackWPF.Models;
using PowerTrackWPF.Services;
using PowerTrackWPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PowerTrackWPF.Views
{
    public partial class EditDeviceWindow : Window
    {
        private int _previousSelectedTipoId;

        public EditDeviceWindow()
        {
            InitializeComponent();
        }

        public EditDeviceWindow(Dispositivo dispositivo, IDeviceService deviceService)
        {
            InitializeComponent();
            DataContext = new EditDeviceViewModel(dispositivo, deviceService, this);
            _previousSelectedTipoId = ((EditDeviceViewModel)DataContext).Device.TipoDispositivoId;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is EditDeviceViewModel viewModel)
            {
                Dispatcher.BeginInvoke(new Action(async () =>
                {
                    var newId = viewModel.Device.TipoDispositivoId;

                    if (newId != _previousSelectedTipoId)
                    {
                        var result = await viewModel.UpdateTipoDispositivoAsync(newId);
                        if (result?.Success == true)
                        {
                            MessageBox.Show("¡Genial! El tipo de dispositivo ha sido actualizado correctamente.",
                                            "Actualización completada",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Information);
                            _previousSelectedTipoId = newId;
                        }
                        else
                        {
                            MessageBox.Show(result?.Message ?? "Ocurrió un error al actualizar el tipo de dispositivo.",
                                            "Error al actualizar",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                        }
                    }
                }));
            }
        }
    }
}
