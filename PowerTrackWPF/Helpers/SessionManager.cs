using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrackWPF.Helpers
{
    public static class SessionManager
    {
        public static string Token { get; private set; }
        public static int UserId { get; private set; }
        public static string Nombre { get; private set; }

        public static void SetSession(string token, int userId, string nombre)
        {
            Token = token;
            UserId = userId;
            Nombre = nombre;
        }

        public static void ClearSession()
        {
            Token = null;
            UserId = 0;
            Nombre = null;
        }

        public static bool IsAuthenticated => !string.IsNullOrEmpty(Token);
    }
}
