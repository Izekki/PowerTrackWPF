using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using PowerTrackWPF.Helpers;
using PowerTrackWPF.Models;
using PowerTrackWPF.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PowerTrackWPF.ViewModels
{
    public class ConsumoViewModel : INotifyPropertyChanged
    {
        private object _selectedItem;
        private ISeries[] _series;

        public ObservableCollection<DeviceConsume> Devices { get; set; } = new();
        public ObservableCollection<GroupConsume> Groups { get; set; } = new();

        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged();
                    UpdateChart();
                }
            }
        }

        public ISeries[] Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        private readonly IConsumeService _consumeService;

        public ConsumoViewModel(IConsumeService consumeService)
        {
            _consumeService = consumeService;
            LoadDataAsync(SessionManager.UserId);
        }

        private async void LoadDataAsync(int userId)
        {
            var data = await _consumeService.GetConsumoDataAsync(userId);

            Devices.Clear();
            foreach (var d in data.Dispositivos)
            {
                Devices.Add(new DeviceConsume
                {
                    Id = d.DispositivoId,
                    Nombre = d.Nombre,
                    GrupoId = d.GrupoId,
                    ConsumoActualKWh = d.ConsumoActualKWh,
                    CostoPorMedicionMXN = double.Parse(d.CostoPorMedicionMXN),
                    EstimacionCostoDiario = double.Parse(d.CostoDiarioMXN),
                    EstimacionCostoMensual = double.Parse(d.CostoMensualMXN),
                    CargoTarifas = new TarifaDetalle
                    {
                        CargoFijo = d.Tarifas.CargoFijo,
                        CargoVariable = d.Tarifas.CargoVariable,
                        CargoDistribucion = d.Tarifas.CargoDistribucion,
                        CargoCapacidad = d.Tarifas.CargoCapacidad
                    }
                });
            }

            Groups.Clear();
            foreach (var g in data.Grupos)
            {
                Groups.Add(new GroupConsume
                {
                    GrupoId = g.GrupoId,
                    Nombre = g.Nombre,
                    ConsumoTotalKWh = g.ConsumoTotalKWh,
                    Dispositivos = new ObservableCollection<DeviceConsume>(
                        g.Dispositivos.Select(d => new DeviceConsume
                        {
                            Id = d.DispositivoId,
                            Nombre = d.Nombre,
                            ConsumoActualKWh = d.ConsumoActualKWh,
                            GrupoId = d.GrupoId,
                            CostoPorMedicionMXN = double.Parse(d.CostoPorMedicionMXN),
                            EstimacionCostoDiario = double.Parse(d.CostoDiarioMXN),
                            EstimacionCostoMensual = double.Parse(d.CostoMensualMXN),
                            CargoTarifas = new TarifaDetalle
                            {
                                CargoFijo = d.Tarifas.CargoFijo,
                                CargoVariable = d.Tarifas.CargoVariable,
                                CargoDistribucion = d.Tarifas.CargoDistribucion,
                                CargoCapacidad = d.Tarifas.CargoCapacidad
                            }
                        }))
                });
            }

            if (Devices.Count > 0)
                SelectedItem = Devices[0];
            else if (Groups.Count > 0)
                SelectedItem = Groups[0];
        }

        private void UpdateChart()
        {
            const double scaleFactor = 1000; // Escala para valores pequeños

            if (SelectedItem is DeviceConsume device)
            {
                Series = new ISeries[]
                {
                    new PieSeries<ObservableValue>
                    {
                        Name = device.Nombre,
                        Values = new ObservableValue[] { new ObservableValue(device.ConsumoActualKWh * scaleFactor) }
                    }
                };
            }
            else if (SelectedItem is GroupConsume group)
            {
                Series = group.Dispositivos.Select(d => new PieSeries<ObservableValue>
                {
                    Name = d.Nombre,
                    Values = new ObservableValue[] { new ObservableValue(d.ConsumoActualKWh * scaleFactor) }
                }).ToArray();
            }
            else
            {
                Series = Devices.Select(d => new PieSeries<ObservableValue>
                {
                    Name = d.Nombre,
                    Values = new ObservableValue[] { new ObservableValue(d.ConsumoActualKWh * scaleFactor) }
                }).ToArray();
            }
        }



        // INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}