using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerTrackWPF.Models;

namespace PowerTrackWPF.Services
{
    public interface IConsumeService
    {
        Task<ConsumoResponseDto> GetConsumoDataAsync(int userId);
        Task<DeviceDetail> GetDeviceDetailsAsync(int deviceId);
    }
}