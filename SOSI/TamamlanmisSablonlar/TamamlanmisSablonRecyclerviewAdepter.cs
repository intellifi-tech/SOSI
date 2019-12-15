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
using static SOSI.TamamlanmisSablonlar.TamamlanmisSablonlarBaseActivity;

namespace SOSI.TamamlanmisSablonlar
{
    class TamamlanmisSablonRecyclerViewHolder : RecyclerView.ViewHolder
    {
        
        
        public TamamlanmisSablonRecyclerViewHolder(View itemView, Action<object[]> listener) : base(itemView)
        {

            
            itemView.Click += (sender, e) => listener(new object[] { base.Position,itemView });
        }
    }
    class TamamlanmisSablonRecyclerViewAdapter : RecyclerView.Adapter/*, ValueAnimator.IAnimatorUpdateListener*/
    {
        private List<TamamlanmisSablonDTO> mData = new List<TamamlanmisSablonDTO>();
        AppCompatActivity BaseActivity;
        public event EventHandler<object[]> ItemClick;
        int Genislikk;
        public TamamlanmisSablonRecyclerViewAdapter(List<TamamlanmisSablonDTO> GelenData, AppCompatActivity GelenContex,int GelenGenislik)
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
        TamamlanmisSablonRecyclerViewHolder HolderForAnimation;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            TamamlanmisSablonRecyclerViewHolder viewholder = holder as TamamlanmisSablonRecyclerViewHolder;
            HolderForAnimation = holder as TamamlanmisSablonRecyclerViewHolder;
           
        }
        
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.TamamlanmisSablonlarCustomCardView, parent, false);
            var paramss = v.LayoutParameters;
            paramss.Height = Genislikk;
            paramss.Width = Genislikk;
            v.LayoutParameters = paramss;
            return new TamamlanmisSablonRecyclerViewHolder(v, OnClick);
        }

        void OnClick(object[] Icerik)
        {
            if (ItemClick != null)
                ItemClick(this, Icerik);
        }
    }
}