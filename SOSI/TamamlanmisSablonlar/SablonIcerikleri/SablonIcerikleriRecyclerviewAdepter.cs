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
using static SOSI.TamamlanmisSablonlar.SablonIcerikleri.SablonIcerikleriBaseActivity;

namespace SOSI.TamamlanmisSablonlar.SablonIcerikleri
{
    class SablonIcerikleriRecyclerViewHolder : RecyclerView.ViewHolder
    {

        TextView AyText, IcerikSayisiText, TamamlanmaText;
        public SablonIcerikleriRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {

            AyText = itemView.FindViewById<TextView>(Resource.Id.textView1);
            IcerikSayisiText = itemView.FindViewById<TextView>(Resource.Id.textView2);
            TamamlanmaText = itemView.FindViewById<TextView>(Resource.Id.textView3);
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class SablonIcerikleriRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
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
            SablonIcerikleriRecyclerViewHolder viewholder = holder as SablonIcerikleriRecyclerViewHolder;
            HolderForAnimation = holder as SablonIcerikleriRecyclerViewHolder;
           
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
    }
}