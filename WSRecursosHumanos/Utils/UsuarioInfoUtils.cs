using Newtonsoft.Json.Linq;
using System;
using WSRecursosHumanos.Models;

namespace WSRecursosHumanos.Utils
{
    public class UsuarioInfoUtils
    {
        public static bool HasRecursosHumanosRol(string rol)
        {
            return rol == "rh";
        }

        public static UsuarioInfo JsonToUsuarioInfo(string json) {
            try
            {
                JObject p = JObject.Parse(json);
                if (p.Property("nombre") != null &&
                    p.Property("telefono") != null &&
                    p.Property("correo") != null &&
                    p.Property("rol") != null) {
                    return new UsuarioInfo(p);
                }
                
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool IsJsonWellFormed(string json) {
            try
            {
                JObject p = JObject.Parse(json);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}