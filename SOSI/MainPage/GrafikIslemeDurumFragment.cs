using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using SOSI.WebServicee;
using static SOSI.MainPage.MainPageBaseActivity;

namespace SOSI.MainPage
{
    public class GrafikIslemeDurumFragment : Android.Support.V4.App.Fragment
    {
        TextView BaslikText, AciklamaText,CounterText;
        LottieAnimationView lottieAnimationView;
        SablonlarDTO SonSablon;
        List<MedyaIcerikleriDTO> medyaIcerikleriDTOs = new List<MedyaIcerikleriDTO>();
        public GrafikIslemeDurumFragment(SablonlarDTO SonSablon1)
        {
            SonSablon = SonSablon1;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View RootView =  inflater.Inflate(Resource.Layout.GrafikIslemeDurum, container, false);
            BaslikText = RootView.FindViewById<TextView>(Resource.Id.textView1);
            AciklamaText = RootView.FindViewById<TextView>(Resource.Id.textView2);
            CounterText = RootView.FindViewById<TextView>(Resource.Id.countertext);
            lottieAnimationView = RootView.FindViewById<LottieAnimationView>(Resource.Id.animation_view1);
            return RootView;
        }
        public override void OnStart()
        {
            base.OnStart();
            GetSonDurum();
        }
        void GetSonDurum()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("template-medias/template/" + SonSablon.id);
            if (Donus != null)
            {
                medyaIcerikleriDTOs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MedyaIcerikleriDTO>>(Donus.ToString());
                var Total = medyaIcerikleriDTOs.Count;
                var Tamamlanan = medyaIcerikleriDTOs.FindAll(item => item.processed == true).Count;
                if (Tamamlanan != 0)
                {
                    if (Total == Tamamlanan)//Bitti
                    {
                        BaslikText.Text = "Paylaşım Şablonu Tamamlandı!";
                        AciklamaText.Text = "Tamamlanan şablonlar sayfasına giderek paylaşımlarınızı inceleyebilirsiniz.";
                        lottieAnimationView.SetAnimation("done1.json");
                        lottieAnimationView.PlayAnimation();
                    }
                }

                CounterText.Text = Tamamlanan.ToString() + "/" + Total.ToString();
            }
        }

        public class MedyaIcerikleriDTO
        {
            public string afterMediaPath { get; set; }
            public string beforeMediaPath { get; set; }
            public string id { get; set; }
            public int mediaCount { get; set; }
            public string postText { get; set; }
            public bool processed { get; set; }
            public DateTime shareDateTime { get; set; }
            public string templateId { get; set; }
            public string type { get; set; }
            public string userId { get; set; }
            public bool video { get; set; }
        }
    }
}