using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SOSI.DataBasee;
using SOSI.GenericClass;
using SOSI.GenericUI;
using SOSI.WebServicee;
using static SOSI.GenericClass.Contento_Helpers.Contento_HelperClasses;
using static SOSI.TamamlanmisSablonlar.SablonIcerikleri.SablonIcerikleriBaseActivity;

namespace SOSI.TamamlanmisSablonlar.SablonDetay
{
    [Activity(Label = "SOSI")]
    public class SablonDetayBaseActivity : Android.Support.V7.App.AppCompatActivity, Android.Media.MediaPlayer.IOnPreparedListener
    {
        ImageButton Geri;
        ImageView VideoyaBaslaButton;
        TextView PostTipiText, PostTarihiTet, PostAciklamaText;
        ImageView PostImage;
        VideoView PostVideoView;
        RelativeLayout VideoHazne;
        Button PaylasButton;
        MediaController mediaController;
        IDownloader downloader = new Downloader();
        MEMBER_DATA Me;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TamamlanmisSablonDetayBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            Me = DataBase.MEMBER_DATA_GETIR()[0];
            Geri = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            PostTipiText = FindViewById<TextView>(Resource.Id.textView2);
            PostTarihiTet = FindViewById<TextView>(Resource.Id.textView3);
            PostAciklamaText = FindViewById<TextView>(Resource.Id.textView4);
            VideoHazne = FindViewById<RelativeLayout>(Resource.Id.videohazne);
            PostImage = FindViewById<ImageView>(Resource.Id.ımageView1);
            PostVideoView = FindViewById<VideoView>(Resource.Id.videoView1);
            VideoyaBaslaButton = FindViewById<ImageView>(Resource.Id.baslabutton);
            PaylasButton = FindViewById<Button>(Resource.Id.paylasbutton);
            PaylasButton.Click += PaylasButton_Click;


            string sablonId = Intent.GetStringExtra("sablonID");
            if (!string.IsNullOrEmpty(sablonId))
            {
                WebService webService = new WebService();
                var Donus = webService.OkuGetir("template-medias/" + sablonId);
                if (Donus!=null)
                {
                    SecilenSablonDTO.SecilenSablon = Newtonsoft.Json.JsonConvert.DeserializeObject<SablonIcerikleriDTO>(Donus.ToString());
                    if (SecilenSablonDTO.SecilenSablon==null)
                    {
                        Toast.MakeText(this, "Bir sorun oluştu.", ToastLength.Long).Show();
                        this.Finish();
                    }
                }
            }

            PostTipiText.Text = SecilenSablonDTO.SecilenSablon.type;
            mediaController = new MediaController(this);
            PostVideoView.SetMediaController(mediaController);
            downloader.OnFileDownloaded += Downloader_OnFileDownloaded;
            if (SecilenSablonDTO.SecilenSablon.shareDateTime != null)
            {
                PostTarihiTet.Text = Convert.ToDateTime(SecilenSablonDTO.SecilenSablon.shareDateTime).ToString("MMMM dd") + ", " + Convert.ToDateTime(SecilenSablonDTO.SecilenSablon.shareDateTime).ToString("HH:mm");
            }
            else
            {
                PostTarihiTet.Text = "";
            }

            PostAciklamaText.Text = SecilenSablonDTO.SecilenSablon.postText;

            Geri.Click += Geri_Click;
            VideoyaBaslaButton.Click += VideoyaBaslaButton_Click;

            if (SecilenSablonDTO.SecilenSablon.video)
            {
                PostImage.Visibility = ViewStates.Gone;
                VideoHazne.Visibility = ViewStates.Visible;
                String videoUrl = "https://contentoapp.co/app/" + SecilenSablonDTO.SecilenSablon.afterMediaPath;
                Android.Net.Uri video = Android.Net.Uri.Parse(videoUrl);
                PostVideoView.SetVideoURI(video);
                PostVideoView.SetOnPreparedListener(this);
            }
            else
            {
                PostImage.Visibility = ViewStates.Visible;
                VideoHazne.Visibility = ViewStates.Gone;
                new SetImageHelper().SetImage(this, PostImage, SecilenSablonDTO.SecilenSablon.afterMediaPath);
            }
        }

