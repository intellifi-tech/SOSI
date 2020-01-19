using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SOSI.GenericClass;
using SOSI.GenericUI;

namespace SOSI.IsletmeProfiliOlustur
{
    [Activity(Label = "SOSI")]
    public class IsletmeProfiliBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        ViewPager viewpager;
        RelativeLayout Transformiew;
        Button IlerButton, GeriButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.IsletmeProfiliBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            Transformiew = FindViewById<RelativeLayout>(Resource.Id.rootview);
            viewpager = FindViewById<ViewPager>(Resource.Id.viewPager1);
            viewpager.OffscreenPageLimit = 50;
            IlerButton = FindViewById<Button>(Resource.Id.button1);
            GeriButton = FindViewById<Button>(Resource.Id.button2);
            IlerButton.Click += IlerButton_Click;
            GeriButton.Click += GeriButton_Click;
            viepageratama();
            // Create your application here
        }

        private void GeriButton_Click(object sender, EventArgs e)
        {
            viewpager.SetCurrentItem(viewpager.CurrentItem - 1, true);
        }

        private void IlerButton_Click(object sender, EventArgs e)
        {
            if (viewpager.CurrentItem < 3)
            {
                viewpager.SetCurrentItem(viewpager.CurrentItem + 1, true);
            }
            else
            {
                ((LogoFragment)fragments[3]).IsletmeAdiniGetir();
                this.StartActivity(typeof(ProfilOlustuBaseActivity));
                OverridePendingTransition(Resource.Animation.enter_from_right, Resource.Animation.exit_to_left);
                this.Finish();
            }
        }

        Android.Support.V4.App.Fragment[] fragments;
        void viepageratama()
        {
            var ss1 = new SeoktorFragment(this);
            var ss2 = new HizmetFragment(this);
            var ss3 = new KurumsalRenkFragment(this);
            var ss4 = new LogoFragment(this);

            //Fragment array
            fragments = new Android.Support.V4.App.Fragment[]
            {
                ss1,
                ss2,
                ss3,
                ss4,
            };
            //Tab title array
            var titles = CharSequence.ArrayFromStringArray(new[] {
               "s1",
               "s2",
               "s3",
               "s4",
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


        public static class IsletmeBilgileri
        {
            public static string IsletmeAdi { get; set; }
        }

        public class StringDTO
        {
            public string Metin { get; set; }
            public bool IsSelect { get; set; }
        }

        #region KurumsalRenk
        public class Rgb
        {
            public int r { get; set; }
            public int g { get; set; }
            public int b { get; set; }
        }
        public class Hsl
        {
            public double h { get; set; }
            public int s { get; set; }
            public int l { get; set; }
        }
        public class KurumsalRenkDTO
        {
            public int colorId { get; set; }
            public string hexString { get; set; }
            public Rgb rgb { get; set; }
            public Hsl hsl { get; set; }
            public string name { get; set; }
            public bool IsSelect { get; set; }
        }
        #endregion

        public class SeoktorFragment : Android.Support.V4.App.Fragment
        {
            IsletmeProfiliBaseActivity IsletmeProfiliBaseActivity1;
            RecyclerView mRecyclerView;
            RecyclerView.LayoutManager mLayoutManager;
            StringRecyclerViewAdapter mViewAdapter;
            List<StringDTO> SektorList = new List<StringDTO>();
            public SeoktorFragment(IsletmeProfiliBaseActivity IsletmeProfiliBaseActivity2)
            {
                IsletmeProfiliBaseActivity1 = IsletmeProfiliBaseActivity2;
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                View view = inflater.Inflate(Resource.Layout.SektorSecBaseActivity, container, false);
                mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
                new System.Threading.Thread(new System.Threading.ThreadStart(async delegate
                {
                    await Task.Run(async delegate {
                        await Task.Delay(1000);
                        SektorleriGetir();
                    });
                })).Start();
                return view;
            }
            void SektorleriGetir()
            {
                for (int i = 0; i < 20; i++)
                {
                    SektorList.Add(new StringDTO() { 
                        IsSelect=false,
                        Metin = "Lorem Impus Sit Dolor Amed"
                    });
                }
                this.Activity.RunOnUiThread(delegate
                {
                    mViewAdapter = new StringRecyclerViewAdapter(SektorList, (Android.Support.V7.App.AppCompatActivity)this.Activity);
                    mRecyclerView.HasFixedSize = true;
                    mLayoutManager = new LinearLayoutManager(this.Activity);
                    mRecyclerView.SetLayoutManager(mLayoutManager);
                    mRecyclerView.SetAdapter(mViewAdapter);
                    mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                });
            }

            private void MViewAdapter_ItemClick(object sender, object[] e)
            {
                if (SektorList.Count > 0)
                {
                    SektorList.ForEach(item => item.IsSelect = false);
                    SektorList[(int)e[0]].IsSelect = true;
                    mViewAdapter.mData = SektorList;
                    mViewAdapter.NotifyDataSetChanged();
                }
            }

            public override void OnStart()
            {
                base.OnStart();
            }
        }
        public class HizmetFragment : Android.Support.V4.App.Fragment
        {
            IsletmeProfiliBaseActivity IsletmeProfiliBaseActivity1;
            RecyclerView mRecyclerView;
            RecyclerView.LayoutManager mLayoutManager;
            StringRecyclerViewAdapter mViewAdapter;
            List<StringDTO> SektorList = new List<StringDTO>();
            public HizmetFragment(IsletmeProfiliBaseActivity IsletmeProfiliBaseActivity2)
            {
                IsletmeProfiliBaseActivity1 = IsletmeProfiliBaseActivity2;
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                View view = inflater.Inflate(Resource.Layout.HizmetSecBaseActivity, container, false);
                mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
                new System.Threading.Thread(new System.Threading.ThreadStart(async delegate
                {
                    await Task.Run(async delegate {
                        await Task.Delay(1000);
                        SektorleriGetir();
                    });
                })).Start();
                return view;

            }
            void SektorleriGetir()
            {
                for (int i = 0; i < 20; i++)
                {
                    SektorList.Add(new StringDTO()
                    {
                        IsSelect = false,
                        Metin = "Lorem Impus Sit Dolor Amed"
                    });
                }
                this.Activity.RunOnUiThread(delegate
                {
                    mViewAdapter = new StringRecyclerViewAdapter(SektorList, (Android.Support.V7.App.AppCompatActivity)this.Activity);
                    mRecyclerView.HasFixedSize = true;
                    mLayoutManager = new LinearLayoutManager(this.Activity);
                    mRecyclerView.SetLayoutManager(mLayoutManager);
                    mRecyclerView.SetAdapter(mViewAdapter);
                    mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                });
            }

            private void MViewAdapter_ItemClick(object sender, object[] e)
            {
                if (SektorList.Count > 0)
                {
                    SektorList.ForEach(item => item.IsSelect = false);
                    SektorList[(int)e[0]].IsSelect = true;
                    mViewAdapter.mData = SektorList;
                    mViewAdapter.NotifyDataSetChanged();
                }
            }
        }
        public class KurumsalRenkFragment : Android.Support.V4.App.Fragment
        {
            IsletmeProfiliBaseActivity IsletmeProfiliBaseActivity1;
            RecyclerView mRecyclerView;
            RecyclerView.LayoutManager mLayoutManager;
            KurumsalRenkRecyclerViewAdapter mViewAdapter;
            List<KurumsalRenkDTO> KurumsalRenkDTO1 = new List<KurumsalRenkDTO>();
            public KurumsalRenkFragment(IsletmeProfiliBaseActivity IsletmeProfiliBaseActivity2)
            {
                IsletmeProfiliBaseActivity1 = IsletmeProfiliBaseActivity2;
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                View view = inflater.Inflate(Resource.Layout.KurumsalRenkSecBaseActivity, container, false);
                mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
                mRecyclerView.HasFixedSize = true;
                new System.Threading.Thread(new System.Threading.ThreadStart(async delegate
                {
                    await Task.Run(async delegate {
                        await Task.Delay(1000);
                        RekleriGetir();
                    });
                })).Start();
                return view;

            }

            void RekleriGetir()
            {
                ReadColorsFromJson();

                #region Genislik Alır
                int width = 0;
                int height = 0;

                mRecyclerView.Post(() =>
                {
                    width = mRecyclerView.Width;
                    height = mRecyclerView.Height;
                    var Genislik = (width / 4);

                    mViewAdapter = new KurumsalRenkRecyclerViewAdapter(KurumsalRenkDTO1, (Android.Support.V7.App.AppCompatActivity)this.Activity, Genislik);
                    mRecyclerView.HasFixedSize = true;
                    mLayoutManager = new GridLayoutManager(this.Activity,4);
                    mRecyclerView.SetLayoutManager(mLayoutManager);
                    mRecyclerView.SetAdapter(mViewAdapter);
                    mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                });

                #endregion
            }
            void ReadColorsFromJson()
            {
                string UlkelerJson, SehirlerJson;
                AssetManager assets = this.Activity.Assets;
                using (StreamReader sr = new StreamReader(assets.Open("ColorList.json")))
                {
                    SehirlerJson = sr.ReadToEnd();
                    KurumsalRenkDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KurumsalRenkDTO>>(SehirlerJson.ToString());
                }
            }
            private void MViewAdapter_ItemClick(object sender, object[] e)
            {
                if (KurumsalRenkDTO1.Count > 0)
                {
                    KurumsalRenkDTO1.ForEach(item => item.IsSelect = false);
                    KurumsalRenkDTO1[(int)e[0]].IsSelect = true;
                    mViewAdapter.mData = KurumsalRenkDTO1;
                    mViewAdapter.NotifyDataSetChanged();
                }
            }
        }
        public class LogoFragment : Android.Support.V4.App.Fragment
        {
            IsletmeProfiliBaseActivity IsletmeProfiliBaseActivity1;
            EditText IsletmeAdi;
            public LogoFragment(IsletmeProfiliBaseActivity IsletmeProfiliBaseActivity2)
            {
                IsletmeProfiliBaseActivity1 = IsletmeProfiliBaseActivity2;
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                View view = inflater.Inflate(Resource.Layout.LogoSecBaseActivity, container, false);
                IsletmeAdi = view.FindViewById<EditText>(Resource.Id.editText1);
                return view;
            }
            public void IsletmeAdiniGetir()
            {
                IsletmeBilgileri.IsletmeAdi = IsletmeAdi.Text;
            }
        }
    }
}