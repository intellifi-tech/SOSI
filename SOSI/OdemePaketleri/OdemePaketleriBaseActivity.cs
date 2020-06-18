using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Model.V2;
using Iyzipay.Model.V2.Subscription;
using Iyzipay.Request.V2.Subscription;
using Newtonsoft.Json;
using SOSI.GenericClass;
using SOSI.WebServicee;
//using SOSI.IyziPayHelper;

namespace SOSI.OdemePaketleri
{
    [Activity(Label = "Contento",MainLauncher =false)]
    public class OdemePaketleriBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        protected Options options;
        LinearLayout Paket1, Paket2, Paket3;
        ImageButton Geri;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OdemePaketleriBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            Paket1 = FindViewById<LinearLayout>(Resource.Id.paket1hazne);
            Paket2 = FindViewById<LinearLayout>(Resource.Id.paket2hazne);
            Paket3 = FindViewById<LinearLayout>(Resource.Id.paket3hazne);
            Geri = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            Geri.Click += Geri_Click;
            //PaketleriOlustur();
            Paket1.Click += Paket1_Click;
            Paket2.Click += Paket2_Click;
            Paket3.Click += Paket3_Click;

        }

        private void Geri_Click(object sender, EventArgs e)
        {
            this.Finish();
        }

        private void Paket3_Click(object sender, EventArgs e)
        {
            OdemePaketleriBaseActivity_Helper.OdemePaketleriBaseActivity1 = this;
            OdemePaketleriBaseActivity_Helper.PricingPlanReferenceCode = "e98e298b-756f-488a-912f-493f07f40273";
            OdemePaketleriBaseActivity_Helper.PackageName = "PLATINUM";
            StartActivity(typeof(OdemeFormBaseActivity));

        }

        private void Paket2_Click(object sender, EventArgs e)
        {
            OdemePaketleriBaseActivity_Helper.OdemePaketleriBaseActivity1 = this;
            OdemePaketleriBaseActivity_Helper.PricingPlanReferenceCode = "8fa202ba-3ed3-4856-ae62-0282182d28d2";
            OdemePaketleriBaseActivity_Helper.PackageName = "GOLD";
            StartActivity(typeof(OdemeFormBaseActivity));
        }

        private void Paket1_Click(object sender, EventArgs e)
        {
            OdemePaketleriBaseActivity_Helper.OdemePaketleriBaseActivity1 = this;
            OdemePaketleriBaseActivity_Helper.PricingPlanReferenceCode = "adb65336-ae25-40bf-b579-c9f20529bec6";
            OdemePaketleriBaseActivity_Helper.PackageName = "SILVER";
            StartActivity(typeof(OdemeFormBaseActivity));

        }

        #region DummyData
    
        void PaketleriOlustur()
        {
            PaketDTOHazirla();
            WebService webService = new WebService();
            for (int i = 0; i < PaketOlusturUzakDBDTO1.Count; i++)
            {
                string jsonString = JsonConvert.SerializeObject(PaketOlusturUzakDBDTO1[i]);
                webService.ServisIslem("package-tariffs", jsonString);
            }
            string aaaa = "";
        }
        List<PaketOlusturUzakDBDTO> PaketOlusturUzakDBDTO1 = new List<PaketOlusturUzakDBDTO>();
        void PaketDTOHazirla()
        {
            PaketOlusturUzakDBDTO1.Add(new PaketOlusturUzakDBDTO() { 
            
              mediaCount = 10,
              mountCount=1,
              name="SILVER",
              price=499,
              reviseCount=10000,
            });
            PaketOlusturUzakDBDTO1.Add(new PaketOlusturUzakDBDTO()
            {

                mediaCount = 15,
                mountCount = 1,
                name = "GOLD",
                price = 699,
                reviseCount = 10000,
            });
            PaketOlusturUzakDBDTO1.Add(new PaketOlusturUzakDBDTO()
            {

                mediaCount = 20,
                mountCount = 1,
                name = "PLATINUM",
                price = 899,
                reviseCount = 10000,
            });
        }
        #endregion

        public static class OdemePaketleriBaseActivity_Helper
        {
            public static string PricingPlanReferenceCode { get; set; }
            public static OdemePaketleriBaseActivity OdemePaketleriBaseActivity1 { get; set; }
            public static string PackageName { get; set; }
        }

        public class PaketOlusturUzakDBDTO
        {
            public string id { get; set; }
            public int mediaCount { get; set; }
            public int mountCount { get; set; }
            public string name { get; set; }
            public int price { get; set; }
            public int reviseCount { get; set; }
        }
    }
}