        private void PaylasButton_Click(object sender, EventArgs e)
        {
            ShowLoading.Show(this);
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                InstagramPaylas();
            })).Start();
            
        }

        #region Paylas
        void InstagramPaylas()
        {
            MedyayiIndırKaydet();
        }
        void MedyayiIndırKaydet()
        {
            downloader.DownloadFile("https://contentoapp.co/app/"  + SecilenSablonDTO.SecilenSablon.afterMediaPath, "SharedMedias");
        }
        private void Downloader_OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            this.RunOnUiThread(delegate () {

                ShowLoading.Hide();
            });
            if (e.FileSaved)//Başarılı
            {

                ClipboardManager clipboard = (ClipboardManager)GetSystemService(Context.ClipboardService);
                ClipData clip = ClipData.NewPlainText(SecilenSablonDTO.SecilenSablon.id, SecilenSablonDTO.SecilenSablon.postText);
                clipboard.PrimaryClip = (clip);

                var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim).AbsolutePath;

                var pathh = Path.Combine(dir, "SharedMedias");
                string pathh2 = Path.Combine(pathh, Path.GetFileName("https://contentoapp.co/app/" + SecilenSablonDTO.SecilenSablon.afterMediaPath));
                Java.IO.File media = new Java.IO.File(pathh2);
                Android.Net.Uri uri = Android.Net.Uri.FromFile(media);

                //Android.Net.Uri.Builder Builderr = new Android.Net.Uri.Builder();
                //Android.Net.Uri newUri;
                ////if (DahaOnceEklenenVarmi[i].isVideo)
                ////{
                ////    newUri = Builderr.Scheme("content").Path(DahaOnceEklenenVarmi[i].MediaUri).Authority("media").EncodedAuthority("media").Build();
                ////}
                ////else
                ////{
                ////    newUri = Builderr.Scheme("content").Path(DahaOnceEklenenVarmi[i].MediaUri).Authority("com.android.providers.media.documents").EncodedAuthority("com.android.providers.media.documents").Build();
                ////}
                //newUri = Android.Net.Uri.Parse(pathh2);


                byte[] ImageData = File.ReadAllBytes(uri.Path);

                string base64String = Convert.ToBase64String(ImageData);

                Intent shareIntent = new Intent(Intent.ActionSend);
                shareIntent.SetType(SecilenSablonDTO.SecilenSablon.video ? "video/*" : "image/*");
                shareIntent.AddFlags(ActivityFlags.NewTask);//FLAG_ACTIVITY_NEW_TASK
                shareIntent.PutExtra(Intent.ExtraStream, uri);

                this.GrantUriPermission("com.instagram.android", uri, ActivityFlags.GrantReadUriPermission);

                //shareIntent.PutExtra(Intent.ExtraText, SecilenSablonDTO.SecilenSablon.postText);
                shareIntent.SetPackage("com.instagram.android");
                this.StartActivity(shareIntent);
                this.RunOnUiThread(delegate () {

                    Toast.MakeText(this, "Paylaşım metni panayo kopyalandı! Paylaşım esnasında yapıştırmayı unutmayın.", ToastLength.Long).Show();
                });
                

            }
            else
            {
                this.RunOnUiThread(delegate () {
                    AlertHelper.AlertGoster("Bir sorunla karşılaşıldı!", this);
                });
                return;
            }
        }

        #endregion

        private void VideoyaBaslaButton_Click(object sender, EventArgs e)
        {
            PostVideoView.SeekTo(0);
            PostVideoView.Start();
            VideoyaBaslaButton.Visibility = ViewStates.Gone;
        }

        public void OnPrepared(MediaPlayer mp)
        {
            mp.SeekTo(2000);
        }
        private void Geri_Click(object sender, EventArgs e)
        {
            this.Finish();
        }
    }

    public class SecilenSablonDTO
    {
        public static SablonIcerikleriDTO SecilenSablon { get; set; }
    }
}