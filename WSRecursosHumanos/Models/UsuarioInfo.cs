using Newtonsoft.Json.Linq;

namespace WSRecursosHumanos.Models
{
    public class UsuarioInfo
    {
        public string correo { get; set; }
        public string nombre { get; set; }
        public string rol { get; set; }
        public string telefono { get; set; }

        public UsuarioInfo() { }

        public UsuarioInfo(JObject usuarioInfo)
        {
            this.correo = usuarioInfo.GetValue("correo").ToString();
            this.nombre = usuarioInfo.GetValue("nombre").ToString();
            this.rol = usuarioInfo.GetValue("rol").ToString();
            this.telefono = usuarioInfo.GetValue("telefono").ToString();
        }
    }
}