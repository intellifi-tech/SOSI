using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SOSI.GenericClass;
using SOSI.YeniSablonOlustur.Bilgilendirme;

namespace SOSI.YeniSablonOlustur
{
    [Activity(Label = "SOSI")]
    public class YeniSablonOlusturBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        ImageButton Geri, InformationButton;

        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        GorselYukleRecyclerViewAdapter mViewAdapter;
        List<SablonDTO> SablonDTO1 = new List<SablonDTO>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.YeniSablonOlusturBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            Geri = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            InformationButton = FindViewById<ImageButton>(Resource.Id.ımageButton2);
            Geri.Click += Geri_Click;
            InformationButton.Click += InformationButton_Click;
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            mRecyclerView.HasFixedSize = true;
        }
        protected override void OnStart()
        {
            base.OnStart();
            GetItemss();
        }

        void GetItemss()
        {
            #region Genislik Alır
            int width = 0;
            int height = 0;

            mRecyclerView.Post(() =>
            {
                width = mRecyclerView.Width;
                height = mRecyclerView.Height;
                var Genislik = (width / 4);

                CreateGiftList();

                mViewAdapter = new GorselYukleRecyclerViewAdapter(SablonDTO1, this, Genislik);
                mRecyclerView.SetAdapter(mViewAdapter);
                mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                //mRecyclerView.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Horizontal));
                //mRecyclerView.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Vertical));
                var layoutManager = new GridLayoutManager(this, 4);
                mRecyclerView.SetLayoutManager(layoutManager);
            });

            #endregion
        }

        private void MViewAdapter_ItemClick(object sender, object[] e)
        {
            
        }

        void CreateGiftList()
        {
            for (int i = 0; i < 25; i++)
            {
                SablonDTO1.Add(new SablonDTO());
            }
        }
        private void InformationButton_Click(object sender, EventArgs e)
        {
            this.StartActivity(typeof(InformationBaseActivity));
        }

        private void Geri_Click(object sender, EventArgs e)
        {
            //Yuklenen Fotoları Burada Tut Önce
            this.Finish();   
        }
        public class SablonDTO
        {

        }
    }
}