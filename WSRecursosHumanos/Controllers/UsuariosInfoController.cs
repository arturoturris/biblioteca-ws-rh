using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WSRecursosHumanos.Models;
using WSRecursosHumanos.Utils;

namespace WSRecursosHumanos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosInfoController : Controller
    {
        private IFirebaseConfig config;
        private IFirebaseClient client;

        public UsuariosInfoController()
        {
            config = new FirebaseConfig
            {
                AuthSecret = "GlBIK78ZVCv03j1B49pp2zbvgIjbN07JXgOU9NNT",
                BasePath = "https://sw-tienda-online-default-rtdb.firebaseio.com/"
            };
            client = new FireSharp.FirebaseClient(config);
        }

        [HttpGet]
        public ActionResult GetUsuariosInfo()
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
                string jsonUsuarioInfo = FirebaseUtils.GetUsuariosInfo(client);
                response.code = 207;
                response.data = jsonUsuarioInfo;
            }

            response.status = response.code == 207 ? "success" : "error";
            response.message = FirebaseUtils.GetResponseMessage(client, response.code);

            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> SetUsuarioInfo([FromBody]object body)
        {
            String token = JwtUtils.getTokenFromAuthorizationHeader(Request.Headers["Authorization"]);
            WSResponse response = new WSResponse { code = 999, status = "error" };
            IDictionary<string, object> payload;
            JObject jo = JObject.Parse(body.ToString());
            String searchedUser = (string)jo.GetValue("searchedUser");
            String userInfoJSON = jo.GetValue("userInfoJSON").ToString();
            UsuarioInfo usuarioInfo;

            if ((payload = JwtUtils.decodeToken(token)) == null)
                response.code = 305;
            else if (!UsuarioInfoUtils.HasRecursosHumanosRol((string)payload["rol"]))
                response.code = 306;
            else if (FirebaseUtils.ExistsUsuarioInfo(client, searchedUser))
                response.code = 314;
            else if (!UsuarioInfoUtils.IsJsonWellFormed(userInfoJSON))
                response.code = 303;
            else if ((usuarioInfo=UsuarioInfoUtils.JsonToUsuarioInfo(userInfoJSON)) == null)
                response.code = 312;
            else
            {
                await FirebaseUtils.SetUsuarioInfo(client, searchedUser, usuarioInfo);
                response.code = 208;
            }

            response.message = FirebaseUtils.GetResponseMessage(client, response.code);
            response.status = response.code == 208 ? "success" : "error";

            return Json(response);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUsuarioInfo([FromBody] object body)
        {
            String token = JwtUtils.getTokenFromAuthorizationHeader(Request.Headers["Authorization"]);
            WSResponse response = new WSResponse { code = 999, status = "error" };
            IDictionary<string, object> payload;
            JObject jo = JObject.Parse(body.ToString());
            String searchedUser = (string)jo.GetValue("searchedUser");
            String userInfoJSON = jo.GetValue("userInfoJSON").ToString();
            UsuarioInfo usuarioInfo;

            if ((payload = JwtUtils.decodeToken(token)) == null)
                response.code = 305;
            else if (!UsuarioInfoUtils.HasRecursosHumanosRol((string)payload["rol"]))
                response.code = 306;
            else if (!FirebaseUtils.ExistsUsuarioInfo(client, searchedUser))
                response.code = 315;
            else if (!UsuarioInfoUtils.IsJsonWellFormed(userInfoJSON))
                response.code = 303;
            else if ((usuarioInfo = UsuarioInfoUtils.JsonToUsuarioInfo(userInfoJSON)) == null)
                response.code = 312;
            else
            {
                await FirebaseUtils.UpdateUsuarioInfo(client, searchedUser, usuarioInfo);
                response.code = 209;
            }

            response.message = FirebaseUtils.GetResponseMessage(client, response.code);
            response.status = response.code == 209 ? "success" : "error";

            return Json(response);
        }
    }
}
