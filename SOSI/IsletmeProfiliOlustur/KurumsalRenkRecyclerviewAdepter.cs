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
    class KurumsalRenkRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public RelativeLayout RenkHaznesi;
        public ImageView Tick;
        public KurumsalRenkRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {
            RenkHaznesi = ItemView.FindViewById<RelativeLayout>(Resource.Id.renkhanesi);
            Tick = ItemView.FindViewById<ImageView>(Resource.Id.ımageView1);
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class KurumsalRenkRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        public List<KurumsalRenkDTO> mData = new List<KurumsalRenkDTO>();
        AppCompatActivity BaseActivity;
        public event EventHandler<object[]> ItemClick;
        int Genislikk;
        public KurumsalRenkRecyclerViewAdapter(List<KurumsalRenkDTO> GelenData, AppCompatActivity GelenContex, int GelenGenislik)
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
        KurumsalRenkRecyclerViewHolder HolderForAnimation;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            KurumsalRenkRecyclerViewHolder viewholder = holder as KurumsalRenkRecyclerViewHolder;
            HolderForAnimation = holder as KurumsalRenkRecyclerViewHolder;
            var item = mData[position];
            viewholder.RenkHaznesi.SetBackgroundColor(Color.ParseColor(item.hexString));
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
            View v = inflater.Inflate(Resource.Layout.KurumsalRenkCardView, parent, false);
            var paramss = v.LayoutParameters;
            paramss.Height = Genislikk;
            paramss.Width = Genislikk;
            v.LayoutParameters = paramss;
            return new KurumsalRenkRecyclerViewHolder(v, OnClick);
        }

        void OnClick(object[] Icerik)
        {
            if (ItemClick != null)
                ItemClick(this, Icerik);
        }
    }
}