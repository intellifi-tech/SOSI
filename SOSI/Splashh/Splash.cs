﻿using System;
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
using SOSI.DataBasee;
using SOSI.GenericClass;
using SOSI.GirisKayit;
using SOSI.IsletmeProfiliOlustur;
using SOSI.MainPage;

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
            new DataBase();
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
            await Task.Delay(2000);
            HazirlikYap();
            
        }
        void HazirlikYap()
        {
            this.RunOnUiThread(delegate ()
            {
                var Kullanici = DataBase.MEMBER_DATA_GETIR();

                if (Kullanici.Count > 0)
                {
                    var SirketBilgileri = DataBase.COMPANY_INFORMATION_GETIR();
                    if (SirketBilgileri.Count>0)
                    {
                        StartActivity(typeof(MainPageBaseActivity));//AppIntroBaseActivity
                        this.Finish();
                    }
                    else
                    {
                        StartActivity(typeof(IsletmeProfiliBaseActivity));//IsletmeProfiliBaseActivity
                        this.Finish();
                    }
                    
                }
                else
                {
                    //StartActivity(typeof(AnaMenuBaseActivitty));
                    //return;
                    StartActivity(typeof(AppIntroBaseActivity));
                    this.Finish();
                }
                
            });
            
        }
    }
}