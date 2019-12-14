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

namespace SOSI.MainPage
{
    [Activity(Label = "SOSI")]
    public class MainPageBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        CircleImageView IsletmeLogo;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainPageBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            IsletmeLogo = FindViewById<CircleImageView>(Resource.Id.profile_image);
        }
        protected override void OnStart()
        {
            base.OnStart();
            IsletmeLogo.BringToFront();
        }
    }
}