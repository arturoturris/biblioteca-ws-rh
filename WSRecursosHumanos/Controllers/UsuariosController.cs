using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using WSRecursosHumanos.Models;
using WSRecursosHumanos.Utils;

namespace WSRecursosHumanos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : Controller
    {
        private IFirebaseConfig config;
        private IFirebaseClient client;

        public UsuariosController()
        {
            config = new FirebaseConfig
            {
                AuthSecret = "GlBIK78ZVCv03j1B49pp2zbvgIjbN07JXgOU9NNT",
                BasePath = "https://sw-tienda-online-default-rtdb.firebaseio.com/"
            };
            client = new FireSharp.FirebaseClient(config);
        }

        [HttpGet]
        public ActionResult GetUsuarios()
        {
            String token = JwtUtils.getTokenFromAuthorizationHeader(Request.Headers["Authorization"]);
            WSResponse response = new WSResponse { code = 999, status = "error" };
            IDictionary<string, object> payload;

            if ((payload = JwtUtils.decodeToken(token)) == null)
                response.code = 305;
            else if (!UsuarioInfoUtils.HasRecursosHumanosRol((string)payload["rol"]))
                response.code = 311;
            else
            {
                string jsonUsuarios = FirebaseUtils.GetUsuarios(client);
                response.code = 207;
                response.data = jsonUsuarios;
            }

            response.status = response.code == 207 ? "success" : "error";
            response.message = FirebaseUtils.GetResponseMessage(client, response.code);

            return Json(response);
        }

        [HttpPost]
        public ActionResult SetUsuario([FromBody] object body)
        {
            String token = JwtUtils.getTokenFromAuthorizationHeader(Request.Headers["Authorization"]);
            WSResponse response = new WSResponse { code = 999, status = "error" };
            IDictionary<string, object> payload;
            JObject jo = JObject.Parse(body.ToString());
            String newUser = (string)jo.GetValue("newUser");
            String newPass = (string)jo.GetValue("newPass");

            if ((payload = JwtUtils.decodeToken(token)) == null)
                response.code = 305;
            else if (!UsuarioInfoUtils.HasRecursosHumanosRol((string)payload["rol"]))
                response.code = 306;
            else if (FirebaseUtils.ExistsUsuario(client, newUser))
                response.code = 307;
            else
            {
                
                String hashedPass = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(newPass))).Replace("-", "").ToLower();
                FirebaseUtils.SetUsuario(client, newUser, hashedPass);
                response.code = 205;
            }

            response.message = FirebaseUtils.GetResponseMessage(client, response.code);
            response.status = response.code == 205 ? "success" : "error";

            return Json(response);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUsuarioInfo([FromBody] object body)
        {
            String token = JwtUtils.getTokenFromAuthorizationHeader(Request.Headers["Authorization"]);
            WSResponse response = new WSResponse { code = 999, status = "error" };
            IDictionary<string, object> payload;
            JObject jo = JObject.Parse(body.ToString());
            String oldUser = (string)jo.GetValue("oldUser");
            String newUser = (string)jo.GetValue("newUser");
            String newPass = (string)jo.GetValue("newPass");

            if ((payload = JwtUtils.decodeToken(token)) == null)
                response.code = 305;
            else if (!UsuarioInfoUtils.HasRecursosHumanosRol((string)payload["rol"]))
                response.code = 306;
            else if (!FirebaseUtils.ExistsUsuario(client, oldUser))
                response.code = 310;
            else if (FirebaseUtils.ExistsUsuario(client, newUser))
                response.code = 307;
            else if(!UsuarioUtils.IsUserSintaxCorrect(newUser))
                response.code = 309;
            else if (!UsuarioUtils.IsPasswordSintaxCorrect(newPass))
                response.code = 308;
            else
            {
                String hashedPass = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(newPass))).Replace("-", "").ToLower();
                String jsonUsuarioInfo = FirebaseUtils.GetUsuarioInfo(client, oldUser);
                UsuarioInfo usuarioInfo = UsuarioInfoUtils.JsonToUsuarioInfo(jsonUsuarioInfo);
                FirebaseUtils.SetUsuario(client, newUser, hashedPass);
                FirebaseUtils.DeleteUser(client, oldUser);
                await FirebaseUtils.SetUsuarioInfo(client, newUser, usuarioInfo);
                await FirebaseUtils.DeleteUserInfo(client, oldUser);
                response.code = 206;
            }

            response.message = FirebaseUtils.GetResponseMessage(client, response.code);
            response.status = response.code == 206 ? "success" : "error";

            return Json(response);
        }
    }
}
