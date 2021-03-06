﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using SOSI.DataBasee;
using SOSI.GenericClass;
using SOSI.GenericUI;
using SOSI.MediaUploader;
using SOSI.YeniSablonOlustur.Bilgilendirme;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Model.V2;
using Iyzipay.Model.V2.Subscription;
using Iyzipay.Request.V2.Subscription;
using SOSI.WebServicee;
using SOSI.OdemePaketleri;
using static SOSI.YeniSablonOlustur.TebriklerSablonGonderildiBaseActivity;
using static SOSI.GenericClass.Contento_Helpers.Contento_HelperClasses;

namespace SOSI.YeniSablonOlustur
{
    [Activity(Label = "SOSI")]
    public class YeniSablonOlusturBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        ImageButton Geri, InformationButton;
        TextView AciklamaText;
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        GorselYukleRecyclerViewAdapter mViewAdapter;
        List<SablonDTO> SablonDTO1 = new List<SablonDTO>();
        Button GonderButton;
        //protected Options options;
        public static class App
        {
            public static Java.IO.File _file;
            public static Java.IO.File _dir;
            public static Bitmap bitmap;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.YeniSablonOlusturBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            Geri = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            InformationButton = FindViewById<ImageButton>(Resource.Id.ımageButton2);
            GonderButton = FindViewById<Button>(Resource.Id.button1);
            GonderButton.Click += GonderButton_Click;
            AciklamaText = FindViewById<TextView>(Resource.Id.textView2);
            AciklamaText.Text = "1 aylık paylaşım için lütfen " + YuklenecekMediaCountHelper.Countt + " adet içerik yükleyin.";
            Geri.Click += Geri_Click;
            InformationButton.Click += InformationButton_Click;
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);

