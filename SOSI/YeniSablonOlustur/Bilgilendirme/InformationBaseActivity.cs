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
using SOSI.YeniSablonOlustur.Bilgilendirme.OrnekCalisma;

namespace SOSI.YeniSablonOlustur.Bilgilendirme
{
    [Activity(Label = "SOSI")]
    public class InformationBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        ViewPager viewpager;
        protected IPageIndicator _indicator;
        ImageButton Ileri, Geri;
        Button OrnekCalismalar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InformationBaseAcivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            Ileri = FindViewById<ImageButton>(Resource.Id.ımageButton3);
            Geri = FindViewById<ImageButton>(Resource.Id.ımageButton2);
            OrnekCalismalar = FindViewById<Button>(Resource.Id.button1);
            OrnekCalismalar.Click += OrnekCalismalar_Click;
            Ileri.Click += Ileri_Click;
            Geri.Click += Geri_Click;
            viewpager = FindViewById<ViewPager>(Resource.Id.viewPager1);
            viepageratama();
            _indicator = FindViewById<CirclePageIndicator>(Resource.Id.circlePageIndicator1);
            _indicator.SetViewPager(viewpager);
            ((CirclePageIndicator)_indicator).Snap = true;
            var density = Resources.DisplayMetrics.Density;
            //((CirclePageIndicator)_indicator).SetBackgroundColor(Color.Argb(255, 204, 204, 204));
            ((CirclePageIndicator)_indicator).Radius = 5 * density;
            ((CirclePageIndicator)_indicator).PageColor = Color.ParseColor("#00A1FF");
            ((CirclePageIndicator)_indicator).FillColor = Color.Transparent;
            ((CirclePageIndicator)_indicator).StrokeColor = Color.ParseColor("#00A1FF");
            ((CirclePageIndicator)_indicator).StrokeWidth = 3f;
        }

        private void OrnekCalismalar_Click(object sender, EventArgs e)
        {
            this.StartActivity(typeof(OrnekCalismaBaseActivity));
        }

        private void Geri_Click(object sender, EventArgs e)
        {
            viewpager.CurrentItem = viewpager.CurrentItem - 1;
        }

        private void Ileri_Click(object sender, EventArgs e)
        {
            viewpager.CurrentItem = viewpager.CurrentItem + 1;
        }

        Android.Support.V4.App.Fragment[] fragments;
        void viepageratama()
        {
            var ss1 = new InformationFragmentParca("Ürünlerinizi ortalama 15-20cm mesafeden çekin.", 1);
            var ss2 = new InformationFragmentParca("Fotoğraflarınızda altın oran kanununa mümkün olduğunca dikkat edin.", 1);
            var ss3 = new InformationFragmentParca("Portre modunu sıklıkla kullanın.", 1);
            var ss4 = new InformationFragmentParca("Ters ışıkta fotoğraflar çekmemeye dikkat edin.", 1);
            var ss5 = new InformationFragmentParca("Ürününüzü görüntülerken arka planda işletmenizden detaylar ekleyin.", 1);
            var ss6 = new InformationFragmentParca("Örnek fotoğraflarımızdan esinlenin", 1);

            //Fragment array
            fragments = new Android.Support.V4.App.Fragment[]
            {
                ss1,
                ss2,
                ss3,
                ss4,
                ss5,
                ss6,

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
        }
        public class InformationFragmentParca : Android.Support.V4.App.Fragment
        {
            string Metin1;
            int imageid;
            TextView MetinText1;
            ImageView imageview;
            public InformationFragmentParca(string metin1, int gelenimageid)
            {
                Metin1 = metin1;
                imageid = gelenimageid;
               
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                View view = inflater.Inflate(Resource.Layout.InformationFragmentParca, container, false);
                imageview = view.FindViewById<ImageView>(Resource.Id.ımageView1);
                MetinText1 = view.FindViewById<TextView>(Resource.Id.textView1);
                MetinText1.Text = Metin1;
                return view;
            }
        }
    }
}