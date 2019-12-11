using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SOSI.AppIntro;
using SOSI.GenericClass;

namespace SOSI.Splashh
{
    [Activity(Label = "SOSI",MainLauncher =true)]
    public class Splash : Android.Support.V7.App.AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            SetContentView(Resource.Layout.Splash);
        }
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }
        async void SimulateStartup()
        {
            //Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
            await Task.Delay(2000); // Simulate a bit of startup work.
            //Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
            StartActivity(new Intent(Application.Context, typeof(AppIntroBaseActivity)));
            this.Finish();
        }
        async void HazirlikYap()
        {
            //new DataBase();
            await Task.Run(() => {
                Task.Delay(2000);
            });
            this.RunOnUiThread(delegate ()
            {
                StartActivity(typeof(AppIntroBaseActivity));
            });
            //var Kullanici = DataBase.USER_INFO_GETIR();

            //if (Kullanici.Count > 0)
            //{
            //    StartActivity(typeof(AnaMenuBaseActivitty));
            //}
            //else
            //{
            //    //StartActivity(typeof(AnaMenuBaseActivitty));
            //    //return;
            //    StartActivity(typeof(LoginBaseActivty));
            //}
        }
    }
}