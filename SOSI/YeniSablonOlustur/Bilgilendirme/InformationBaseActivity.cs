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
using SOSI.WebServicee;
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
        List<HowToUseInfomationDTO> HowToUseInfomationDTO1 = new List<HowToUseInfomationDTO>();
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
            _indicator = FindViewById<CirclePageIndicator>(Resource.Id.circlePageIndicator1);
            viepageratama();
            
           
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
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("how-to-uses");
            if (Donus != null)
            {
                HowToUseInfomationDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HowToUseInfomationDTO>>(Donus.ToString());
                if (HowToUseInfomationDTO1.Count>0)
                {
                    fragments = new Android.Support.V4.App.Fragment[HowToUseInfomationDTO1.Count];
                    for (int i = 0; i < HowToUseInfomationDTO1.Count; i++)
                    {
                        fragments[i] = new InformationFragmentParca(HowToUseInfomationDTO1[i]);
                    }
                    var titelssss = new string[HowToUseInfomationDTO1.Count];
                    var titles = CharSequence.ArrayFromStringArray(titelssss);
                    try
                    {
                        this.RunOnUiThread(delegate () {
                            if (fragments.Length > 0)
                            {
                                viewpager.Adapter = new TabPagerAdaptor(this.SupportFragmentManager, fragments, titles);
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
                        });
                    }
                    catch
                    {
                    }
                }
            }
            
        }
      
        public class HowToUseInfomationDTO
        {
            public string id { get; set; }
            public string imagePath { get; set; }
            public string text { get; set; }
        }
        public class InformationFragmentParca : Android.Support.V4.App.Fragment
        {
            
            TextView MetinText1;
            ImageView imageview;
            HowToUseInfomationDTO HowToUseInfomationDTO1;
            public InformationFragmentParca(HowToUseInfomationDTO HowToUseInfomationDTO11)
            {
                HowToUseInfomationDTO1 = HowToUseInfomationDTO11;
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                View view = inflater.Inflate(Resource.Layout.InformationFragmentParca, container, false);
                imageview = view.FindViewById<ImageView>(Resource.Id.ımageView1);
                MetinText1 = view.FindViewById<TextView>(Resource.Id.textView1);
                MetinText1.Text = HowToUseInfomationDTO1.text;
                return view;
            }
        }
    }
}