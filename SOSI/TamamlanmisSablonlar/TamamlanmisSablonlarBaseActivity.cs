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
using SOSI.DataBasee;
using SOSI.GenericClass;
using SOSI.GenericUI;
using SOSI.TamamlanmisSablonlar.SablonDetay;
using SOSI.TamamlanmisSablonlar.SablonIcerikleri;
using SOSI.WebServicee;
using static SOSI.TamamlanmisSablonlar.SablonIcerikleri.SablonIcerikleriBaseActivity;

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
        MEMBER_DATA Me = DataBase.MEMBER_DATA_GETIR()[0];

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
            SonTamamlanmisSablonuGetir();
        }
        void SonTamamlanmisSablonuGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("templates/user/"+ Me.id);
            if (Donus!=null)
            {
                TamamlanmisSablonDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TamamlanmisSablonDTO>>(Donus.ToString());
                if (TamamlanmisSablonDTO1.Count>0)
                {
                    mViewAdapter = new TamamlanmisSablonRecyclerViewAdapter(TamamlanmisSablonDTO1, this);
                    mRecyclerView.SetAdapter(mViewAdapter);
                    mViewAdapter.ItemClick += MViewAdapter_ItemClick;
                    var layoutManager = new LinearLayoutManager(this);
                    mRecyclerView.SetLayoutManager(layoutManager);
                }
            }
        }
        
        private void MViewAdapter_ItemClick(object sender, object[] e)
        {
            if ((bool)TamamlanmisSablonDTO1[(int)e[0]].complete)
            {
                SecilenSablon.SablonID = TamamlanmisSablonDTO1[(int)e[0]].id;
                this.StartActivity(typeof(SablonIcerikleriBaseActivity));
            }
            else
            {
                Toast.MakeText(this, "Şablonunuz henüz hazır değil. Lütfen tamamlanmasını bekleyin..", ToastLength.Long).Show();
            }
        }
    
        public class TamamlanmisSablonDTO
        {
            public bool? complete { get; set; }
            public string id { get; set; }
            public int mediaCount { get; set; }
            public string userId { get; set; }
        }
    }
}