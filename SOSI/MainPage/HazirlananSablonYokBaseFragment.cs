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
using SOSI.YeniSablonOlustur;
using static SOSI.MainPage.MainPageBaseActivity;

namespace SOSI.MainPage
{
    public class HazirlananSablonYokBaseFragment : Android.Support.V4.App.Fragment
    {
        Button YeniSablonButton;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View vieww =  inflater.Inflate(Resource.Layout.HazirlananSablonYokBaseFragment, container, false);
            YeniSablonButton = vieww.FindViewById<Button>(Resource.Id.button2);
            YeniSablonButton.Click += YeniSablonButton_Click;
            return vieww;
        }

        private void YeniSablonButton_Click(object sender, EventArgs e)
        {
            //MainPageBaseActivity_Helper.MainPageBaseActivity1.KullaniciAbonelikSorgula();
            //return;
            var PaylasimSayisiDialogFragment1 = new PaylasimSayisiDialogFragment();
            PaylasimSayisiDialogFragment1.Show(this.Activity.SupportFragmentManager, "PaylasimSayisiDialogFragment1");
        }
    }
}