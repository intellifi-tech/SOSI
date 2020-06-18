using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Android;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Newtonsoft.Json;
using SOSI.DataBasee;
using SOSI.WebServicee;

namespace SOSI.MediaUploader
{
    [Service]
    class MediaUploaderService : Service
    {
        MEMBER_DATA MeId;
        GUNCEL_SABLON GuncelSablon;
        public override void OnCreate()
        {
            base.OnCreate();
            MeId = DataBase.MEMBER_DATA_GETIR()[0];
            SablonKontrol();
            if (GuncelSablon!=null)
            {
                UploadMedias();
            }
        }
        void UploadMedias()
        {
            var YuklenecekMedialar = DataBase.YUKLENECEK_SABLON_GETIR();
            if (YuklenecekMedialar.Count > 0)
            {
                var YuklenmeyenVarmi = YuklenecekMedialar.FindAll(item => item.isUploaded == false);
                if (YuklenmeyenVarmi.Count > 0)
                {
                    for (int i = 0; i < YuklenecekMedialar.Count; i++)
                    {
                        if (!YuklenecekMedialar[i].isUploaded)
                        {
            
                            //Android.Net.Uri newUri;
                            //newUri = Android.Net.Uri.Parse(YuklenecekMedialar[i].MediaUri);

                            MediaUploadDTO mediaUploadDTO = new MediaUploadDTO()
                            {
                                mediaCount = YuklenecekMedialar[i].maxMediaCount,
                                postText = YuklenecekMedialar[i].aciklama,
                                templateId = DataBase.GUNCEL_SABLON_GETIR()[0].id,
                                video = YuklenecekMedialar[i].isVideo,
                                userId = MeId.id,
                                processed = false,
                                type="POST"
                            };
                            var bytess = ConvertImageToByte(YuklenecekMedialar[i].MediaUri);
                            UploadWebService(mediaUploadDTO, bytess, YuklenecekMedialar[i]);
                        }
                    }

                    var YuklenecekMedialar2 = DataBase.YUKLENECEK_SABLON_GETIR();
                    var YuklenmeyenVarmi2 = YuklenecekMedialar2.FindAll(item => item.isUploaded == false);
                    if (YuklenmeyenVarmi2.Count <= 0)
                    {
                        DataBase.YUKLENECEK_SABLON_TEMIZLE();
                        DataBase.GUNCEL_SABLON_TEMIZLE();
                    }
                }
                else
                {
                    DataBase.GUNCEL_SABLON_TEMIZLE();
                }
            }
            else
            {
                DataBase.GUNCEL_SABLON_TEMIZLE();
            }
        }
        public byte[] ConvertImageToByte(string path)
        {
            var byteArray = File.ReadAllBytes(path);
            return byteArray;
        }
        void SablonKontrol()
        {
            var UygulanacakSablonuGetir = DataBase.GUNCEL_SABLON_GETIR();
            if (UygulanacakSablonuGetir.Count > 0)
            {
                GuncelSablon = UygulanacakSablonuGetir[UygulanacakSablonuGetir.Count - 1];
            }
            else
            {
                WebService webService = new WebService();
                GUNCEL_SABLON YeniSablon = new GUNCEL_SABLON() {
                    complete = false,
                    mediaCount = GetMediaCount(),
                    userId = MeId.id
                };
                string jsonString = JsonConvert.SerializeObject(YeniSablon);
                var Donus = webService.ServisIslem("templates", jsonString);
                if (Donus != "Hata")
                {
                    var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<GUNCEL_SABLON>(Donus.ToString());
                    if (Icerik!=null)
                    {
                        DataBase.GUNCEL_SABLON_TEMIZLE();
                        DataBase.GUNCEL_SABLON_EKLE(Icerik);
                        var UygulanacakSablonuGetir2 = DataBase.GUNCEL_SABLON_GETIR();
                        if (UygulanacakSablonuGetir2.Count > 0)
                        {
                            GuncelSablon = UygulanacakSablonuGetir2[UygulanacakSablonuGetir2.Count - 1];
                        }
                    }
                }
            }
        }

        int GetMediaCount()
        {
            var HerhangiBirPaylasim = DataBase.YUKLENECEK_SABLON_GETIR();
            if (HerhangiBirPaylasim.Count > 0)
            {
                return HerhangiBirPaylasim[HerhangiBirPaylasim.Count - 1].maxMediaCount;
            }
            else
            {
                return 0;
            }
        }

        void UploadWebService(MediaUploadDTO MediaUploadDTO1,byte[] mediabyte, YUKLENECEK_SABLON GuncellenecekSablon)
        {
            string jsonString = JsonConvert.SerializeObject(MediaUploadDTO1);
            string uzanti = ".png";
            if (MediaUploadDTO1.video)
            {
                uzanti = ".mp4";
            }
            var client = new RestSharp.RestClient("http://31.169.67.210:8080/api/template-medias");
            client.Timeout = -1;
            var request = new RestSharp.RestRequest(RestSharp.Method.POST);
            //request.Accept = "*/*";
            //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36";
            //client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36";
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Authorization", "Bearer " + MeId.API_TOKEN);
            request.AddParameter("mediaCount", MediaUploadDTO1.mediaCount);
            request.AddParameter("postText", MediaUploadDTO1.postText);
            request.AddParameter("templateId", MediaUploadDTO1.templateId);
            request.AddParameter("video", MediaUploadDTO1.video);
            request.AddParameter("userId", MeId.id);
            request.AddParameter("processed", false);
            request.AddParameter("type", "POST");
            request.AddFile("photo", mediabyte, "sosi_media_file"+ uzanti);
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
                GuncellenecekSablon.isUploaded = true;
                DataBase.YUKLENECEK_SABLON_Guncelle(GuncellenecekSablon);
            }
        }

        public class MediaUploadDTO
        {
            public string afterMediaPath { get; set; }
            public string beforeMediaPath { get; set; }
            public string id { get; set; }
            public int mediaCount { get; set; }
            public string postText { get; set; }
            public bool processed { get; set; }
            public string shareDateTime { get; set; }
            public string templateId { get; set; }
            public string type { get; set; }
            public string userId { get; set; }
            public bool video { get; set; }
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

    }
}