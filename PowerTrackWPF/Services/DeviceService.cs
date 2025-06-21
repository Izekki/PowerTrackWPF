using PowerTrackWPF.Helpers;
using PowerTrackWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PowerTrackWPF.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly HttpClient _client;
        private readonly string DOMAIN_URL = Config.GetBackendUrl();

        public DeviceService()
        {
            _client = new HttpClient();
        }

        public async Task<List<Dispositivo>> GetDispositivosAsync(int userId)
        {
            var response = await _client.GetAsync($"{DOMAIN_URL}/device/dispositivosPorUsuario/{userId}");
            if (!response.IsSuccessStatusCode) return new List<Dispositivo>();

            var content = await response.Content.ReadAsStringAsync();
            var dispositivosDto = JsonSerializer.Deserialize<List<DispositivoDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return dispositivosDto?.Select(d => new Dispositivo
            {
                Id = d.Id,
                Nombre = d.DispositivoNombre,
                Ubicacion = d.Ubicacion,
                GrupoNombre = d.GrupoNombre,
                MacAddress = d.MacAddress,
                GrupoId = d.IdGrupo,
                TipoDispositivoId = d.IdTipoDispositivo,
                SensorId = d.IdSensor
            }).ToList() ?? new List<Dispositivo>();
        }

        public async Task<bool> DeleteDispositivoAsync(int id, int userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{DOMAIN_URL}/device/deleteDevice/{id}")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new { usuarioId = userId }),
                    Encoding.UTF8,
                    "application/json"
                )
            };

            var response = await _client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<ApiResponse<CreateDispositivoResponse>> CreateDispositivoAsync(object dispositivo)
        {
            var json = JsonSerializer.Serialize(dispositivo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{DOMAIN_URL}/device/devices", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var resultData = JsonSerializer.Deserialize<CreateDispositivoResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return new ApiResponse<CreateDispositivoResponse>
                    {
                        Success = true,
                        Message = resultData?.Message ?? "Dispositivo creado exitosamente.",
                        Data = resultData
                    };
                }
                catch
                {
                    return new ApiResponse<CreateDispositivoResponse>
                    {
                        Success = true,
                        Message = "Dispositivo creado, pero no se pudo leer la respuesta detallada.",
                        Data = null
                    };
                }
            }
            else
            {
                string errorMessage = "Error desconocido del servidor.";
                try
                {
                    var errorDict = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
                    if (errorDict != null && errorDict.ContainsKey("message"))
                        errorMessage = errorDict["message"];
                }
                catch { }

                return new ApiResponse<CreateDispositivoResponse>
                {
                    Success = false,
                    Message = errorMessage,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseForEditDevice> UpdateDispositivoAsync(int id, EditDeviceDto device)
        {
            var json = JsonSerializer.Serialize(device);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{DOMAIN_URL}/device/editar/{id}", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                var updatedDevice = JsonSerializer.Deserialize<EditedDeviceResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new ApiResponseForEditDevice
                {
                    Success = true,
                    Message = "Dispositivo actualizado correctamente.",
                    Data = updatedDevice
                };
            }
            catch
            {
                return new ApiResponseForEditDevice
                {
                    Success = false,
                    Message = "Error al interpretar la respuesta del servidor.",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseForEditDevice> UpdateTipoDispositivoAsync(int id, int tipoId)
        {
            var json = JsonSerializer.Serialize(new { id_tipo_dispositivo = tipoId });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{DOMAIN_URL}/device/editar/icono/{id}", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    // Intenta extraer el mensaje como string plano desde el JSON
                    var jsonDoc = JsonDocument.Parse(responseBody);
                    var message = jsonDoc.RootElement.GetProperty("message").GetString();

                    return new ApiResponseForEditDevice
                    {
                        Success = true,
                        Message = message ?? "Tipo de dispositivo actualizado correctamente.",
                        Data = null
                    };
                }
                catch
                {
                    return new ApiResponseForEditDevice
                    {
                        Success = true,
                        Message = "Tipo de dispositivo actualizado correctamente (sin mensaje del servidor).",
                        Data = null
                    };
                }
            }
            else
            {
                return new ApiResponseForEditDevice
                {
                    Success = false,
                    Message = "Error al actualizar tipo de dispositivo.",
                    Data = null
                };
            }
        }
    }
}
