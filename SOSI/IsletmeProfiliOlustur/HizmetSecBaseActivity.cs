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

namespace SOSI.IsletmeProfiliOlustur
{
    [Activity(Label = "SOSI")]
    public class HizmetSecBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        Button DevamEt;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HizmetSecBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Beyaz(this);
            DevamEt = FindViewById<Button>(Resource.Id.button1);
            DevamEt.Click += DevamEt_Click;
        }
        private void DevamEt_Click(object sender, EventArgs e)
        {
            this.StartActivity(typeof(KurumsalRenkSecBaseActivity));
            OverridePendingTransition(Resource.Animation.enter_from_right, Resource.Animation.exit_to_left);
            this.Finish();
        }
    }
}