using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerTrackClienteWPF.Models;

namespace PowerTrackClienteWPF.Services
{
    public interface IUserService
    {
        Task<ProfileDto> GetProfileAsync();
        Task<bool> UpdateProfileAsync(ProfileUpdateDto updateDto);
    }
}
