using PowerTrackWPF.Models;
using PowerTrackWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrackWPF.Services
{
    public interface IGroupService
    {
        Task<List<Grupo>> GetGruposAsync(int userId);
        Task<bool> DeleteGrupoAsync(int id, int userId);
        Task<ApiResponse<CreateGrupoResponse>> CreateGrupoAsync(CreateGroupDto grupo);
        Task<ApiResponse<(ObservableCollection<SelectableDevice> DevicesInGroup, ObservableCollection<SelectableDevice> DevicesOutGroup)>> GetGroupDevicesAsync(int userId, int groupId);
        Task<ApiResponse<string>> UpdateGroupAsync(UpdateGroupDto group);
    }
}
