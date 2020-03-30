using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SOSI.GenericClass;
using SOSI.GenericUI;
using SOSI.TamamlanmisSablonlar.SablonDetay;
using SOSI.WebServicee;

namespace SOSI.TamamlanmisSablonlar.SablonIcerikleri
{
    [Activity(Label = "Contento")]
    public class SablonIcerikleriBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        List<SablonIcerikleriDTO> sablonIcerikleriDTOs = new List<SablonIcerikleriDTO>();
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        SablonIcerikleriRecyclerViewAdapter mViewAdapter;
        IDownloader downloader;
        ImageButton GeriButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SablonIcerikleriBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            GeriButton = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            GeriButton.Click += GeriButton_Click;
            downloader.OnFileDownloaded += Downloader_OnFileDownloaded;
        }

        
        private void GeriButton_Click(object sender, EventArgs e)
        {
            this.Finish();
        }

        protected override void OnStart()
        {
            base.OnStart();
            GetMediaResources();
        }
        void GetMediaResources()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("template-medias/template/" + SecilenSablon.SablonID);
            if (Donus!=null)
            {
                sablonIcerikleriDTOs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SablonIcerikleriDTO>>(Donus.ToString());
                if (sablonIcerikleriDTOs.Count >0)
                {
                    mViewAdapter = new SablonIcerikleriRecyclerViewAdapter(sablonIcerikleriDTOs, this);
                    mRecyclerView.SetAdapter(mViewAdapter);
                    mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                    var layoutManager = new LinearLayoutManager(this);
                    mRecyclerView.SetLayoutManager(layoutManager);
                }
            }
        }
        DinamikAdresSec DinamikActionSheet1;
        List<Buttons_Image_DataModels> Butonlarr = new List<Buttons_Image_DataModels>();

        private void MViewAdapter_ItemClick(object sender, object[] e)
        {
            SecilenSablonDTO.SecilenSablon = sablonIcerikleriDTOs[(int)e[0]];
            Butonlarr = new List<Buttons_Image_DataModels>();

            Butonlarr.Add(new Buttons_Image_DataModels()
            {
                Button_Text = "Paylaş",
                Button_Image = Resource.Drawable.share_2
            });
            Butonlarr.Add(new Buttons_Image_DataModels()
            {
                Button_Text = "Görüntüle",
                Button_Image = Resource.Drawable.eye
            });
            Butonlarr.Add(new Buttons_Image_DataModels()
            {
                Button_Text = "Revize Gönder",
                Button_Image = Resource.Drawable.send
            });

            DinamikActionSheet1 = new DinamikAdresSec(Butonlarr, "İşlemle Seç", "Bu görsel ile ne yapmak istersin?.", Buton_Click);
            DinamikActionSheet1.Show(this.SupportFragmentManager, "DinamikActionSheet1");
        }
        private void Buton_Click(object sender, EventArgs e)
        {
            var Index = (int)((Button)sender).Tag;
            if (Index == 0)
            {
                InstagramPaylas();
            }
            else if (Index == 1)
            {
                 
                this.StartActivity(typeof(SablonDetayBaseActivity));
            }
            else if (Index == Butonlarr.Count - 1)
            {
                this.StartActivity(typeof(RevizeGonderBaseActivity));
            }
            DinamikActionSheet1.Dismiss();
        }
        void InstagramPaylas()
        {
            MedyayiIndırKaydet();
        }
        void MedyayiIndırKaydet()
        {
            downloader.DownloadFile("http://46.45.185.15/"+ SecilenSablonDTO.SecilenSablon.afterMediaPath,"SharedMedias" );
        }
        private void Downloader_OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)//Başarılı
            {
                ClipboardManager clipboard = (ClipboardManager)GetSystemService(Context.ClipboardService);
                ClipData clip = ClipData.NewPlainText(SecilenSablonDTO.SecilenSablon.id, SecilenSablonDTO.SecilenSablon.postText);
                clipboard.PrimaryClip =(clip);
                
                var pathh = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "SharedMedias");
                string pathh2 = Path.Combine(pathh, Path.GetFileName("http://46.45.185.15/" + SecilenSablonDTO.SecilenSablon.afterMediaPath));
                Java.IO.File media = new Java.IO.File(pathh2);
                Android.Net.Uri uri = Android.Net.Uri.FromFile(media);
                Intent shareIntent = new Intent(Intent.ActionSend);
                shareIntent.SetType(SecilenSablonDTO.SecilenSablon.video ? "video/*" : "image/*");
                shareIntent.AddFlags(ActivityFlags.NewTask);//FLAG_ACTIVITY_NEW_TASK
                shareIntent.PutExtra(Intent.ExtraStream, uri);
                shareIntent.PutExtra(Intent.ExtraText, SecilenSablonDTO.SecilenSablon.postText);
                shareIntent.SetPackage("com.instagram.android");
                this.StartActivity(shareIntent);
                Toast.MakeText(this, "Paylaşım metni panayo kopyalandı! Paylaşım esnasında yapıştırmayı unutmayın.", ToastLength.Long).Show();

            }
            else
            {
                AlertHelper.AlertGoster("Bir sorunla karşılaşıldı!", this);
                return;
            }
        }

        public class SablonIcerikleriDTO
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

        public static class SecilenSablon
        {
            public static string SablonID { get; set; }
        }
    }
}