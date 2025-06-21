using PowerTrackWPF.Helpers;
using PowerTrackWPF.Models;
using PowerTrackWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PowerTrackWPF.Services
{
    public class GroupService : IGroupService
    {
        private readonly HttpClient _client;
        private readonly string DOMAIN_URL = Config.GetBackendUrl();

        public GroupService()
        {
            _client = new HttpClient();
        }

        public async Task<List<Grupo>> GetGruposAsync(int userId)
        {
            var response = await _client.GetAsync($"{DOMAIN_URL}/groups/byUser/{userId}");
            if (!response.IsSuccessStatusCode) return new List<Grupo>();

            var content = await response.Content.ReadAsStringAsync();
            var gruposDto = JsonSerializer.Deserialize<List<GrupoDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return gruposDto?.Select(g => new Grupo
            {
                Id = g.Id,
                Nombre = g.Nombre,
                Dispositivos = new ObservableCollection<Dispositivo>(
                    g.Dispositivos.Select(d => new Dispositivo
                    {
                        Id = d.Id,
                        Nombre = d.Nombre,
                        Ubicacion = d.Ubicacion
                    }))
            }).ToList() ?? new List<Grupo>();
        }
        public async Task<bool> DeleteGrupoAsync(int id, int userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{DOMAIN_URL}/groups/deleteGroup/{id}")
            {
                Content = new StringContent(JsonSerializer.Serialize(new { usuarioId = userId }),
                                            Encoding.UTF8,
                                            "application/json")
            };

            var response = await _client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }


        public async Task<ApiResponse<CreateGrupoResponse>> CreateGrupoAsync(CreateGroupDto grupo)
        {
            var json = JsonSerializer.Serialize(grupo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{DOMAIN_URL}/groups/create", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var resultData = JsonSerializer.Deserialize<CreateGrupoResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return new ApiResponse<CreateGrupoResponse>
                    {
                        Success = true,
                        Message = resultData?.Message ?? "Grupo creado exitosamente.",
                        Data = resultData
                    };
                }
                catch
                {
                    return new ApiResponse<CreateGrupoResponse>
                    {
                        Success = true,
                        Message = "Grupo creado, pero no se pudo leer la respuesta detallada.",
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

                return new ApiResponse<CreateGrupoResponse>
                {
                    Success = false,
                    Message = errorMessage,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<(ObservableCollection<SelectableDevice> DevicesInGroup, ObservableCollection<SelectableDevice> DevicesOutGroup)>> GetGroupDevicesAsync(int userId, int groupId)
        {
            var response = await _client.GetAsync($"{DOMAIN_URL}/groups/grupo/{groupId}/dispositivos?usuarioId={userId}");
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<(ObservableCollection<SelectableDevice>, ObservableCollection<SelectableDevice>)>
                {
                    Success = false,
                    Message = "Error al cargar dispositivos del grupo",
                    Data = (new ObservableCollection<SelectableDevice>(), new ObservableCollection<SelectableDevice>())
                };
            }

            var result = JsonSerializer.Deserialize<GroupDevicesResponseDto>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null)
            {
                return new ApiResponse<(ObservableCollection<SelectableDevice>, ObservableCollection<SelectableDevice>)>
                {
                    Success = false,
                    Message = "Respuesta vacía del servidor",
                    Data = (new ObservableCollection<SelectableDevice>(), new ObservableCollection<SelectableDevice>())
                };
            }

            var devicesInGroup = new ObservableCollection<SelectableDevice>(
                result.InGroup.Select(d => new SelectableDevice(new Dispositivo
                {
                    Id = d.Id,
                    Nombre = d.DispositivoNombre,
                    Ubicacion = d.Ubicacion,
                    GrupoId = d.IdGrupo,
                    GrupoNombre = d.GrupoNombre,
                    MacAddress = d.MacAddress,
                    TipoDispositivoId = d.IdTipoDispositivo,
                    SensorId = d.IdSensor
                })
                { IsSelected = true }));

            var devicesOutGroup = new ObservableCollection<SelectableDevice>(
                result.OutGroup.Select(d => new SelectableDevice(new Dispositivo
                {
                    Id = d.Id,
                    Nombre = d.DispositivoNombre,
                    Ubicacion = d.Ubicacion,
                    GrupoId = d.IdGrupo,
                    GrupoNombre = d.GrupoNombre,
                    MacAddress = d.MacAddress,
                    TipoDispositivoId = d.IdTipoDispositivo,
                    SensorId = d.IdSensor
                })
                { IsSelected = false }));

            return new ApiResponse<(ObservableCollection<SelectableDevice>, ObservableCollection<SelectableDevice>)>
            {
                Success = true,
                Message = "Dispositivos cargados correctamente",
                Data = (devicesInGroup, devicesOutGroup)
            };
        }

        public async Task<ApiResponse<string>> UpdateGroupAsync(UpdateGroupDto group)
        {
            var json = JsonSerializer.Serialize(group);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{DOMAIN_URL}/groups/edit", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                var result = JsonSerializer.Deserialize<ApiResponse<string>>(
                    responseBody,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result ?? new ApiResponse<string>
                {
                    Success = false,
                    Message = "Respuesta inválida del servidor"
                };
            }
            catch
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "No se pudo deserializar la respuesta del servidor"
                };
            }
        }
    }
}
