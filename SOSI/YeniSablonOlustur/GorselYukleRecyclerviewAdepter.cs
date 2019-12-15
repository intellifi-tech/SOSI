using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
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
        
        
        public GorselYukleRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {

            
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class GorselYukleRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        private List<SablonDTO> mData = new List<SablonDTO>();
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
        GorselYukleRecyclerViewHolder HolderForAnimation;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            GorselYukleRecyclerViewHolder viewholder = holder as GorselYukleRecyclerViewHolder;
            HolderForAnimation = holder as GorselYukleRecyclerViewHolder;
           
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