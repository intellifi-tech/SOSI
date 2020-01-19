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
using Com.Airbnb.Lottie;
using SOSI.GenericClass;
using SOSI.MainPage;
using static SOSI.IsletmeProfiliOlustur.IsletmeProfiliBaseActivity;

namespace SOSI.IsletmeProfiliOlustur
{
    [Activity(Label = "SOSI",MainLauncher =false)]
    public class ProfilOlustuBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanımlamalar
        LottieAnimationView animationView, animationView2;
        Button DevamEtButton;
        TextView Baslik, Acikalama;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProfilOlustuBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view1);
            animationView2 = FindViewById<LottieAnimationView>(Resource.Id.animation_view2);
            DevamEtButton = FindViewById<Button>(Resource.Id.button1);
            Baslik = FindViewById<TextView>(Resource.Id.textView1);
            Acikalama = FindViewById<TextView>(Resource.Id.textView2);
            DevamEtButton.Visibility = ViewStates.Invisible;
            Baslik.Visibility = ViewStates.Invisible;
            Acikalama.Visibility = ViewStates.Invisible;
            Acikalama.Text = IsletmeBilgileri.IsletmeAdi + "\n" + "İşletme profiliniz oluşturuldu. Şimdi sosyal medya hesaplarınız için biraz grafik çalışalım!";
            DevamEtButton.Click += DevamEtButton_Click;
        }

        private void DevamEtButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainPageBaseActivity));
            this.Finish();
        }

        protected override void OnStart()
        {
            base.OnStart();
            new System.Threading.Thread(new System.Threading.ThreadStart(async delegate
            {
                await Task.Run(async delegate {
                    await Task.Delay(200);
                    StartBgAnimation();
                    await Task.Delay(500);
                    StartSuccessAnimation();
                    await Task.Delay(300);
                    RunOnUiThread(delegate () {
                        DevamEtButton.Visibility = ViewStates.Visible;
                        Baslik.Visibility = ViewStates.Visible;
                        Acikalama.Visibility = ViewStates.Visible;
                    });
                });
            })).Start();
        }
        void StartBgAnimation()
        {
            animationView.SetAnimation("tebriklerbg1.json");
            animationView.PlayAnimation();
        }
        void StartSuccessAnimation()
        {
            animationView2.SetAnimation("success1.json");
            animationView2.PlayAnimation();
            
        }
    }
}