﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Model.V2;
using Iyzipay.Model.V2.Subscription;
using Iyzipay.Request.V2.Subscription;
using Refractored.Controls;
using SOSI.DataBasee;
using SOSI.GenericClass;
using SOSI.GenericClass.Contento_Helpers;
using SOSI.OdemePaketleri;
using SOSI.Paketler;
using SOSI.TamamlanmisSablonlar;
using SOSI.WebServicee;
using SOSI.YeniSablonOlustur;
using static SOSI.GenericClass.Contento_Helpers.Contento_HelperClasses;
using static SOSI.YeniSablonOlustur.YeniSablonOlusturBaseActivity;

namespace SOSI.MainPage
{
    [Activity(Label = "Contento")]
    public class MainPageBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        CircleImageView IsletmeLogo;
        Android.Support.V4.App.FragmentTransaction ft;
        Button HazirSablonlar, YeniSablonOlustur;
        ImageButton PaketAl;
        protected Options options;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainPageBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            IsletmeLogo = FindViewById<CircleImageView>(Resource.Id.profile_image);
            HazirSablonlar = FindViewById<Button>(Resource.Id.button1);
            YeniSablonOlustur = FindViewById<Button>(Resource.Id.button2);
            PaketAl = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            PaketAl.Click += PaketAl_Click;
            HazirSablonlar.Click += HazirSablonlar_Click;
            YeniSablonOlustur.Click += YeniSablonOlustur_Click;
            MainPageBaseActivity_Helper.MainPageBaseActivity1 = this;
        }

        private void PaketAl_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(OdemePaketleriBaseActivity));
            return;
            var PaketlerDialogFragment1 = new PaketlerDialogFragment();
            PaketlerDialogFragment1.Show(this.SupportFragmentManager, "PaketlerDialogFragment1");
        }

        private void YeniSablonOlustur_Click(object sender, EventArgs e)
        {

            //KullaniciAbonelikSorgula();
            //return;

            var PaylasimSayisiDialogFragment1 = new PaylasimSayisiDialogFragment();
            PaylasimSayisiDialogFragment1.Show(this.SupportFragmentManager, "PaylasimSayisiDialogFragment1");
        }

        private void HazirSablonlar_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(TamamlanmisSablonlarBaseActivity));
        }

        protected override void OnStart()
        {
            base.OnStart();
            IsletmeLogo.BringToFront();
            var CompanyInfo = DataBase.COMPANY_INFORMATION_GETIR()[0];
            new SetImageHelper().SetImage(this, IsletmeLogo, CompanyInfo.logoPath);
            SablonKontrol();
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                new ContentoNotificationInit_Helper(this).Init();
            })).Start();
            
        }

       

        void SablonKontrol()
        {
            WebService webService = new WebService();
            var Me = DataBase.MEMBER_DATA_GETIR()[0];
            var Donus = webService.OkuGetir("templates/user/" + Me.id);
            if (Donus!=null)
            {
                var SablonlarDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SablonlarDTO>>(Donus.ToString());
                var YuklenecekMedialar = DataBase.YUKLENECEK_SABLON_GETIR();
                YuklenecekMedialar = YuklenecekMedialar.FindAll(item => item.isUploaded == false);
                if (SablonlarDTO1!=null)
                {
                    if (SablonlarDTO1.Count > 0 && YuklenecekMedialar.Count <= 0)
                    {
                        GetIslemeDurumFragment(SablonlarDTO1[SablonlarDTO1.Count - 1]);
                    }
                    else
                    {
                        GetSablonYokFragment();
                    }
                }
                else
                {
                    GetSablonYokFragment();
                }
            
            }
            else
            {
                GetSablonYokFragment();
            }
        }

        void GetIslemeDurumFragment(SablonlarDTO SonSablon)
        {
            ClearFragment();
            GrafikIslemeDurumFragment ZaplaBaseFragment1 = new GrafikIslemeDurumFragment(SonSablon,this);
            ft = this.SupportFragmentManager.BeginTransaction();
            ft.AddToBackStack(null);
            ft.Replace(Resource.Id.conteinerview, ZaplaBaseFragment1);//
            ft.Commit();
        }
        public void YeniSablonButtonGizle(ViewStates Durum)
        {
            YeniSablonOlustur.Visibility = Durum;
            HazirSablonlar.Visibility = ViewStates.Visible;
        }
        void GetSablonYokFragment()
        {
            ClearFragment();
            HazirlananSablonYokBaseFragment HazirlananSablonYokBaseFragment1 = new HazirlananSablonYokBaseFragment();
            ft = this.SupportFragmentManager.BeginTransaction();
            ft.AddToBackStack(null);
            ft.Replace(Resource.Id.conteinerview, HazirlananSablonYokBaseFragment1);//
            ft.Commit();
            HazirSablonlar.Visibility = YeniSablonOlustur.Visibility = ViewStates.Gone;
        }
        void ClearFragment()
        {
            foreach (var item in SupportFragmentManager.Fragments)
            {
                SupportFragmentManager.BeginTransaction().Remove(item).Commit();
            }
        }
        public override void OnBackPressed()
        {
            this.Finish();
        }
        public class SablonlarDTO
        {
            public bool complete { get; set; }
            public string id { get; set; }
            public int mediaCount { get; set; }
            public string userId { get; set; }
        }



        public class OdemeGecmisiDTO
        {
            public DateTime? date { get; set; }
            public string id { get; set; }
            public string packageId { get; set; }
            public string packageName { get; set; }
            public string userId { get; set; }
            public string userName { get; set; }
        }

        public static class MainPageBaseActivity_Helper
        {
            public static MainPageBaseActivity MainPageBaseActivity1 { get; set; }
        }
    }
}