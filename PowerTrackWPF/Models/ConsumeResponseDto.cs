using PowerTrackWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PowerTrackWPF.Models
{
    public class ResumenDispositivoDto
    {
        [JsonPropertyName("dispositivo_id")]
        public int DispositivoId { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("consumoActualKWh")]
        public double ConsumoActualKWh { get; set; }

        [JsonPropertyName("grupo_id")]
        public int? GrupoId { get; set; }

        [JsonPropertyName("detalleTarifas")]
        public TarifaDetalleDto Tarifas { get; set; }

        [JsonPropertyName("costoPorMedicionMXN")]
        public string CostoPorMedicionMXN { get; set; }

        [JsonPropertyName("costoDiarioMXN")]
        public string CostoDiarioMXN { get; set; }

        [JsonPropertyName("costoMensualMXN")]
        public string CostoMensualMXN { get; set; }
    }

    public class ResumenGrupoDto
    {
        [JsonPropertyName("grupo_id")]
        public int? GrupoId { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("consumoTotalKWh")]
        public double ConsumoTotalKWh { get; set; }

        [JsonPropertyName("costoMensualMXN")]
        public string CostoMensualMXN { get; set; }

        [JsonPropertyName("dispositivos")]
        public List<ResumenDispositivoDto> Dispositivos { get; set; } = new();
    }

    public class ConsumoResponseDto
    {
        [JsonPropertyName("resumenDispositivos")]
        public List<ResumenDispositivoDto> Dispositivos { get; set; } = new();

        [JsonPropertyName("resumenGrupos")]
        public List<ResumenGrupoDto> Grupos { get; set; } = new();
    }

    // Dto para tarifas
    public class TarifaDetalleDto
    {
        [JsonPropertyName("cargo_variable")]
        public double CargoVariable { get; set; }

        [JsonPropertyName("cargo_fijo")]
        public double CargoFijo { get; set; }

        [JsonPropertyName("cargo_distribucion")]
        public double CargoDistribucion { get; set; }

        [JsonPropertyName("cargo_capacidad")]
        public double CargoCapacidad { get; set; }
    }
}