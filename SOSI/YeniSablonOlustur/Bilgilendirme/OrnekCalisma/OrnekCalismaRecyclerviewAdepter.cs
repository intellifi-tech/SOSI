using System;
using System.Collections.Generic;
using System.Linq;
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
using Java.Util;
using static SOSI.YeniSablonOlustur.Bilgilendirme.OrnekCalisma.OrnekCalismaBaseActivity;

namespace SOSI.YeniSablonOlustur.Bilgilendirme.OrnekCalisma
{
    class OrnekCalismaRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public OrnekCalismaRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {
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