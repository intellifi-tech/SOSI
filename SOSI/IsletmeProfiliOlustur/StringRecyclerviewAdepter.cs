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
using static SOSI.IsletmeProfiliOlustur.IsletmeProfiliBaseActivity;

namespace SOSI.IsletmeProfiliOlustur
{
    class StringRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public TextView Metin;
        public ImageView Tick;
        public StringRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {
            Metin = ItemView.FindViewById<TextView>(Resource.Id.textView1);
            Tick = ItemView.FindViewById<ImageView>(Resource.Id.ımageView1);
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class StringRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        public List<StringDTO> mData = new List<StringDTO>();
        AppCompatActivity BaseActivity;
        public event EventHandler<object[]> ItemClick;
        public StringRecyclerViewAdapter(List<StringDTO> GelenData, AppCompatActivity GelenContex)
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
        StringRecyclerViewHolder HolderForAnimation;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            StringRecyclerViewHolder viewholder = holder as StringRecyclerViewHolder;
            HolderForAnimation = holder as StringRecyclerViewHolder;
            var item = mData[position];
            viewholder.Metin.Text = item.name;
            if (item.IsSelect)
            {
                viewholder.Tick.Visibility = ViewStates.Visible;
            }
            else
            {
                viewholder.Tick.Visibility = ViewStates.Gone;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.StringCardView, parent, false);

            return new StringRecyclerViewHolder(v, OnClick);
        }

        void OnClick(object[] Icerik)
        {
            if (ItemClick != null)
                ItemClick(this, Icerik);
        }
    }
}