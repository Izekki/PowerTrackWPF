using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerTrackWPF.Models;
using PowerTrackWPF.Services;

namespace PowerTrackWPF.Services
{
    public interface IUserService
    {
        Task<ProfileDto> GetProfileAsync();
        Task<bool> UpdateProfileAsync(ProfileUpdateDto updateDto);
        Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
    }
}
