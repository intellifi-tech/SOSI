using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Org.Json;
using System.Json;
using SOSI.DataBasee;
using static SOSI.GirisKayit.GirisBaseActivity;

namespace SOSI.WebServicee
{
    class WebService
    {
        string kokurl = "http://46.45.185.15:8080/api/";
        public string ServisIslem(string url, string istekler,bool isLogin=false, string Method = "POST", string ContentType = "application/json")
        {
            RestSharp.Method GelenMethod = RestSharp.Method.POST;

            switch (Method)
            {
                case "POST":
                    GelenMethod = RestSharp.Method.POST;
                    break;
                case "PUT":
                    GelenMethod = RestSharp.Method.PUT;
                    break;
                case "DELETE":
                    GelenMethod = RestSharp.Method.DELETE;
                    break;
                default:
                    break;
            }

            var client = new RestSharp.RestClient(kokurl + url);
            client.Timeout = -1;
            var request = new RestSharp.RestRequest(GelenMethod);
            request.AddHeader("Content-Type", ContentType);
            if (!isLogin)
            {
                request.AddHeader("Authorization", "Bearer " + GetApiToken());
            }
            request.AddParameter(ContentType, istekler, RestSharp.ParameterType.RequestBody);
            RestSharp.IRestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.Unauthorized &&
                response.StatusCode != HttpStatusCode.InternalServerError &&
                response.StatusCode != HttpStatusCode.BadRequest &&
                response.StatusCode != HttpStatusCode.Forbidden &&
                response.StatusCode != HttpStatusCode.MethodNotAllowed &&
                response.StatusCode != HttpStatusCode.NotAcceptable &&
                response.StatusCode != HttpStatusCode.RequestTimeout &&
                response.StatusCode != HttpStatusCode.NotFound)
            {
                return response.Content;
            }
            else
            {
                return "Hata";
            }
        } 
        public string OkuGetir(string url,bool DontUseHostURL = false,bool isLogin=false)
        {

            RestSharp.RestClient client;
            if (DontUseHostURL)
            {
                client = new RestSharp.RestClient(url);
            }
            else
            {
                client = new RestSharp.RestClient(kokurl + url + "?size=100000");
            }
             
            client.Timeout = -1;
            var request = new RestSharp.RestRequest(RestSharp.Method.GET);
            request.AddHeader("Content-Type", "application/json");
            if (!isLogin)
            {
                request.AddHeader("Authorization", "Bearer " + GetApiToken());
            }
            RestSharp.IRestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.Unauthorized &&
                response.StatusCode != HttpStatusCode.InternalServerError &&
                response.StatusCode != HttpStatusCode.BadRequest &&
                response.StatusCode != HttpStatusCode.Forbidden &&
                response.StatusCode != HttpStatusCode.MethodNotAllowed &&
                response.StatusCode != HttpStatusCode.NotAcceptable &&
                response.StatusCode != HttpStatusCode.RequestTimeout &&
                response.StatusCode != HttpStatusCode.NotFound )
            {
                return response.Content;
            }
            else
            {
                return null;
            }
        }
    
        string GetApiToken()
        {
            if (!string.IsNullOrEmpty(APITOKEN.TOKEN))
            {
                return APITOKEN.TOKEN;
            }
            else
            {
                var a = DataBase.MEMBER_DATA_GETIR();
                if (a.Count>0)
                {
                    APITOKEN.TOKEN = a[0].API_TOKEN;
                    return a[0].API_TOKEN;
                }
                else
                {
                    return "";
                }
            }
        }
        void SetApiToken()
        {
            var MemberInfo = DataBase.MEMBER_DATA_GETIR();
            if (MemberInfo.Count > 0)
            {
                LoginRoot loginRoot = new LoginRoot()
                {
                    password = MemberInfo[0].password,
                    rememberMe = true,
                    username = MemberInfo[0].email
                };
                string jsonString = JsonConvert.SerializeObject(loginRoot);
                WebService webService = new WebService();
                var Donus = webService.ServisIslem("authenticate", jsonString);
                if (Donus != "Hata")
                {
                    JSONObject js = new JSONObject(Donus);
                    var Token = js.GetString("id_token");
                    if (Token != null && Token != "")
                    {
                        APITOKEN.TOKEN = Token;
                        MemberInfo[0].API_TOKEN = Token;
                        APITOKEN.TOKEN = Token;
                    }
                }
            }
            else
            {
                APITOKEN.TOKEN = "";
            }
        }
    }

    public static class APITOKEN
    {
        public static string TOKEN { get; set; } 
    }
    public static class CDN
    {
        public static string CDN_Path { get; set; } = "http://590333323.origin.radorecdn.net/";
    }
}