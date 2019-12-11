using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using SOSI.GenericClass;

namespace SOSI.GirisKayit
{
    [Activity(Label = "SOSI")]
    public class GirisBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        TextView SifremiUnuttum,UyeOlText;
        EditText MailText;
        Button GirisButton;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GirisBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            SifremiUnuttum = FindViewById<TextView>(Resource.Id.textView5);
            UyeOlText = FindViewById<TextView>(Resource.Id.textView6);
            MailText = FindViewById<EditText>(Resource.Id.editText1);
            GirisButton = FindViewById<Button>(Resource.Id.button1);
            GirisButton.Click += GirisButton_Click;
            SifremiUnuttum.Click += SifremiUnuttum_Click;
            UyeOlText.Click += UyeOlText_Click;
        }

        private void UyeOlText_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(HesapOlusturActivity));
        }

        private void GirisButton_Click(object sender, EventArgs e)
        {
            //StartActivity(typeof(MainPageBaseActivity));
            this.Finish();
        }

        private void SifremiUnuttum_Click(object sender, EventArgs e)
        {
            //StartActivity(typeof(SifremiUnuttumMailGonder));
        }
    }
}