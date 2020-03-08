using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Util;
using static SOSI.TamamlanmisSablonlar.TamamlanmisSablonlarBaseActivity;

namespace SOSI.TamamlanmisSablonlar
{
    class TamamlanmisSablonRecyclerViewHolder : RecyclerView.ViewHolder
    {

        public TextView AyText, IcerikSayisiText, TamamlanmaText;
        public TamamlanmisSablonRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {

            AyText = itemView.FindViewById<TextView>(Resource.Id.textView1);
            IcerikSayisiText = itemView.FindViewById<TextView>(Resource.Id.textView2);
            TamamlanmaText = itemView.FindViewById<TextView>(Resource.Id.textView3);
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class TamamlanmisSablonRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        private List<TamamlanmisSablonDTO> mData = new List<TamamlanmisSablonDTO>();
        AppCompatActivity BaseActivity;
        public event EventHandler<object[]> ItemClick;
        
        public TamamlanmisSablonRecyclerViewAdapter(List<TamamlanmisSablonDTO> GelenData, AppCompatActivity GelenContex)
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
        TamamlanmisSablonRecyclerViewHolder HolderForAnimation;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            TamamlanmisSablonRecyclerViewHolder viewholder = holder as TamamlanmisSablonRecyclerViewHolder;
            HolderForAnimation = holder as TamamlanmisSablonRecyclerViewHolder;
            var item = mData[position];
            viewholder.IcerikSayisiText.Text = item.mediaCount + " içerik";
            if ((bool)item.complete)
            {
                viewholder.TamamlanmaText.Text = "Tamamlandı";
                viewholder.TamamlanmaText.SetTextColor(Color.ParseColor("#1E8E3E"));
            }
            else
            {
                viewholder.TamamlanmaText.Text = "Tamamlanmadı";
                viewholder.TamamlanmaText.SetTextColor(Color.ParseColor("#eb0000"));
            }
        }
        
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.TamamlanmisSablonlarSablonListItem, parent, false);
            //var paramss = v.LayoutParameters;
            //paramss.Height = Genislikk;
            //paramss.Width = Genislikk;
            //v.LayoutParameters = paramss;
            return new TamamlanmisSablonRecyclerViewHolder(v, OnClick);
        }

        void OnClick(object[] Icerik)
        {
            if (ItemClick != null)
                ItemClick(this, Icerik);
        }
    }
}