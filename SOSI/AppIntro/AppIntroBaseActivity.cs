using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using DK.Ostebaronen.Droid.ViewPagerIndicator;
using SOSI.GenericClass;
using SOSI.GirisKayit;

namespace SOSI.AppIntro
{
    [Activity(Label = "SOSI")]
    public class AppIntroBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        ViewPager viewpager;
        protected IPageIndicator _indicator;
        RelativeLayout Transformiew;
       
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AppIntro);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            Transformiew = FindViewById<RelativeLayout>(Resource.Id.rootview);
            viewpager = FindViewById<ViewPager>(Resource.Id.viewPager1);
            viepageratama();
            _indicator = FindViewById<CirclePageIndicator>(Resource.Id.circlePageIndicator1);
            _indicator.SetViewPager(viewpager);
            ((CirclePageIndicator)_indicator).Snap = true;
            var density = Resources.DisplayMetrics.Density;
            //((CirclePageIndicator)_indicator).SetBackgroundColor(Color.Argb(255, 204, 204, 204));
            ((CirclePageIndicator)_indicator).Radius = 5 * density;
            ((CirclePageIndicator)_indicator).PageColor = Color.Argb(255, 255, 255, 255);
            ((CirclePageIndicator)_indicator).FillColor = Color.Transparent;
            ((CirclePageIndicator)_indicator).StrokeColor = Color.White;
            ((CirclePageIndicator)_indicator).StrokeWidth = 2f;

            //BaslangicIslemleri();
        }


        Android.Support.V4.App.Fragment[] fragments;
        void viepageratama()
        {
            var ss1 = new IntroFragment("SOSİ", "LOREM", "Lorem impus sit dolor amed. Lorem impus sit dolor amed. Lorem impus sit dolor amed.", Resource.Mipmap.ic_launcher, false);
            var ss2 = new IntroFragment("SOSİ", "LOREM", "Lorem impus sit dolor amed. Lorem impus sit dolor amed. Lorem impus sit dolor amed.", Resource.Mipmap.ic_launcher, false);
            var ss3 = new IntroFragment("SOSİ", "LOREM", "Lorem impus sit dolor amed. Lorem impus sit dolor amed. Lorem impus sit dolor amed.", Resource.Mipmap.ic_launcher, true);

            //Fragment array
            fragments = new Android.Support.V4.App.Fragment[]
            {
                ss1,
                ss2,
                ss3,
         
            };
            //Tab title array
            var titles = CharSequence.ArrayFromStringArray(new[] {
               "s1",
               "s2",
               "s3",
            });
            try
            {
                viewpager.Adapter = new TabPagerAdaptor(this.SupportFragmentManager, fragments, titles);
            }
            catch
            {
            }
            viewpager.SetPageTransformer(true, new IntroTransformer(Transformiew));
        }
        public class IntroFragment : Android.Support.V4.App.Fragment
        {
            string Metin1, Metin2, Metin3;
            int imageid;
            bool buttondurum;
            TextView MetinText1,MetinText2,MetinText3;
            ImageView imageview;
            Button devamet;
            public IntroFragment(string metin1,string metin2,string metin3, int gelenimageid, bool buttonolsunmu)
            {
                Metin1 = metin1;
                Metin2 = metin2;
                Metin3 = metin3;
                imageid = gelenimageid;
                buttondurum = buttonolsunmu;
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                View view = inflater.Inflate(Resource.Layout.AppIntroParcaFragment, container, false);
                imageview = view.FindViewById<ImageView>(Resource.Id.ımageView1);
                devamet = view.FindViewById<Button>(Resource.Id.button1);
                MetinText1 = view.FindViewById<TextView>(Resource.Id.textView1);
                MetinText2 = view.FindViewById<TextView>(Resource.Id.textView2);
                MetinText3 = view.FindViewById<TextView>(Resource.Id.textView3);
                
                imageview.SetImageResource(imageid);

                MetinText1.Text = Metin1;
                MetinText2.Text = Metin2;
                MetinText3.Text = Metin3;
               

                if (buttondurum == false)
                    devamet.Visibility = ViewStates.Invisible;
                else
                    devamet.Click += Devamet_Click;
                return view;

            }

            private void Devamet_Click(object sender, EventArgs e)
            {
                this.Activity.StartActivity(typeof(GirisBaseActivity));
                this.Activity.Finish();

            }
        }
    }
}