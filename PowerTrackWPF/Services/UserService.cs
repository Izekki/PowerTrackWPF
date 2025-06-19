using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerTrackWPF.Models;
using PowerTrackWPF.Services;


namespace PowerTrackWPF.Services
{
    class UserService : IUserService
    {
        public async Task<ProfileDto> GetProfileAsync()
        {
            // Simulación de datos. Reemplaza con llamada HTTP real o lectura de BD.
            await Task.Delay(500);
            return new ProfileDto
            {
                Nombre = "Usuario de prueba",
                Correo = "correo@ejemplo.com",
                IdProveedor = 1,
                ProveedoresDisponibles = new()
                {
                    new Proveedor { Id = 1, Nombre = "CFE" },
                    new Proveedor { Id = 2, Nombre = "IUSA" }
                },
                Dispositivos = new()
                {
                    new Dispositivo { Id = 1, Nombre = "SmartPlug 01" }
                },
                Grupos = new()
                {
                    new Grupo { Id = 1, Nombre = "Sala" }
                }
            };
        }

        public async Task<bool> UpdateProfileAsync(ProfileUpdateDto updateDto)
        {
            // Simula éxito al actualizar. Cambia por lógica real.
            await Task.Delay(300);
            return true;
        }
    }
}
