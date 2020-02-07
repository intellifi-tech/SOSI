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
        }
        void GetIslemeDurumFragment()
        {
            ClearFragment();
            GrafikIslemeDurumFragment ZaplaBaseFragment1 = new GrafikIslemeDurumFragment();
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
    }
}