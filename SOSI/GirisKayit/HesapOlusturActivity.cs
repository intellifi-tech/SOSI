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
using SOSI.GenericClass;

namespace SOSI.GirisKayit
{
    [Activity(Label = "SOSI")]
    public class HesapOlusturActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        Button KayitOlButton;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HesapOlusturActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            KayitOlButton = FindViewById<Button>(Resource.Id.button1);
            KayitOlButton.Click += KayitOlButton_Click;
        }

        private void KayitOlButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(HosgeldinActivity));
            this.Finish();
        }
    }
}