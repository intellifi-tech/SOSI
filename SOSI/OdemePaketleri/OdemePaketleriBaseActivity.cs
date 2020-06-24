using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Model.V2;
using Iyzipay.Model.V2.Subscription;
using Iyzipay.Request.V2.Subscription;
using Newtonsoft.Json;
using SOSI.DataBasee;
using SOSI.GenericClass;
using SOSI.WebServicee;
using static SOSI.MainPage.MainPageBaseActivity;
//using SOSI.IyziPayHelper;

namespace SOSI.OdemePaketleri
{
    [Activity(Label = "Contento",MainLauncher =false)]
    public class OdemePaketleriBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        protected Options options;
        LinearLayout Paket1, Paket2, Paket3;
        ImageButton Geri;

        TextView MevcutAlinmisPaket;
        Button PaketIptal;
        LinearLayout MevcutPaketHanze;
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

            MevcutAlinmisPaket = FindViewById<TextView>(Resource.Id.textView6);
            PaketIptal = FindViewById<Button>(Resource.Id.button1);
            MevcutPaketHanze = FindViewById<LinearLayout>(Resource.Id.linearLayout3);
            PaketIptal.Click += PaketIptal_Click;


            MevcutPaketHanze.Visibility = ViewStates.Gone;

            Geri.Click += Geri_Click;
            //PaketleriOlustur();
            Paket1.Click += Paket1_Click;
            Paket2.Click += Paket2_Click;
            Paket3.Click += Paket3_Click;

        }

        private void PaketIptal_Click(object sender, EventArgs e)
        {
            var cevap = new AlertDialog.Builder(this);
            cevap.SetCancelable(true);
            cevap.SetIcon(Resource.Mipmap.ic_launcher);
            cevap.SetTitle(Spannla(Color.Black, "Contento"));
            cevap.SetMessage(Spannla(Color.DarkGray, "Aktif aboneliğinizi iptal etmek istediğinizden emin misiniz?"));
            cevap.SetPositiveButton("Evet", delegate
            {
                Should_Cancel_Subscription();
            });
            cevap.SetNegativeButton("Hayır", delegate
            {
            });
            cevap.Show();
            
        }
        SpannableStringBuilder Spannla(Color Renk, string textt)
        {
            ForegroundColorSpan foregroundColorSpan = new ForegroundColorSpan(Renk);

            string title = textt;
            SpannableStringBuilder ssBuilder = new SpannableStringBuilder(title);
            ssBuilder.SetSpan(
                    foregroundColorSpan,
                    0,
                    title.Length,
                    SpanTypes.ExclusiveExclusive
            );
            return ssBuilder;
        }
        public void Should_Cancel_Subscription()
        {
            Initializee();
            CancelSubscriptionRequest request = new CancelSubscriptionRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = "123456789",
                SubscriptionReferenceCode = AktifAbonelikKodu
            };

            IyzipayResourceV2 response = Subscription.Cancel(request, options);
            if (response.Status=="success")
            {
                Toast.MakeText(this, "Aboneliğiniz İptal Edildi.", ToastLength.Long).Show();
                this.RunOnUiThread(delegate () {

                    MevcutPaketHanze.Visibility = ViewStates.Gone;
                    MevcutAlinmisPaket.Text = "";
                });
            }
            else
            {
                Toast.MakeText(this, "Bir sorun oluştu lütfen daha sonra tekrar deneyin", ToastLength.Long).Show();
            }
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


        protected override void OnStart()
        {
            base.OnStart();
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                KullaniciAbonelikSorgula();
            })).Start();
            
        }

        #region Paket Sorgula
        OdemeGecmisiDTO BenimkileriFiltrele;
        string AktifPaketAi,AktifAbonelikKodu;
        public void KullaniciAbonelikSorgula()
        {
            var MeData = DataBase.MEMBER_DATA_GETIR()[0];

            WebService webService = new WebService();
            var Donus = webService.OkuGetir("payment-histories");
            if (Donus != null)
            {
                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OdemeGecmisiDTO>>(Donus.ToString());
                if (Icerik.Count > 0)
                {
                    BenimkileriFiltrele = Icerik.FindLast(item => item.userId == MeData.id);
                    if (BenimkileriFiltrele != null)
                    {
                        var pricingPlanReferenceCode = "";
                        switch (BenimkileriFiltrele.packageName)
                        {
                            case "SILVER":
                                pricingPlanReferenceCode = "adb65336-ae25-40bf-b579-c9f20529bec6";
                                AktifPaketAi = "SILVER";
                                break;
                            case "GOLD":
                                pricingPlanReferenceCode = "8fa202ba-3ed3-4856-ae62-0282182d28d2";
                                AktifPaketAi = "GOLD";
                                break;
                            case "PLATINUM":
                                pricingPlanReferenceCode = "adb65336-ae25-40bf-b579-c9f20529bec6";
                                AktifPaketAi = "PLATINUM";
                                break;
                            default:
                                break;
                        }
                        var ReferansNumarasiGetir = DataBase.ODEME_GECMISI_GETIR_UZAKID(BenimkileriFiltrele.id);
                        if (ReferansNumarasiGetir.Count > 0)
                        {
                            Should_Search_Subscription(ReferansNumarasiGetir[0].iyzicoReferanceCode, pricingPlanReferenceCode, BenimkileriFiltrele.packageName);
                        }
                    }
                }
            }
        }
        
        void Should_Search_Subscription(string referanscode, string pricingPlanReferenceCode, string packageName)
        {
            Initializee();
            SearchSubscriptionRequest request = new SearchSubscriptionRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = "123456789",
                SubscriptionReferenceCode = referanscode,
                Page = 1,
                Count = 1,
                SubscriptionStatus = SubscriptionStatus.ACTIVE.ToString(),
                PricingPlanReferenceCode = pricingPlanReferenceCode
            };

            ResponsePagingData<SubscriptionResource> response = Subscription.Search(request, options);
            if (response.Data.Items[response.Data.Items.Count - 1].SubscriptionStatus == "ACTIVE")
            {
                AktifAbonelikKodu = response.Data.Items[response.Data.Items.Count - 1].ReferenceCode;
                this.RunOnUiThread(delegate () {

                    MevcutPaketHanze.Visibility = ViewStates.Visible;
                    MevcutAlinmisPaket.Text = AktifPaketAi;
                });
            }
            
        }
        public void Initializee()
        {
            options = new Options();
            options.ApiKey = "sandbox-S8fBp3d3O6g2v4iLlweEymY7jRkFBQnV";
            options.SecretKey = "sandbox-trdXadVcZmdSN8GFnf6Cmb5pzGr8JIYE";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";
        }

        #endregion



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