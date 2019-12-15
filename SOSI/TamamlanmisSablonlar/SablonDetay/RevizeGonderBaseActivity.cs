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

namespace SOSI.TamamlanmisSablonlar.SablonDetay
{
    [Activity(Label = "SOSI")]
    public class RevizeGonderBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        ImageButton Geri;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RevizeGonderBaseActivity);
            Geri = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            Geri.Click += Geri_Click;
        }

        private void Geri_Click(object sender, EventArgs e)
        {
            this.Finish();
        }
    }
}