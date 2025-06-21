using PowerTrackWPF.Helpers;
using PowerTrackWPF.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PowerTrackWPF.Services
{
    public class ConsumeService : IConsumeService
    {
        private readonly HttpClient _client = new();
        private readonly string DOMAIN_URL = Config.GetBackendUrl();

        public async Task<ConsumoResponseDto> GetConsumoDataAsync(int userId)
        {
            var response = await _client.GetAsync($"{DOMAIN_URL}/electrical_analysis/consumoPorDispositivosGrupos/{userId}");
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ConsumoResponseDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<DeviceDetail> GetDeviceDetailsAsync(int deviceId)
        {
            var response = await _client.GetAsync($"{DOMAIN_URL}/electrical_analysis/dispositivo/{deviceId}/consumo-detallado");
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<DeviceDetail>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}