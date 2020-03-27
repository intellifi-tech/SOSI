using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using SOSI.DataBasee;
using SOSI.GenericClass;
using SOSI.GenericUI;
using SOSI.WebServicee;
using static SOSI.IsletmeProfiliOlustur.ProfilOlustuBaseActivity;

namespace SOSI.IsletmeProfiliOlustur
{
    [Activity(Label = "SOSI")]
    public class IsletmeProfiliBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        public ViewPager viewpager;
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
                if (EsksikVarmi())
                {
                    IsletmeBilgileriDTOOlustur();
                }

                //((LogoFragment)fragments[3]).IsletmeAdiniGetir();
                
            }
        }

        void IsletmeBilgileriDTOOlustur()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                string jsonString = "";
                this.RunOnUiThread(delegate ()
                {
                    var logopath = OnceLogoyuYule();

                    COMPANY_INFORMATION kayitIcinIsletmeBilgileri = new COMPANY_INFORMATION()
                    {
                        companyColor = ((KurumsalRenkFragment)fragments[2]).GetSeletedColor(),
                        logoPath = logopath,
                        name = ((LogoFragment)fragments[3]).GetCompanyName(),
                        sectorId = ((SeoktorFragment)fragments[0]).GetSeletedSectorID(),
                        serviceAreaId = ((HizmetFragment)fragments[1]).GetSeletedHizmetID(),
                    };
                    jsonString = JsonConvert.SerializeObject(kayitIcinIsletmeBilgileri);
                    IsletmeAdiClass.IsletmeAdi = kayitIcinIsletmeBilgileri.name;
                    WebService webService = new WebService();
                    var Donus = webService.ServisIslem("company-informations", jsonString);
                    if (Donus != "Hata")
                    {
                        this.RunOnUiThread(delegate ()
                        {
                            var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<COMPANY_INFORMATION>(Donus.ToString());
                            if (DataBase.COMPANY_INFORMATION_EKLE(Icerik))
                            {
                                this.StartActivity(typeof(ProfilOlustuBaseActivity));
                                OverridePendingTransition(Resource.Animation.enter_from_right, Resource.Animation.exit_to_left);
                                this.Finish();
                            }
                        });
                    }
                    else
                    {
                        this.RunOnUiThread(delegate ()
                        {
                            Toast.MakeText(this, "Bir sorun oluştu lütfen tekrar deneyin.", ToastLength.Long).Show();
                        });

                    }
                });

            })).Start();
        }
        string OnceLogoyuYule()
        {
            var MeID = DataBase.MEMBER_DATA_GETIR()[0];
            byte[] mediabyte = ConvertImageToByte(((LogoFragment)fragments[3]).GetCompanyLogoPath());
            var client = new RestSharp.RestClient("http://46.45.185.15:9003/api/template-medias");
            client.Timeout = -1;
            var request = new RestSharp.RestRequest(RestSharp.Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Authorization", "Bearer " + MeID.API_TOKEN);
            request.AddParameter("mediaCount", 0);
            request.AddParameter("postText", "");
            request.AddParameter("templateId", "0");
            request.AddParameter("video", false);
            request.AddParameter("userId", MeID.id);
            request.AddParameter("processed", false);
            request.AddParameter("type", "POST");
            request.AddFile("photo", mediabyte, "sosi_media_file.png");
            RestSharp.IRestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.Unauthorized &&
                response.StatusCode != HttpStatusCode.InternalServerError &&
                response.StatusCode != HttpStatusCode.BadRequest &&
                response.StatusCode != HttpStatusCode.Forbidden &&
                response.StatusCode != HttpStatusCode.MethodNotAllowed &&
                response.StatusCode != HttpStatusCode.NotAcceptable &&
                response.StatusCode != HttpStatusCode.RequestTimeout &&
                response.StatusCode != HttpStatusCode.NotFound)
            {
                var jsonobj = response.Content;
                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<MediaUploadDTO>(jsonobj.ToString());
                if (Icerik != null)
                {
                    return Icerik.beforeMediaPath;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public byte[] ConvertImageToByte(Android.Net.Uri uri)
        {
            Stream stream = ContentResolver.OpenInputStream(uri);
            byte[] byteArray;

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                byteArray = memoryStream.ToArray();
            }
            return byteArray;
        }

        public class MediaUploadDTO
        {
            public string afterMediaPath { get; set; }
            public string beforeMediaPath { get; set; }
            public string id { get; set; }
            public int mediaCount { get; set; }
            public string postText { get; set; }
            public bool processed { get; set; }
            public string shareDateTime { get; set; }
            public string templateId { get; set; }
            public string type { get; set; }
            public string userId { get; set; }
            public bool video { get; set; }
        }

        bool EsksikVarmi()
        {
            if (((SeoktorFragment)fragments[0]).GetSeletedSectorID() == "null")
            {
                Toast.MakeText(this, "Lütfen Sektör Seçin", ToastLength.Long).Show();
                return false;
            }
            else if(((HizmetFragment)fragments[1]).GetSeletedHizmetID() == "null")
            {
                Toast.MakeText(this, "Lütfen Hizmet Alanını Seçin", ToastLength.Long).Show();
                return false;
            }
            else if (((KurumsalRenkFragment)fragments[2]).GetSeletedColor() == "null")
            {
                Toast.MakeText(this, "Lütfen Kurumsal Rekginizi Seçin", ToastLength.Long).Show();
                return false;
            }
            else if (((LogoFragment)fragments[3]).GetCompanyName() == "null")
            {
                Toast.MakeText(this, "Lütfen İşletme Adınızı Yazın", ToastLength.Long).Show();
                return false;
            }
            else if (((LogoFragment)fragments[3]).GetCompanyLogoPath() == null)
            {
                Toast.MakeText(this, "Lütfen İşletmenizin Logosunu Yükleyin", ToastLength.Long).Show();
                return false;
            }
            else
            {
                return true;
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
       
        public void SektorSecildiHizmetleriGuncelle(string SektorID)
        {
            ((HizmetFragment)fragments[1]).HizmetleriGetir(SektorID);
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
                if (mViewAdapter == null)
                {
                    new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                    {
                        SektorleriGetir();
                    })).Start();
                }

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
                            //SektorList.Add(new StringDTO() { 
                            // id="",
                            // name="Diğer",
                            // IsSelect=false,
                            // sectorId=""
                            //});
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
                        this.Activity.RunOnUiThread(delegate
                        {
                            Toast.MakeText(this.Activity, "Sektörler alınamadı lütfen tekrar deneyin.", ToastLength.Long).Show();
                            return;
                        });
                        
                    }
                }
                else
                {
                    this.Activity.RunOnUiThread(delegate
                    {
                        Toast.MakeText(this.Activity, "Sektörler alınamadı lütfen tekrar deneyin.", ToastLength.Long).Show();
                        return;
                    });
                }
            }
            private void MViewAdapter_ItemClick(object sender, object[] e)
            {
                if (SektorList.Count > 0)
                {
                    if (SektorList[(int)e[0]].name=="Diğer")
                    {

                    }
                    else
                    {
                        SektorList.ForEach(item => item.IsSelect = false);
                        SektorList[(int)e[0]].IsSelect = true;
                        mViewAdapter.mData = SektorList;
                        mViewAdapter.NotifyDataSetChanged();
                        IsletmeProfiliBaseActivity1.SektorSecildiHizmetleriGuncelle(SektorList[(int)e[0]].id);
                    }
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
            List<StringDTO> HizmetList_Hepsi = new List<StringDTO>();
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
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    HizmetleriGetir("");

                })).Start();
            }
            public void HizmetleriGetir(string SektorID)
            {
                if (string.IsNullOrEmpty(SektorID))
                {
                    WebService webService = new WebService();
                    var Donus = webService.OkuGetir("service-areas");
                    if (Donus != null)
                    {
                        HizmetList_Hepsi = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StringDTO>>(Donus.ToString());
                        //if (HizmetList_Hepsi.Count > 0)
                        //{
                        //    this.Activity.RunOnUiThread(delegate
                        //    {
                        //        mViewAdapter = new StringRecyclerViewAdapter(HizmetList_Hepsi, (Android.Support.V7.App.AppCompatActivity)this.Activity);
                        //        mRecyclerView.HasFixedSize = true;
                        //        mLayoutManager = new LinearLayoutManager(this.Activity);
                        //        mRecyclerView.SetLayoutManager(mLayoutManager);
                        //        mRecyclerView.SetAdapter(mViewAdapter);
                        //        mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                        //    });
                        //}
                        //else
                        //{
                        //    this.Activity.RunOnUiThread(delegate
                        //    {
                        //        Toast.MakeText(this.Activity, "Hizmetler alınamadı lütfen tekrar deneyin.", ToastLength.Long).Show();
                        //        return;
                        //    });
                        //}
                    }
                    else
                    {
                        this.Activity.RunOnUiThread(delegate
                        {
                            Toast.MakeText(this.Activity, "Hizmetler alınamadı lütfen tekrar deneyin.", ToastLength.Long).Show();
                            return;
                        });
                    }
                }
                else
                {
                    if (HizmetList_Hepsi.Count > 0)
                    {
                        HizmetList = HizmetList_Hepsi.FindAll(item => item.sectorId == SektorID);
                        this.Activity.RunOnUiThread(delegate ()
                        {
                            if (HizmetList.Count > 0)
                            {

                                if (mViewAdapter != null)
                                {
                                    if (mViewAdapter.mData.Count > 0)
                                    {
                                        mViewAdapter.mData = HizmetList;
                                        mViewAdapter.NotifyDataSetChanged();
                                    }
                                }
                                else
                                {
                                    mViewAdapter = new StringRecyclerViewAdapter(HizmetList_Hepsi, (Android.Support.V7.App.AppCompatActivity)this.Activity);
                                    mRecyclerView.HasFixedSize = true;
                                    mLayoutManager = new LinearLayoutManager(this.Activity);
                                    mRecyclerView.SetLayoutManager(mLayoutManager);
                                    mRecyclerView.SetAdapter(mViewAdapter);
                                    mViewAdapter.ItemClick -= MViewAdapter_ItemClick;
                                    mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                                }
                                IsletmeProfiliBaseActivity1.viewpager.SetCurrentItem(IsletmeProfiliBaseActivity1.viewpager.CurrentItem + 1, true);
                            }
                            else
                            {
                                mViewAdapter = null;
                                mRecyclerView.SetAdapter(mViewAdapter);
                                mRecyclerView.HasFixedSize = true;
                            }
                        });
                    }
                    else
                    {
                        this.Activity.RunOnUiThread(delegate
                        {
                            Toast.MakeText(this.Activity, "Hizmetler alınamadı lütfen tekrar deneyin.", ToastLength.Long).Show();
                            return;
                        });
                    }
                }
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
            Android.Net.Uri LogoPath;
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
                circleImageView.Click += LogoYukleButton_Click;
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
                                LogoYukleButton.Visibility = ViewStates.Gone;
                                LogoPath = uri;
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

            public Android.Net.Uri GetCompanyLogoPath()
            {
                if (!String.IsNullOrEmpty(LogoPath.ToString()))
                {
                    return LogoPath;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}