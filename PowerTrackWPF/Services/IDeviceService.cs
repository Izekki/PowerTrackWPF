using PowerTrackWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PowerTrackWPF.RegisterView;

namespace PowerTrackWPF.Services
{
    public interface IDeviceService
    {
        Task<List<Dispositivo>> GetDispositivosAsync(int userId);
        Task<bool> DeleteDispositivoAsync(int id, int userId);
        Task<ApiResponse<CreateDispositivoResponse>> CreateDispositivoAsync(object dispositivo);
        Task<ApiResponseForEditDevice> UpdateDispositivoAsync(int id, EditDeviceDto device);
        Task<ApiResponseForEditDevice> UpdateTipoDispositivoAsync(int dispositivoId, int tipoId);

    }
}
