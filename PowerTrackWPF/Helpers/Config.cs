using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PowerTrackWPF.Helpers
{
    using System.Text.Json;

    public static class Config
    {
        public static string GetBackendUrl()
        {
            var json = File.ReadAllText("appsettings.json");
            var config = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            return config["BackendUrl"];
        }
    }

}
