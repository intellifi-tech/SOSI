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
using SOSI.GenericClass;
using SOSI.YeniSablonOlustur;

namespace SOSI.MainPage
{
    [Activity(Label = "SOSI")]
    public class MainPageBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        CircleImageView IsletmeLogo;
        Android.Support.V4.App.FragmentTransaction ft;
        Button HazirSablonlar, YeniSablonOlustur;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainPageBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            IsletmeLogo = FindViewById<CircleImageView>(Resource.Id.profile_image);
            HazirSablonlar = FindViewById<Button>(Resource.Id.button1);
            YeniSablonOlustur = FindViewById<Button>(Resource.Id.button2);
            HazirSablonlar.Click += HazirSablonlar_Click;
            YeniSablonOlustur.Click += YeniSablonOlustur_Click;
        }

        private void YeniSablonOlustur_Click(object sender, EventArgs e)
        {
            this.StartActivity(typeof(YeniSablonOlusturBaseActivity));
        }

        private void HazirSablonlar_Click(object sender, EventArgs e)
        {
            
        }

        protected override void OnStart()
        {
            base.OnStart();
            IsletmeLogo.BringToFront();
            GetIslemeDurumFragment();
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