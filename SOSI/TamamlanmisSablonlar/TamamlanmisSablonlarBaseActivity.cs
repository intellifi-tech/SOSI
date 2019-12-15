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
using SOSI.GenericUI;
using SOSI.TamamlanmisSablonlar.SablonDetay;

namespace SOSI.TamamlanmisSablonlar
{
    [Activity(Label = "SOSI")]
    public class TamamlanmisSablonlarBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        ImageButton Geri;
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        TamamlanmisSablonRecyclerViewAdapter mViewAdapter;
        List<TamamlanmisSablonDTO> TamamlanmisSablonDTO1 = new List<TamamlanmisSablonDTO>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TamamlanmisSablonlarBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            Geri = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            Geri.Click += Geri_Click;
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            mRecyclerView.HasFixedSize = true;
        }
        private void Geri_Click(object sender, EventArgs e)
        {
            //Yuklenen Fotoları Burada Tut Önce
            this.Finish();
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
                var Genislik = (width / 3);

                CreateGiftList();

                mViewAdapter = new TamamlanmisSablonRecyclerViewAdapter(TamamlanmisSablonDTO1, this, Genislik);
                mRecyclerView.SetAdapter(mViewAdapter);
                mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                //mRecyclerView.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Horizontal));
                //mRecyclerView.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Vertical));
                var layoutManager = new GridLayoutManager(this, 3);
                mRecyclerView.SetLayoutManager(layoutManager);
            });

            #endregion
        }
        DinamikAdresSec DinamikActionSheet1;
        List<Buttons_Image_DataModels> Butonlarr = new List<Buttons_Image_DataModels>();
        private void MViewAdapter_ItemClick(object sender, object[] e)
        {
            Butonlarr = new List<Buttons_Image_DataModels>();

            Butonlarr.Add(new Buttons_Image_DataModels()
            {
                Button_Text = "Paylaş",
                Button_Image = Resource.Drawable.share_2
            });
            Butonlarr.Add(new Buttons_Image_DataModels()
            {
                Button_Text = "Görüntüle",
                Button_Image = Resource.Drawable.eye
            });
            Butonlarr.Add(new Buttons_Image_DataModels()
            {
                Button_Text = "Revize Gönder",
                Button_Image = Resource.Drawable.send
            });

            DinamikActionSheet1 = new DinamikAdresSec(Butonlarr, "İşlemle Seç", "Bu görsel ile ne yapmak istersin?.", Buton_Click);
            DinamikActionSheet1.Show(this.SupportFragmentManager, "DinamikActionSheet1");
        }
        private void Buton_Click(object sender, EventArgs e)
        {
            var Index = (int)((Button)sender).Tag;
            if (Index == 0)
            {
               

            }
            else if (Index == 1)
            {
                this.StartActivity(typeof(SablonDetayBaseActivity));
            }
            else if (Index == Butonlarr.Count - 1)
            {
                this.StartActivity(typeof(RevizeGonderBaseActivity));
            }
            DinamikActionSheet1.Dismiss();
        }

        void CreateGiftList()
        {
            for (int i = 0; i < 25; i++)
            {
                TamamlanmisSablonDTO1.Add(new TamamlanmisSablonDTO());
            }
        }
        public class TamamlanmisSablonDTO
        {

        }
    }
}