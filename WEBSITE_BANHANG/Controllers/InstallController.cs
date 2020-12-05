using JWT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBSITE_BANHANG.Models;

namespace WEBSITE_BANHANG.Controllers
{
    public class InstallController : Controller
    {
        private static string URL_CallBack = "http://localhost:8008/install/login&nonce=abc123";
        private static string URL_CallBack_install = "http://localhost:8008/install/grandservice&nonce=abc123";
        private static string URL_clientId = "a28ee0b137bbf1bd5948a413583fa026";
        private static string URL_required_permission = "openid profile email org userinfo";
        // GET: Install
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(string orgid, string code, string id_token)
        {


            if (Session["expired"] != null)
            {
                if (DateTime.Parse(Session["expired"].ToString()) >= DateTime.Now)
                {
                   return  Redirect("/");
                }
                else
                {
                    // het 
                    Session["expired"] = null;
                    Session["id_token"] = null;
                    Session["orgid"] = null;
                    // xoa het thong tin 
                }
            }

            if (!string.IsNullOrWhiteSpace(orgid))
            {
                if(Session["orgid"] == null)
                {
                    Session["orgid"] = orgid;
                }
                return Redirect($"https://accounts.haravan.com/connect/authorize?response_mode=form_post&response_type=code id_token&scope={URL_required_permission}&client_id={URL_clientId}&redirect_uri={URL_CallBack}");
            }

            if (!string.IsNullOrWhiteSpace(id_token))
            {
                var model = new HaravanLoginModel();
                var obj = DeserializeObjectToken(id_token);
                model.id_token = id_token;
                if (obj["email"] != null)
                    model.email = obj["email"];
                if (obj["name"] != null)
                    model.name = obj["name"];

                Session["id_token"] = model.id_token;
                Session["name"] = model.name;
                Session["email"] = model.email;
                Session["expired"] = DateTime.Now.AddMinutes(1);

                return Redirect($"https://accounts.haravan.com/connect/authorize?response_mode=form_post&response_type=code id_token&scope=openid profile email org userinfo grant_service web.write_contents&client_id={URL_clientId}&redirect_uri={URL_CallBack_install}"); /// lay ac

            }
            return View();
        }

        public ActionResult GrandService(string id_token)
        {
            var context = HttpContext.Request;
            if (!string.IsNullOrWhiteSpace(id_token))
            {
                
                Session["access_token"] = id_token;
                Session["expired"] = DateTime.Now.AddMinutes(1);
                var obj = DeserializeObjectToken(id_token);
            }
            else
            {
                return Redirect("/error");
            }
            return Redirect("/");
        }

        private dynamic DeserializeObjectToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token).ToString();
            jsonToken = jsonToken.Replace(jsonToken.Substring(0, jsonToken.IndexOf("}.") + 2), "");
            var obj = JsonConvert.DeserializeObject<dynamic>(jsonToken);
            return obj;
        }



    }
}