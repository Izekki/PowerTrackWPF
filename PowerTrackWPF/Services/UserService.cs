using PowerTrackWPF.Helpers;
using PowerTrackWPF.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PowerTrackWPF.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
        private readonly string DOMAIN_URL = Config.GetBackendUrl();

        public UserService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SessionManager.Token);
        }

        public async Task<ProfileDto> GetProfileAsync()
        {
            var response = await _client.GetAsync($"{DOMAIN_URL}/user/show/{SessionManager.UserId}");
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ApiResponse<ProfileDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result?.Success != true)
                throw new Exception(result?.Message ?? "Error al obtener el perfil");

            return result.Data;
        }

        public async Task<bool> UpdateProfileAsync(ProfileUpdateDto updateDto)
        {
            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Console.WriteLine("Payload enviado:");
            Console.WriteLine(json); // Opcional para debug

            var response = await _client.PutAsync($"{DOMAIN_URL}/user/edit/{SessionManager.UserId}", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine("StatusCode: " + response.StatusCode);
            Console.WriteLine("Response body: " + responseBody);

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<ApiResponse<object>>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                throw new Exception(error?.Message ?? "Error al actualizar perfil");
            }

            return true;
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            var json = JsonSerializer.Serialize(new
            {
                currentPassword,
                newPassword
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{DOMAIN_URL}/user/{SessionManager.UserId}/change-password", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<ApiResponse<object>>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                throw new Exception(error?.Message ?? "Error al cambiar contraseña");
            }

            return true;
        }
    }
}
