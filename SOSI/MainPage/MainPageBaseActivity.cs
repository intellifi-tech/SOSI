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
using Refractored.Controls;
using SOSI.DataBasee;
using SOSI.GenericClass;
using SOSI.Paketler;
using SOSI.TamamlanmisSablonlar;
using SOSI.WebServicee;
using SOSI.YeniSablonOlustur;
using static SOSI.GenericClass.Contento_Helpers.Contento_HelperClasses;

namespace SOSI.MainPage
{
    [Activity(Label = "SOSI")]
    public class MainPageBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        CircleImageView IsletmeLogo;
        Android.Support.V4.App.FragmentTransaction ft;
        Button HazirSablonlar, YeniSablonOlustur;
        ImageButton PaketAl;
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
        }

        private void PaketAl_Click(object sender, EventArgs e)
        {
            var PaketlerDialogFragment1 = new PaketlerDialogFragment();
            PaketlerDialogFragment1.Show(this.SupportFragmentManager, "PaketlerDialogFragment1");
        }

        private void YeniSablonOlustur_Click(object sender, EventArgs e)
        {
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
                if (SablonlarDTO1.Count > 0 && YuklenecekMedialar.Count <= 0)
                {
                    GetIslemeDurumFragment(SablonlarDTO1[SablonlarDTO1.Count-1]);
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
            GrafikIslemeDurumFragment ZaplaBaseFragment1 = new GrafikIslemeDurumFragment(SonSablon);
            ft = this.SupportFragmentManager.BeginTransaction();
            ft.AddToBackStack(null);
            ft.Replace(Resource.Id.conteinerview, ZaplaBaseFragment1);//
            ft.Commit();
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
    }
}