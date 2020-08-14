using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using Newtonsoft.Json;
using SOSI.AppIntro;
using SOSI.DataBasee;
using SOSI.GenericClass;
using SOSI.GirisKayit;
using SOSI.IsletmeProfiliOlustur;
using SOSI.MainPage;
using SOSI.TamamlanmisSablonlar.SablonDetay;
using SOSI.WebServicee;

namespace SOSI.Splashh
{
    [Activity(Label = "Contento",MainLauncher =true)]
    public class Splash : Android.Support.V7.App.AppCompatActivity
    {
        string OpenSablonDetayID;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            new DataBase();
            SetContentView(Resource.Layout.Splash);
            
            OpenSablonDetayID =  Intent.GetStringExtra("sablonID");
        }
        protected override void OnResume()
        {
            base.OnResume();
          

            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }
        async void SimulateStartup()
        {
            await Task.Delay(2000);
            HazirlikYap();
        }
        void HazirlikYap()
        {
            //OrnekCalismaTestDummy();
            SetDumyData();
            this.RunOnUiThread(delegate ()
            {
                var Kullanici = DataBase.MEMBER_DATA_GETIR();

                if (Kullanici.Count > 0)
                {
                    var SirketBilgileri = DataBase.COMPANY_INFORMATION_GETIR();
                    if (SirketBilgileri.Count>0)
                    {
                        
                        StartActivity(typeof(MainPageBaseActivity));//AppIntroBaseActivity
                        this.Finish();
                    }
                    else
                    {
                        //SetDumyData();
                        StartActivity(typeof(IsletmeProfiliBaseActivity));//IsletmeProfiliBaseActivity
                        this.Finish();
                    }
                    
                }
                else
                {
                    //StartActivity(typeof(AnaMenuBaseActivitty));
                    //return;
                    StartActivity(typeof(AppIntroBaseActivity)); //AppIntroBaseActivity
                    this.Finish();
                }
                
            });
            
        }

        void OrnekCalismaTestDummy()
        {
            var icon = BitmapFactory.DecodeResource(Resources, Resource.Mipmap.aaestro);
            var ms = new MemoryStream();
            icon.Compress(Bitmap.CompressFormat.Png, 0, ms);
            byte[] mediabyte = ms.ToArray();

            var MeID = DataBase.MEMBER_DATA_GETIR()[0];
            
            var client = new RestSharp.RestClient("http://31.169.67.210:8080/api/examples");
            client.Timeout = -1;
            var request = new RestSharp.RestRequest(RestSharp.Method.POST);
           // request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36");
            request.AddHeader("Accept", "*/*");
            //request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Authorization", "Bearer " + MeID.API_TOKEN);
            request.AddParameter("text", "ddddddddd");
            request.AddFile("afterImagePath", mediabyte, "sosi_media_file.png");
            request.AddFile("beforeImagePath", mediabyte, "sosi_media_file1.png");
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
                var jsonobj = response.Content;
                //var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<MediaUploadDTO>(jsonobj.ToString());
                //if (Icerik != null)
                //{
                //    return Icerik.beforeMediaPath;
                //}
                //else
                //{
                //    return "";
                //}
            }
            else
            {
                //return "";
            }
        }
        public class OrnekCalisma
        {
            public string afterImagePath { get; set; }
            public string beforeImagePath { get; set; }
            public string id { get; set; }
            public string text { get; set; }
        }
        void SetDumyData()
        {
            return;
            for (int i = 0; i < 20; i++)
            {
                RootObject rootObject = new RootObject()
                {
                    name = "Hizmet Alanı " + (i+1).ToString(),
                    sectorId = "5e91da68be077700010095d9"
                };
                WebService webService = new WebService();
                string jsonString = JsonConvert.SerializeObject(rootObject);
                var Donus = webService.ServisIslem("service-areas", jsonString);
            }
            
        }
        public class RootObject
        {
            public string id { get; set; }
            public string name { get; set; }
            public string sectorId { get; set; }
        }
    }
    
    
}