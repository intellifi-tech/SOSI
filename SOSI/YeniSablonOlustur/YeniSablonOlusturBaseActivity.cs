using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using SOSI.DataBasee;
using SOSI.GenericClass;
using SOSI.YeniSablonOlustur.Bilgilendirme;

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
        }

        private void GonderButton_Click(object sender, EventArgs e)
        {
            var HepsiOkeymi = SablonDTO1.FindAll(item => item.MediaUri == null);
            if (HepsiOkeymi.Count > 0)
            {
                Toast.MakeText(this, "Lütfen tüm içerik alanlarını doldurun", ToastLength.Long).Show();
                return;
            }
            else
            {
                //Gönderme İşlemini Başlat
            }
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
        int SonSecilenItem = -1;
        private void MViewAdapter_ItemClick(object sender, object[] e)
        {
            SonSecilenItem = (int)e[0];
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
        }

        void SaveMediaLocalDB(SablonDTO gelenicerik, int positionn)
        {
            var DahaOnceEklenenVarmi = DataBase.YUKLENECEK_SABLON_GETIR();
            if (DahaOnceEklenenVarmi.Count > 0)
            {
                var BuPosizyondavarmi = DahaOnceEklenenVarmi.FindAll(item => item.position == positionn);
                if (BuPosizyondavarmi.Count > 0)
                {
                    BuPosizyondavarmi[0].isVideo = gelenicerik.isVideo;
                    BuPosizyondavarmi[0].isUploaded = false;
                    BuPosizyondavarmi[0].MediaUri = gelenicerik.MediaUri.Path;
                    DataBase.YUKLENECEK_SABLON_Guncelle(BuPosizyondavarmi[0]);
                }
                else
                {
                    DataBase.YUKLENECEK_SABLON_EKLE(new YUKLENECEK_SABLON()
                    {
                        isUploaded = false,
                        isVideo = gelenicerik.isVideo,
                        maxMediaCount = YuklenecekMediaCountHelper.Countt,
                        MediaUri = gelenicerik.MediaUri.Path,
                        position = positionn
                    });
                }
            }
            else
            {
                DataBase.YUKLENECEK_SABLON_EKLE(new YUKLENECEK_SABLON()
                {
                    isUploaded = false,
                    isVideo = gelenicerik.isVideo,
                    maxMediaCount = YuklenecekMediaCountHelper.Countt,
                    MediaUri = gelenicerik.MediaUri.Path,
                    position = positionn
                });
            }
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
                if (DahaOnceEklenenVarmi[i].isVideo)
                {
                    newUri = Builderr.Scheme("content").Path(DahaOnceEklenenVarmi[i].MediaUri).Authority("media").EncodedAuthority("media").Build();
                }
                else
                {
                    newUri = Builderr.Scheme("content").Path(DahaOnceEklenenVarmi[i].MediaUri).Authority("com.android.providers.media.documents").EncodedAuthority("com.android.providers.media.documents").Build();
                }
                
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
    }
}