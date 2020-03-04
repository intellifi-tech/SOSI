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
using SOSI.IsletmeProfiliOlustur;

namespace SOSI.GirisKayit
{
    [Activity(Label = "Contento")]
    public class HosgeldinActivity : Android.Support.V7.App.AppCompatActivity
    {
        Button DevamEt;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HosgeldinActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            DevamEt = FindViewById<Button>(Resource.Id.button1);
            DevamEt.Click += DevamEt_Click;
        }

        private void DevamEt_Click(object sender, EventArgs e)
        {
            this.StartActivity(typeof(IsletmeProfiliBaseActivity));
            this.Finish();
        }
    }
}