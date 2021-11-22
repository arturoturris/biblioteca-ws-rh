using FireSharp.Interfaces;
using FireSharp.Response;
using System.Threading.Tasks;
using WSRecursosHumanos.Models;

namespace WSRecursosHumanos.Utils
{
    public class FirebaseUtils
    {
        public static string GetResponseMessage(IFirebaseClient client, int code = 999)
        {
            FirebaseResponse response = client.Get("/respuestas/" + code.ToString());
            return response.Body;
        }

        public static bool ExistsUsuario(IFirebaseClient client, string usuario)
        {
            FirebaseResponse response = client.Get("usuarios/" + usuario);
            return response.Body != "null";
        }

        public static bool ExistsUsuarioInfo(IFirebaseClient client, string usuario)
        {
            FirebaseResponse response = client.Get("usuarios_info/" + usuario);
            return response.Body != "null";
        }

        public static string GetUsuarios(IFirebaseClient client)
        {
            FirebaseResponse response = client.Get("/usuarios");
            return response.Body;
        }

        public static string GetUsuariosInfo(IFirebaseClient client) {
            FirebaseResponse response = client.Get("/usuarios_info");
            return response.Body;
        }

        public static string GetUsuarioInfo(IFirebaseClient client, string usuario)
        {
            FirebaseResponse response = client.Get("/usuarios_info/" + usuario);
            return response.Body;
        }

        public static int SetUsuario(IFirebaseClient client, string user, string hashedPass)
        {
            FirebaseResponse firebaseResponse = client.Set("usuarios/" + user, hashedPass);
            return (int)firebaseResponse.StatusCode;
        }

        public static async Task<int> SetUsuarioInfo(IFirebaseClient client, string user, UsuarioInfo usuarioInfo)
        {
            FirebaseResponse firebaseResponse = await client.SetAsync("usuarios_info/" + user, usuarioInfo);
            return (int)firebaseResponse.StatusCode;
        }

        public static async Task<int> UpdateUsuarioInfo(IFirebaseClient client, string user, UsuarioInfo usuarioInfo)
        {
            FirebaseResponse firebaseResponse = await client.UpdateAsync("usuarios_info/" + user, usuarioInfo);
            return (int)firebaseResponse.StatusCode;
        }

        public static int DeleteUser(IFirebaseClient client, string user)
        {
            FirebaseResponse firebaseResponse = client.Delete("usuarios/" + user);
            return (int)firebaseResponse.StatusCode;
        }

        public static async Task<int> DeleteUserInfo(IFirebaseClient client, string user)
        {
            FirebaseResponse firebaseResponse = await client.DeleteAsync("usuarios_info/" + user);
            return (int)firebaseResponse.StatusCode;
        }
    }
}