            mRecyclerView.HasFixedSize = true;
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            builder.DetectFileUriExposure();
        }

        private void GonderButton_Click(object sender, EventArgs e)
        {
            ShowLoading.Show(this);
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                var HepsiOkeymi = SablonDTO1.FindAll(item => item.MediaUri == null);
                if (HepsiOkeymi.Count > 0)
                {
                    this.RunOnUiThread(delegate ()
                    {
                        Toast.MakeText(this, "Lütfen tüm içerik alanlarını doldurun", ToastLength.Long).Show();
                        ShowLoading.Hide();
                    });
                    return;
                }
                else
                {
                    KullaniciAbonelikSorgula();
                    this.RunOnUiThread(delegate ()
                    {
                        ShowLoading.Hide();
                    });
                }
            })).Start();
            
        }

        OdemeGecmisiDTO BenimkileriFiltrele;
        public void KullaniciAbonelikSorgula()
        {
            var MeData = DataBase.MEMBER_DATA_GETIR()[0];

            WebService webService = new WebService();
            var Donus = webService.OkuGetir("payment-histories");
            if (Donus != null)
            {
                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OdemeGecmisiDTO>>(Donus.ToString());
                if (Icerik.Count > 0)
                {
                    BenimkileriFiltrele = Icerik.FindLast(item => item.userId == MeData.id);
                    if (BenimkileriFiltrele != null)
                    {
                        var pricingPlanReferenceCode = "";
                        switch (BenimkileriFiltrele.packageName)
                        {
                            case "SILVER":
                                pricingPlanReferenceCode = Contento_Resources_Helper.SilverUrunCode;
                                break;
                            case "GOLD":
                                pricingPlanReferenceCode = Contento_Resources_Helper.GoldUrunCode;
                                break;
                            case "PLATINUM":
                                pricingPlanReferenceCode = Contento_Resources_Helper.PlatinumUrunCode;
                                break;
                            default:
                                break;
                        }
                        var ReferansNumarasiGetir = DataBase.ODEME_GECMISI_GETIR_UZAKID(BenimkileriFiltrele.id);
                        if (ReferansNumarasiGetir.Count > 0)
                        {
                            Should_Search_Subscription(ReferansNumarasiGetir[0].iyzicoReferanceCode, pricingPlanReferenceCode, BenimkileriFiltrele.packageName);
                        }
                        else
                        {
                            GonderimiBaslat();
                            TebriklerSablonGonderildiBaseActivity_Helper.OdemeliMusteri = false;
                            StartActivity(typeof(TebriklerSablonGonderildiBaseActivity));
                            this.Finish();
                        }
                    }
                    else
                    {
                        GonderimiBaslat();
                        TebriklerSablonGonderildiBaseActivity_Helper.OdemeliMusteri = false;
                        StartActivity(typeof(TebriklerSablonGonderildiBaseActivity));
                        this.Finish();
                    }
                }
                else
                {
                    GonderimiBaslat();
                    TebriklerSablonGonderildiBaseActivity_Helper.OdemeliMusteri = false;
                    StartActivity(typeof(TebriklerSablonGonderildiBaseActivity));
                    this.Finish();
                }
            }
            else
            {
                GonderimiBaslat();
                TebriklerSablonGonderildiBaseActivity_Helper.OdemeliMusteri = false;
                StartActivity(typeof(TebriklerSablonGonderildiBaseActivity));
                this.Finish();
            }
        }
        void Should_Search_Subscription(string referanscode, string pricingPlanReferenceCode, string packageName)
        {
            //Initializee();
            SearchSubscriptionRequest request = new SearchSubscriptionRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = "123456789",
                SubscriptionReferenceCode = referanscode,
                Page = 1,
                Count = 1,
                SubscriptionStatus = SubscriptionStatus.ACTIVE.ToString(),
                PricingPlanReferenceCode = pricingPlanReferenceCode
            };

            ResponsePagingData<SubscriptionResource> response = Subscription.Search(request, Contento_Resources_Helper.options);
            if (response.Data.Items[response.Data.Items.Count - 1].SubscriptionStatus == "ACTIVE")
            {
                GonderimiBaslat();
                TebriklerSablonGonderildiBaseActivity_Helper.OdemeliMusteri = true;
                StartActivity(typeof(TebriklerSablonGonderildiBaseActivity));
                this.Finish();
            }
            else
            {
                GonderimiBaslat();
                TebriklerSablonGonderildiBaseActivity_Helper.OdemeliMusteri = false;
                StartActivity(typeof(TebriklerSablonGonderildiBaseActivity));
                this.Finish();
            }
        }
        //public void Initializee()
        //{
        //    options = new Options();
        //    options.ApiKey = "sandbox-S8fBp3d3O6g2v4iLlweEymY7jRkFBQnV";
        //    options.SecretKey = "sandbox-trdXadVcZmdSN8GFnf6Cmb5pzGr8JIYE";
        //    options.BaseUrl = "https://sandbox-api.iyzipay.com";
        //}

        void GonderimiBaslat()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                new MediaUploaderService().Init(this);
            })).Start();
        }

        bool Actinmi = false;
        protected override void OnStart()
        {
            base.OnStart();
            if (!Actinmi)
            {
                GetItemss();
                Actinmi = true;
            }
            
        }

        //void YuklemeSayfasiniAc()
        //{
        //    var DevamEdenSablonVarmi = DataBase.YUKLENECEK_SABLON_GETIR();
        //    if (DevamEdenSablonVarmi.Count > 0)
        //    {
        //        YuklenecekMediaCountHelper.Countt = DevamEdenSablonVarmi[0].maxMediaCount;
        //        //this.Activity.StartActivity(typeof(YeniSablonOlusturBaseActivity));
        //    }
        //    else
        //    {
        //        if (SecilenPaketTag == 0)
        //        {
        //            YuklenecekMediaCountHelper.Countt = 10;
        //            this.Activity.StartActivity(typeof(YeniSablonOlusturBaseActivity));
        //        }
        //        else if (SecilenPaketTag == 1)
        //        {
        //            YuklenecekMediaCountHelper.Countt = 15;
        //            this.Activity.StartActivity(typeof(YeniSablonOlusturBaseActivity));
        //        }
        //        else
        //        {
        //            YuklenecekMediaCountHelper.Countt = 25;
        //            this.Activity.StartActivity(typeof(YeniSablonOlusturBaseActivity));
        //        }
        //    }
        //}



        void GetItemss()
        {
            #region Genislik Alır
            int width = 0;
            int height = 0;

            mRecyclerView.Post(() =>
            {
                width = mRecyclerView.Width;
                height = mRecyclerView.Height;
                var Genislik = (width / 4);

                CreateGiftList();

                mViewAdapter = new GorselYukleRecyclerViewAdapter(SablonDTO1, this, Genislik);
                mRecyclerView.SetAdapter(mViewAdapter);
                mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                //mRecyclerView.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Horizontal));
                //mRecyclerView.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Vertical));
                var layoutManager = new GridLayoutManager(this, 4);
                mRecyclerView.SetLayoutManager(layoutManager);
            });

            #endregion
        }
        DinamikAdresSec DinamikActionSheet1;
        List<Buttons_Image_DataModels> Butonlarr = new List<Buttons_Image_DataModels>();
        int SonSecilenItem = -1;
        private void MViewAdapter_ItemClick(object sender, object[] e)
        {
            SonSecilenItem = (int)e[0];

            Butonlarr = new List<Buttons_Image_DataModels>();

            Butonlarr.Add(new Buttons_Image_DataModels()
            {
                Button_Text = "Galeri",
                Button_Image = Resource.Drawable.photo
            });
            Butonlarr.Add(new Buttons_Image_DataModels()
            {
                Button_Text = "Kamera",
                Button_Image = Resource.Drawable.camera
            });

            DinamikActionSheet1 = new DinamikAdresSec(Butonlarr, "İşlemle Seç", "Devam etmek için aşağıdaki seçenekleri kullanın.", Buton_Click);
            DinamikActionSheet1.Show(this.SupportFragmentManager, "DinamikActionSheet1");
        }

        private void Buton_Click(object sender, EventArgs e)
        {
            var Index = (int)((Button)sender).Tag;
            if (Index == 0)
            {
                GaleridenSecMetod();
            }
            if (Index == 1)
            {
                KameradanCek();
            }
            DinamikActionSheet1.Dismiss();
        }
        void KameradanCek()
        {

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();
            }

            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new Java.IO.File(App._dir, String.Format("contentoPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));
            StartActivityForResult(intent, 0);
        }
        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = this.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }
        private void CreateDirectoryForPictures()
        {
            App._dir = new Java.IO.File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "Contento");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }
        void GaleridenSecMetod()
        {
            var cevap = new AlertDialog.Builder(this);
            cevap.SetCancelable(true);
            cevap.SetIcon(Resource.Mipmap.ic_launcher);
            cevap.SetTitle(Spannla(Color.Black, "Contento"));
            cevap.SetMessage(Spannla(Color.DarkGray, "Yüklemek istediğiniz medya tipini seçin"));
            cevap.SetPositiveButton("Resim", delegate
            {
                var Intent = new Intent();
                Intent.SetType("image/*");
                Intent.SetAction(Intent.ActionOpenDocument);
                StartActivityForResult(Intent.CreateChooser(Intent, "Resim Yükle"), 444);
                cevap.Dispose();
            });
            cevap.SetNegativeButton("Video", delegate
            {
                var Intent = new Intent();
                Intent.SetType("video/*");
                Intent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(Intent.CreateChooser(Intent, "Video Yükle"), 555);
                cevap.Dispose();
            });
            cevap.Show();
        }

        SpannableStringBuilder Spannla(Color Renk, string textt)
        {
            ForegroundColorSpan foregroundColorSpan = new ForegroundColorSpan(Renk);

            string title = textt;
            SpannableStringBuilder ssBuilder = new SpannableStringBuilder(title);
            ssBuilder.SetSpan(
                    foregroundColorSpan,
                    0,
                    title.Length,
                    SpanTypes.ExclusiveExclusive
            );

            return ssBuilder;
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if ((requestCode == 444) && resultCode == Android.App.Result.Ok && (data != null))
            {
                Android.Net.Uri uri = data.Data;
                SablonDTO1[SonSecilenItem].MediaUri = uri;
                SablonDTO1[SonSecilenItem].isVideo = false;
                mViewAdapter.mData = SablonDTO1;
                mViewAdapter.NotifyItemChanged(SonSecilenItem);
                SaveMediaLocalDB(SablonDTO1[SonSecilenItem], SonSecilenItem);
            }
            else if ((requestCode == 555) && resultCode == Android.App.Result.Ok && (data != null))
            {
                Android.Net.Uri uri = data.Data;
                SablonDTO1[SonSecilenItem].MediaUri = uri;
                SablonDTO1[SonSecilenItem].isVideo = true;
                mViewAdapter.mData = SablonDTO1;
                mViewAdapter.NotifyItemChanged(SonSecilenItem);
                SaveMediaLocalDB(SablonDTO1[SonSecilenItem], SonSecilenItem);
            }
            else if ((resultCode == Android.App.Result.Ok))
            {
                if (App._file != null)
                {
                    Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
                    mediaScanIntent.SetData(contentUri);
                    this.SendBroadcast(mediaScanIntent);
                    Android.Net.Uri uri = contentUri;

                   // Android.Net.Uri uri = data.Data;
                    SablonDTO1[SonSecilenItem].MediaUri = uri;
                    SablonDTO1[SonSecilenItem].isVideo = false;
                    mViewAdapter.mData = SablonDTO1;
                    mViewAdapter.NotifyItemChanged(SonSecilenItem);
                    SaveMediaLocalDB(SablonDTO1[SonSecilenItem], SonSecilenItem);
                }
            }
        }

        void SaveMediaLocalDB(SablonDTO gelenicerik, int positionn)
        {
            var YeniPaylasimMetinDialogFragment1 = new YeniPaylasimMetinDialogFragment(gelenicerik, positionn,this);
            YeniPaylasimMetinDialogFragment1.Cancelable = false;
            YeniPaylasimMetinDialogFragment1.Show(this.SupportFragmentManager, "YeniPaylasimMetinDialogFragment");
        }
        public void MetinGirildiMedyayiKaydet(SablonDTO gelenicerik, int positionn,string GelenAciklama)
        {
            var DahaOnceEklenenVarmi = DataBase.YUKLENECEK_SABLON_GETIR();
            if (DahaOnceEklenenVarmi.Count > 0)
            {
                var BuPosizyondavarmi = DahaOnceEklenenVarmi.FindAll(item => item.position == positionn);
                if (BuPosizyondavarmi.Count > 0)
                {
                    BuPosizyondavarmi[0].aciklama = GelenAciklama;
                    BuPosizyondavarmi[0].isVideo = gelenicerik.isVideo;
                    BuPosizyondavarmi[0].isUploaded = false;
                    //BuPosizyondavarmi[0].MediaUri = gelenicerik.MediaUri.Path;
                    var DosyaYolu = DosyayıLokaleKopyala(gelenicerik.MediaUri, gelenicerik);
                    if (DosyaYolu!="")
                    {
                        BuPosizyondavarmi[0].MediaUri = DosyaYolu;
                        DataBase.YUKLENECEK_SABLON_Guncelle(BuPosizyondavarmi[0]);
                    }
                }
                else
                {
                    var DosyaYolu = DosyayıLokaleKopyala(gelenicerik.MediaUri, gelenicerik);
                    if (DosyaYolu != "")
                    {
                        DataBase.YUKLENECEK_SABLON_EKLE(new YUKLENECEK_SABLON()
                        {
                            isUploaded = false,
                            isVideo = gelenicerik.isVideo,
                            maxMediaCount = YuklenecekMediaCountHelper.Countt,
                            MediaUri = DosyaYolu,
                            position = positionn,
                            aciklama = GelenAciklama,
                        });
                    }
                    
                }
            }
            else
            {

                var DosyaYolu = DosyayıLokaleKopyala(gelenicerik.MediaUri, gelenicerik);
                if (DosyaYolu != "")
                {
                    DataBase.YUKLENECEK_SABLON_EKLE(new YUKLENECEK_SABLON()
                    {
                        isUploaded = false,
                        isVideo = gelenicerik.isVideo,
                        maxMediaCount = YuklenecekMediaCountHelper.Countt,
                        MediaUri = DosyaYolu,
                        position = positionn,
                        aciklama = GelenAciklama,
                    });
                }
            }
        }

        string DosyayıLokaleKopyala(Android.Net.Uri uri, SablonDTO GelenSablonBilgileri)
        {
            try
            {
                Stream stream = ContentResolver.OpenInputStream(uri);
                byte[] byteArray;

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    byteArray = memoryStream.ToArray();
                }
                stream.Dispose();
                var Klasor = documentsFolder();
                string FileNamee = Guid.NewGuid().ToString();
                if (GelenSablonBilgileri.isVideo)
                {
                    FileNamee = FileNamee + ".mp4";
                }
                else
                {
                    FileNamee = FileNamee + ".png";
                }


                FileStream dosya = System.IO.File.Create(Klasor + "/" + FileNamee);
                dosya.Write(byteArray, 0, byteArray.Length);
                dosya.Close();

                return Klasor + "/" + FileNamee;
            }
            catch (Exception exx)
            {
                string aaa = exx.Message;
                return "";
            }

        }
        static string documentsFolder()
        {
            string path;
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Directory.CreateDirectory(path);
            if (!Directory.Exists(path+"/MediaFiles"))
            {
                Directory.CreateDirectory(path + "/MediaFiles");
            }
            return path +"/MediaFiles";
        }
        void CreateGiftList()
        {
            for (int i = 0; i < YuklenecekMediaCountHelper.Countt; i++)
            {
                SablonDTO1.Add(new SablonDTO());
            }

            var DahaOnceEklenenVarmi = DataBase.YUKLENECEK_SABLON_GETIR();
            for (int i = 0; i < DahaOnceEklenenVarmi.Count; i++)
            {
                Android.Net.Uri.Builder Builderr = new Android.Net.Uri.Builder();
                Android.Net.Uri newUri;
                //if (DahaOnceEklenenVarmi[i].isVideo)
                //{
                //    newUri = Builderr.Scheme("content").Path(DahaOnceEklenenVarmi[i].MediaUri).Authority("media").EncodedAuthority("media").Build();
                //}
                //else
                //{
                //    newUri = Builderr.Scheme("content").Path(DahaOnceEklenenVarmi[i].MediaUri).Authority("com.android.providers.media.documents").EncodedAuthority("com.android.providers.media.documents").Build();
                //}
                newUri = Android.Net.Uri.Parse(DahaOnceEklenenVarmi[i].MediaUri);
                SablonDTO1[DahaOnceEklenenVarmi[i].position].MediaUri = newUri;
                SablonDTO1[DahaOnceEklenenVarmi[i].position].isVideo = DahaOnceEklenenVarmi[i].isVideo;
            }


        }
        private void InformationButton_Click(object sender, EventArgs e)
        {
            this.StartActivity(typeof(InformationBaseActivity));
        }
        private void Geri_Click(object sender, EventArgs e)
        {
            //Yuklenen Fotoları Burada Tut Önce
            this.Finish();   
        }
        public class SablonDTO
        {
            public Android.Net.Uri MediaUri { get; set; }
            public bool isVideo { get; set; }
        }
        public static class YuklenecekMediaCountHelper
        {
            public static int Countt { get; set; }
        }

        public class OdemeGecmisiDTO
        {
            public DateTime? date { get; set; }
            public string id { get; set; }
            public string packageId { get; set; }
            public string packageName { get; set; }
            public string userId { get; set; }
            public string userName { get; set; }
        }
    }
}