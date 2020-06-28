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
using SOSI.OdemePaketleri;

namespace SOSI.YeniSablonOlustur
{
    [Activity(Label = "Contento")]
    public class TebriklerSablonGonderildiBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanımlamalar
        LottieAnimationView animationView, animationView2;
        Button DevamEtButton,UcretsizDevamEt;
        TextView Baslik, Acikalama;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TebriklerSablonGonderildiBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view1);
            animationView2 = FindViewById<LottieAnimationView>(Resource.Id.animation_view2);
            DevamEtButton = FindViewById<Button>(Resource.Id.button1);
            UcretsizDevamEt = FindViewById<Button>(Resource.Id.button2);
            Baslik = FindViewById<TextView>(Resource.Id.textView1);
            Acikalama = FindViewById<TextView>(Resource.Id.textView2);
            DevamEtButton.Visibility = ViewStates.Invisible;
            Baslik.Visibility = ViewStates.Invisible;
            Acikalama.Visibility = ViewStates.Invisible;
            if (!TebriklerSablonGonderildiBaseActivity_Helper.OdemeliMusteri)
            {
                Acikalama.Text = "İşletmeniz için oluşturduğunuz paylaşım akışı şuan bize aktarılıyor. Ücretsiz deneme sürümü boyunca ilk 3 içerik 5 günde paylaşılmak üzere hazırlanacak! Eğer devam etmek istersen, sonraki 24 saat içerisinde uzman tasarımcılarımız bir aylık içeriğinizi tasarlayıp paylaşıma hazır getirecek!";
            }
            else
            {
                Acikalama.Text = "İşletmeniz için oluşturduğunuz paylaşım akışı şuan bize aktarılıyor. Aktarım tamamlandıktan sonraki 24 saat içerisinde uzman tasarımcılarımız ile birlikte paylaşımlarınızı hazır hale getireceğiz. Tasarım sürecini AnaSayfa'dan takip edebilirsiniz.";
            }
            DevamEtButton.Click += DevamEtButton_Click;
            UcretsizDevamEt.Click += UcretsizDevamEt_Click;
        }

        private void UcretsizDevamEt_Click(object sender, EventArgs e)
        {
            this.Finish();
        }

        private void DevamEtButton_Click(object sender, EventArgs e)
        {
            if (!TebriklerSablonGonderildiBaseActivity_Helper.OdemeliMusteri)
            {
                StartActivity(typeof(OdemePaketleriBaseActivity));
            }
            else
            {
                StartActivity(typeof(OdemePaketleriBaseActivity));
            }
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
      
        public static class TebriklerSablonGonderildiBaseActivity_Helper
        {
            public static bool OdemeliMusteri { get; set; }
        }

    }
}