using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SOSI.GenericClass;
using SOSI.GenericUI;
using SOSI.WebServicee;

namespace SOSI.YeniSablonOlustur.Bilgilendirme.OrnekCalisma
{
    [Activity(Label = "SOSI")]
    public class OrnekCalismaBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        ImageButton Geri;
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        OrnekCalismaRecyclerViewAdapter mViewAdapter;
        List<OrnekCalismaDTO> favorilerRecyclerViewDataModels = new List<OrnekCalismaDTO>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OrnekCalismalarBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            Geri = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            Geri.Click += Geri_Click;
        }
        protected override void OnStart()
        {
            base.OnStart();
            ShowLoading.Show(this);
            new System.Threading.Thread(new System.Threading.ThreadStart(async delegate
            {
                await Task.Run(async delegate {
                    await Task.Delay(1000);
                    OrnekCalismalariGetir();
                });
            })).Start();
        }

        void OrnekCalismalariGetir()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("examples");
            if (Donus != null)
            {
                favorilerRecyclerViewDataModels = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrnekCalismaDTO>>(Donus.ToString());
                if (favorilerRecyclerViewDataModels.Count > 0)
                {
                    this.RunOnUiThread(delegate
                    {
                        mViewAdapter = new OrnekCalismaRecyclerViewAdapter(favorilerRecyclerViewDataModels, (Android.Support.V7.App.AppCompatActivity)this);
                        mRecyclerView.HasFixedSize = true;
                        mLayoutManager = new LinearLayoutManager(this);
                        mRecyclerView.SetLayoutManager(mLayoutManager);
                        mRecyclerView.SetAdapter(mViewAdapter);
                        ShowLoading.Hide();
                    });
                }
            }
        }

        private void Geri_Click(object sender, EventArgs e)
        {
            this.Finish();
        }

        public class OrnekCalismaDTO
        {
            public string afterImagePath { get; set; }
            public string beforeImagePath { get; set; }
            public string id { get; set; }
            public string text { get; set; }
        }
    }
}