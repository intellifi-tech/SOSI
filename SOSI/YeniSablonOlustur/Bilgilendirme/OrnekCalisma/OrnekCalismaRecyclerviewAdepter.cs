using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Ebr163.Lib;
using Java.Util;
using static SOSI.YeniSablonOlustur.Bilgilendirme.OrnekCalisma.OrnekCalismaBaseActivity;

namespace SOSI.YeniSablonOlustur.Bilgilendirme.OrnekCalisma
{
    class OrnekCalismaRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public BifacialView BeforeAfterView;
        public OrnekCalismaRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {
            BeforeAfterView = itemView.FindViewById<BifacialView>(Resource.Id.bifacialvieww);
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class OrnekCalismaRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        private List<OrnekCalismaDTO> mData = new List<OrnekCalismaDTO>();
        AppCompatActivity BaseActivity;
        public event EventHandler<object[]> ItemClick;
        public OrnekCalismaRecyclerViewAdapter(List<OrnekCalismaDTO> GelenData, AppCompatActivity GelenContex)
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
        OrnekCalismaRecyclerViewHolder HolderForAnimation;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            OrnekCalismaRecyclerViewHolder viewholder = holder as OrnekCalismaRecyclerViewHolder;
            HolderForAnimation = holder as OrnekCalismaRecyclerViewHolder;
            var item = mData[position];
            GetBeforeAfterImages(viewholder.BeforeAfterView,item);
        }
        void GetBeforeAfterImages(BifacialView GelenView,OrnekCalismaDTO Itemm)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                var BeforeIMG = GetImageBitmapFromUrl(Itemm.beforeImagePath);
                var AfterIMG = GetImageBitmapFromUrl(Itemm.afterImagePath);
                Drawable BeforeIMGDrawable = new BitmapDrawable(BaseActivity.Resources, BeforeIMG);
                Drawable AfterIMGDrawable = new BitmapDrawable(BaseActivity.Resources, AfterIMG);
                BaseActivity.RunOnUiThread(delegate () {
                    GelenView.SetDrawableLeft(BeforeIMGDrawable);
                    GelenView.SetDrawableRight(AfterIMGDrawable);
                });
            })).Start();
        }
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                try
                {
                    var imageBytes = webClient.DownloadData("https://contentoapp.co/app/" + url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
                catch 
                {

                }

            }

            return imageBitmap;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.OrnekCalismalarCustomCardView, parent, false);

            return new OrnekCalismaRecyclerViewHolder(v, OnClick);
        }

        void OnClick(object[] Icerik)
        {
            if (ItemClick != null)
                ItemClick(this, Icerik);
        }
    }
}