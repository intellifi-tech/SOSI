using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SOSI.GenericClass;
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

        MediaController mediaController;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TamamlanmisSablonDetayBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            Geri = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            PostTipiText = FindViewById<TextView>(Resource.Id.textView2);
            PostTarihiTet = FindViewById<TextView>(Resource.Id.textView3);
            PostAciklamaText = FindViewById<TextView>(Resource.Id.textView4);
            VideoHazne = FindViewById<RelativeLayout>(Resource.Id.videohazne);
            PostImage = FindViewById<ImageView>(Resource.Id.ımageView1);
            PostVideoView = FindViewById<VideoView>(Resource.Id.videoView1);
            VideoyaBaslaButton = FindViewById<ImageView>(Resource.Id.baslabutton);
            PostTipiText.Text = SecilenSablonDTO.SecilenSablon.type;
            mediaController = new MediaController(this);
            PostVideoView.SetMediaController(mediaController);
            
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
                String videoUrl = "https://ia800201.us.archive.org/12/items/BigBuckBunny_328/BigBuckBunny_512kb.mp4";
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