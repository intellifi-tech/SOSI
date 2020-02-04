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
using Newtonsoft.Json;
using Refractored.Controls;
using SOSI.GenericClass;
using SOSI.GenericUI;
using SOSI.WebServicee;
using static SOSI.IsletmeProfiliOlustur.ProfilOlustuBaseActivity;

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
                if (!EsksikVarmi())
                {
                    IsletmeBilgileriDTOOlustur();
                }

                //((LogoFragment)fragments[3]).IsletmeAdiniGetir();
                
            }
        }

        void IsletmeBilgileriDTOOlustur()
        {
            ShowLoading.Show(this);
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                string jsonString = "";
                this.RunOnUiThread(delegate ()
                {
                    KayitIcinIsletmeBilgileri kayitIcinIsletmeBilgileri = new KayitIcinIsletmeBilgileri()
                    {
                        companyColor = ((KurumsalRenkFragment)fragments[0]).GetSeletedColor(),
                        logoPath = ((LogoFragment)fragments[0]).GetCompanyLogoPath(),
                        name = ((LogoFragment)fragments[0]).GetCompanyName(),
                        sectorId = ((SeoktorFragment)fragments[0]).GetSeletedSectorID(),
                        serviceAreaId = ((HizmetFragment)fragments[0]).GetSeletedHizmetID(),
                    };
                    jsonString = JsonConvert.SerializeObject(kayitIcinIsletmeBilgileri);
                    IsletmeAdiClass.IsletmeAdi = kayitIcinIsletmeBilgileri.name;
                });
                 
                WebService webService = new WebService();
                var Donus = webService.ServisIslem("company-informations", jsonString);
                if (Donus != "Hata")
                {
                    ShowLoading.Hide();
                    this.RunOnUiThread(delegate ()
                    {
                        this.StartActivity(typeof(ProfilOlustuBaseActivity));
                        OverridePendingTransition(Resource.Animation.enter_from_right, Resource.Animation.exit_to_left);
                        this.Finish();
                    });
                }
                else
                {
                    this.RunOnUiThread(delegate ()
                    {
                        Toast.MakeText(this, "Bir sorun oluştu lütfen tekrar deneyin.", ToastLength.Long).Show();
                        ShowLoading.Hide();
                    });
                    
                }
                
            })).Start();
        }

        bool EsksikVarmi()
        {
            if (((SeoktorFragment)fragments[0]).GetSeletedSectorID() == "null")
            {
                Toast.MakeText(this, "Lütfen Sektör Seçin", ToastLength.Long).Show();
                return false;
            }
            else if(((HizmetFragment)fragments[0]).GetSeletedHizmetID() == "null")
            {
                Toast.MakeText(this, "Lütfen Hizmet Alanını Seçin", ToastLength.Long).Show();
                return false;
            }
            else if (((KurumsalRenkFragment)fragments[0]).GetSeletedColor() == "null")
            {
                Toast.MakeText(this, "Lütfen Kurumsal Rekginizi Seçin", ToastLength.Long).Show();
                return false;
            }
            else if (((LogoFragment)fragments[0]).GetCompanyName() == "null")
            {
                Toast.MakeText(this, "Lütfen İşletme Adınızı Yazın", ToastLength.Long).Show();
                return false;
            }
            else if (((LogoFragment)fragments[0]).GetCompanyLogoPath() == "null")
            {
                Toast.MakeText(this, "Lütfen İşletmenizin Logosunu Yükleyin", ToastLength.Long).Show();
                return false;
            }


            return false;
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
        public class KayitIcinIsletmeBilgileri
        {
            public string companyColor { get; set; }
            public string id { get; set; }
            public string logoPath { get; set; }
            public string name { get; set; }
            public string sectorId { get; set; }
            public string serviceAreaId { get; set; }
        }
       
        public class StringDTO
        {
            public string name { get; set; }
            public string id { get; set; }
            public string sectorId { get; set; }//Sadece Listesi Hizmetler DTO sunda geliyor
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
            public override void OnStart()
            {
                base.OnStart();
                ShowLoading.Show(this.Activity);
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    SektorleriGetir();

                })).Start();
            }
            void SektorleriGetir()
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("sectors");
                if (Donus!=null)
                {
                    SektorList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StringDTO>>(Donus.ToString());
                    if (SektorList.Count>0)
                    {
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
                    else
                    {
                        Toast.MakeText(this.Activity, "Sektörler alınamadı lütfen tekrar deneyin.", ToastLength.Long).Show();
                        return;
                    }
                }
                else
                {
                    Toast.MakeText(this.Activity, "Sektörler alınamadı lütfen tekrar deneyin.", ToastLength.Long).Show();
                    return;
                }
                ShowLoading.Hide();
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

            public string GetSeletedSectorID()
            {
                var SecilenSektor = SektorList.FindAll(item => item.IsSelect == true);

                if (SecilenSektor.Count > 0)
                {
                    return SecilenSektor.Last().id;
                }
                else
                {
                    return "null";
                }
            }
        }
        public class HizmetFragment : Android.Support.V4.App.Fragment
        {
            IsletmeProfiliBaseActivity IsletmeProfiliBaseActivity1;
            RecyclerView mRecyclerView;
            RecyclerView.LayoutManager mLayoutManager;
            StringRecyclerViewAdapter mViewAdapter;
            List<StringDTO> HizmetList = new List<StringDTO>();
            public HizmetFragment(IsletmeProfiliBaseActivity IsletmeProfiliBaseActivity2)
            {
                IsletmeProfiliBaseActivity1 = IsletmeProfiliBaseActivity2;
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                View view = inflater.Inflate(Resource.Layout.HizmetSecBaseActivity, container, false);
                mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
                return view;

            }
            public override void OnStart()
            {
                base.OnStart();

                ShowLoading.Show(this.Activity);
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    HizmetleriGetir();

                })).Start();
            }
            void HizmetleriGetir()
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("service-areas");
                if (Donus != null)
                {
                    HizmetList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StringDTO>>(Donus.ToString());
                    if (HizmetList.Count > 0)
                    {
                        this.Activity.RunOnUiThread(delegate
                        {
                            mViewAdapter = new StringRecyclerViewAdapter(HizmetList, (Android.Support.V7.App.AppCompatActivity)this.Activity);
                            mRecyclerView.HasFixedSize = true;
                            mLayoutManager = new LinearLayoutManager(this.Activity);
                            mRecyclerView.SetLayoutManager(mLayoutManager);
                            mRecyclerView.SetAdapter(mViewAdapter);
                            mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                        });
                    }
                    else
                    {
                        Toast.MakeText(this.Activity, "Hizmetler alınamadı lütfen tekrar deneyin.", ToastLength.Long).Show();
                        return;
                    }
                }
                else
                {
                    Toast.MakeText(this.Activity, "Hizmetler alınamadı lütfen tekrar deneyin.", ToastLength.Long).Show();
                    return;
                }
                ShowLoading.Hide();
            }

            private void MViewAdapter_ItemClick(object sender, object[] e)
            {
                if (HizmetList.Count > 0)
                {
                    HizmetList.ForEach(item => item.IsSelect = false);
                    HizmetList[(int)e[0]].IsSelect = true;
                    mViewAdapter.mData = HizmetList;
                    mViewAdapter.NotifyDataSetChanged();
                }
            }

            public string GetSeletedHizmetID()
            {
                var SecilenSektor = HizmetList.FindAll(item => item.IsSelect == true);

                if (SecilenSektor.Count > 0)
                {
                    return SecilenSektor.Last().id;
                }
                else
                {
                    return "null";
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

            public string GetSeletedColor()
            {
                var SecilenSektor = KurumsalRenkDTO1.FindAll(item => item.IsSelect == true);

                if (SecilenSektor.Count > 0)
                {
                    return SecilenSektor.Last().hexString;
                }
                else
                {
                    return "null";
                }
            }
        }
        public class LogoFragment : Android.Support.V4.App.Fragment
        {
            IsletmeProfiliBaseActivity IsletmeProfiliBaseActivity1;
            EditText IsletmeAdi;
            CircleImageView circleImageView;
            ImageButton LogoYukleButton;
            byte[] SecilenGoruntuByte;
            string LogoPath;
            public LogoFragment(IsletmeProfiliBaseActivity IsletmeProfiliBaseActivity2)
            {
                IsletmeProfiliBaseActivity1 = IsletmeProfiliBaseActivity2;
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                View view = inflater.Inflate(Resource.Layout.LogoSecBaseActivity, container, false);
                IsletmeAdi = view.FindViewById<EditText>(Resource.Id.editText1);
                circleImageView = view.FindViewById<CircleImageView>(Resource.Id.profile_image);
                LogoYukleButton = view.FindViewById<ImageButton>(Resource.Id.logoyuklebutton);
                LogoYukleButton.Click += LogoYukleButton_Click;
                return view;
            }

            private void LogoYukleButton_Click(object sender, EventArgs e)
            {
                var Intent = new Intent();
                Intent.SetType("image/*");
                Intent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(Intent, "Logo Seç"), 444);
            }

            public override void OnActivityResult(int requestCode, int resultCode, Intent data)
            {
                base.OnActivityResult(requestCode, resultCode, data);
                if ((requestCode == 444) && (resultCode == (int)Android.App.Result.Ok) && (data != null))
                {
                    Android.Net.Uri uri = data.Data;
                    using (var inputStream = this.Activity.ContentResolver.OpenInputStream(uri))
                    {
                        using (var streamReader = new System.IO.StreamReader(inputStream))
                        {
                            var bytes = default(byte[]);
                            using (var memstream = new System.IO.MemoryStream())
                            {
                                streamReader.BaseStream.CopyTo(memstream);
                                bytes = memstream.ToArray();
                                SecilenGoruntuByte = bytes;
                                circleImageView.SetImageURI(uri);
                            }
                        }
                    }
                }
            }
            public string GetCompanyName()
            {
                if (!String.IsNullOrEmpty(IsletmeAdi.Text.Trim()))
                {
                    return IsletmeAdi.Text.Trim();
                }
                else
                {
                    return "null";
                }
            }

            public string GetCompanyLogoPath()
            {
                if (!String.IsNullOrEmpty(LogoPath))
                {
                    return LogoPath;
                }
                else
                {
                    return "null";
                }
            }
        }
    }
}