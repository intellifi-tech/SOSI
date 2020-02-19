using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Util;
using static SOSI.YeniSablonOlustur.YeniSablonOlusturBaseActivity;

namespace SOSI.YeniSablonOlustur
{
    class GorselYukleRecyclerViewHolder : RecyclerView.ViewHolder
    {

        public ImageView GorselImageView;
        public VideoView VideoVieww;
        public GorselYukleRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {
            GorselImageView = itemView.FindViewById<ImageView>(Resource.Id.ımageView1);
            VideoVieww = itemView.FindViewById<VideoView>(Resource.Id.videoView1);
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class GorselYukleRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        public List<SablonDTO> mData = new List<SablonDTO>();
        AppCompatActivity BaseActivity;
        public event EventHandler<object[]> ItemClick;
        int Genislikk;
        public GorselYukleRecyclerViewAdapter(List<SablonDTO> GelenData, AppCompatActivity GelenContex,int GelenGenislik)
        {
            mData = GelenData;
            BaseActivity = GelenContex;
            Genislikk = GelenGenislik;
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
        
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            GorselYukleRecyclerViewHolder viewholder = holder as GorselYukleRecyclerViewHolder;
            var item = mData[position];
            viewholder.VideoVieww.Visibility = ViewStates.Invisible;
            if (!item.isVideo)
            {
                if (item.MediaUri!=null)
                {
                    viewholder.GorselImageView.SetImageURI(item.MediaUri);
                    viewholder.GorselImageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                }
                else
                {
                    viewholder.GorselImageView.SetScaleType(ImageView.ScaleType.CenterInside);
                }
            }
            else
            {
                if (item.MediaUri != null)
                {
                    viewholder.VideoVieww.Visibility = ViewStates.Visible;
                    viewholder.GorselImageView.SetImageBitmap(null);
                    //var mediaUrii = Android.Net.Uri.Parse(item.MediaUri.Path);
                    //var aaaa = new Java.IO.File(mediaUrii.Path);
                    //var bbb = Android.Net.Uri.FromFile(aaaa);
                    viewholder.VideoVieww.SetVideoURI(item.MediaUri);
                    viewholder.VideoVieww.SeekTo(1);
                }
                else
                {
                    viewholder.VideoVieww.Visibility = ViewStates.Invisible;
                }
            }
        }
      
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.YeniSablonGorselYukleCardView, parent, false);
            var paramss = v.LayoutParameters;
            paramss.Height = Genislikk;
            paramss.Width = Genislikk;
            v.LayoutParameters = paramss;
            return new GorselYukleRecyclerViewHolder(v, OnClick);
        }

        void OnClick(object[] Icerik)
        {
            if (ItemClick != null)
                ItemClick(this, Icerik);
        }
    }
}