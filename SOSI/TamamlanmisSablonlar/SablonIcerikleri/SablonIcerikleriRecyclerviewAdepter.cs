using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Util;
using static SOSI.GenericClass.Contento_Helpers.Contento_HelperClasses;
using static SOSI.TamamlanmisSablonlar.SablonIcerikleri.SablonIcerikleriBaseActivity;

namespace SOSI.TamamlanmisSablonlar.SablonIcerikleri
{
    class SablonIcerikleriRecyclerViewHolder : RecyclerView.ViewHolder
    {

        public TextView PaylasimTipiText, PaylasimZamaniText, PostAciklama;
        public ImageView Resim;
        public VideoView Videoo;
        public SablonIcerikleriRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {

            PaylasimTipiText = itemView.FindViewById<TextView>(Resource.Id.textView2);
            PaylasimZamaniText = itemView.FindViewById<TextView>(Resource.Id.textView1);
            Resim = itemView.FindViewById<ImageView>(Resource.Id.ımageView1);
            Videoo = itemView.FindViewById<VideoView>(Resource.Id.videoView1);
            PostAciklama = itemView.FindViewById<TextView>(Resource.Id.textView3);


            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class SablonIcerikleriRecyclerViewAdapter : RecyclerView.Adapter,Android.Media.MediaPlayer.IOnPreparedListener
    {
        private List<SablonIcerikleriDTO> mData = new List<SablonIcerikleriDTO>();
        AppCompatActivity BaseActivity;
        public event EventHandler<object[]> ItemClick;
        
        public SablonIcerikleriRecyclerViewAdapter(List<SablonIcerikleriDTO> GelenData, AppCompatActivity GelenContex)
        {
            mData = GelenData;
            BaseActivity = GelenContex;
        }

        public override int GetItemViewType(int position)
        {
            return position;
        }
        public override int ItemCount
        {
            get
            {
                return mData.Count;
            }
        }
        SablonIcerikleriRecyclerViewHolder HolderForAnimation;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            //
            SablonIcerikleriRecyclerViewHolder viewholder = holder as SablonIcerikleriRecyclerViewHolder;
            HolderForAnimation = holder as SablonIcerikleriRecyclerViewHolder;
            var item = mData[position];
            if (item.video)
            {
                viewholder.Resim.Visibility = ViewStates.Gone;
                viewholder.Videoo.Visibility = ViewStates.Visible;
                String videoUrl = "https://ia800201.us.archive.org/12/items/BigBuckBunny_328/BigBuckBunny_512kb.mp4";
                Android.Net.Uri video = Android.Net.Uri.Parse(videoUrl);
                viewholder.Videoo.SetVideoURI(video);
                viewholder.Videoo.SetOnPreparedListener(this);
            }
            else
            {
                viewholder.Resim.Visibility = ViewStates.Visible;
                viewholder.Videoo.Visibility = ViewStates.Gone;
                new SetImageHelper().SetImage(BaseActivity, viewholder.Resim, item.afterMediaPath);
            }

            if (item.shareDateTime!=null)
            {
                viewholder.PaylasimZamaniText.Text = Convert.ToDateTime(item.shareDateTime).ToString("MMMM dd") + ", " + Convert.ToDateTime(item.shareDateTime).ToString("HH:mm");
            }
            else
            {
                viewholder.PaylasimZamaniText.Text = "";
            }


            viewholder.PaylasimTipiText.Text = item.type.ToString();
            viewholder.PostAciklama.Text = item.postText;
        }
        
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.SablonIcerikleriListCardItem, parent, false);
            //var paramss = v.LayoutParameters;
            //paramss.Height = Genislikk;
            //paramss.Width = Genislikk;
            //v.LayoutParameters = paramss;
            return new SablonIcerikleriRecyclerViewHolder(v, OnClick);
        }

        void OnClick(object[] Icerik)
        {
            if (ItemClick != null)
                ItemClick(this, Icerik);
        }

        public void OnPrepared(MediaPlayer mp)
        {
            mp.SeekTo(2000);
        }
    }
}