using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Org.Json;
using SOSI.DataBasee;
using SOSI.GenericClass;
using SOSI.GenericUI;
using SOSI.Splashh;
using SOSI.WebServicee;
using static SOSI.GirisKayit.GirisBaseActivity;

namespace SOSI.GirisKayit
{
    [Activity(Label = "Contento")]
    public class HesapOlusturActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        Button KayitOlButton;
        EditText AdText, SoyadText, inputmail, SifreText, SifreTekrarText;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HesapOlusturActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Trans(this,true);
            KayitOlButton = FindViewById<Button>(Resource.Id.button1);
            AdText = FindViewById<EditText>(Resource.Id.editText1);
            SoyadText = FindViewById<EditText>(Resource.Id.editText2);
            inputmail = FindViewById<EditText>(Resource.Id.editText3);
            SifreText = FindViewById<EditText>(Resource.Id.editText4);
            SifreTekrarText = FindViewById<EditText>(Resource.Id.editText5);
            KayitOlButton.Click += KayitOlButton_Click;

            //AdText.Text = "Mesut";
            //SoyadText.Text = "Polat";
            //inputmail.Text = "mesut3@intellifi.tech";
            //SifreText.Text = "qwer1234";
            //SifreTekrarText.Text = "qwer1234";

        }

        private void KayitOlButton_Click(object sender, EventArgs e)
        {
            //StartActivity(typeof(HosgeldinActivity));
            //this.Finish();
            if (BosVarmi())
            {
                ShowLoading.Show(this);
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    WebService webService = new WebService();
                    KayitIcinRoot kayitIcinRoot = new KayitIcinRoot()
                    {
                        firstName = AdText.Text.Trim(),
                        lastName = SoyadText.Text.Trim(),
                        password = SifreText.Text,
                        login = inputmail.Text,
                        email = inputmail.Text,
                        authorities = new List<string>() { "ROLE_USER" }//ROLE_USER
                    };
                    string jsonString = JsonConvert.SerializeObject(kayitIcinRoot);
                    var Responsee = webService.ServisIslem("register", jsonString, true);
                    if (Responsee != "Hata")
                    {
                        TokenAlDevamEt();
                        ShowLoading.Hide();
                    }
                    else
                    {
                        ShowLoading.Hide();
                        AlertHelper.AlertGoster("Bir sorunla karşılaşıldı!", this);
                        return;
                    }
                })).Start();
            }

        }
        void TokenAlDevamEt()
        {
            LoginRoot loginRoot = new LoginRoot()
            {
                password = SifreText.Text,
                rememberMe = true,
                username = inputmail.Text,

            };
            string jsonString = JsonConvert.SerializeObject(loginRoot);
            WebService webService = new WebService();
            var Donus = webService.ServisIslem("authenticate", jsonString, true);
            if (Donus == "Hata")
            {
                ShowLoading.Hide();
                AlertHelper.AlertGoster("Giriş Yapılamadı!", this);
                return;
            }
            else
            {
                JSONObject js = new JSONObject(Donus);
                var Token = js.GetString("id_token");
                if (Token != null && Token != "")
                {
                    APITOKEN.TOKEN = Token;
                    if (GetMemberData())
                    {
                        ShowLoading.Hide();
                        StartActivity(typeof(HosgeldinActivity));
                        this.Finish();
                    }
                    else
                    {
                        ShowLoading.Hide();
                        AlertHelper.AlertGoster("Bir sorun oluştu lütfen daha sonra tekrar deneyin.", this);
                        return;
                    }
                }
            }
        }
        bool GetMemberData()
        {
            WebService webService = new WebService();
            var JSONData = webService.OkuGetir("account");
            if (JSONData != null)
            {
                var JsonSting = JSONData.ToString();

                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<MEMBER_DATA>(JSONData.ToString());
                Icerik.API_TOKEN = APITOKEN.TOKEN;
                Icerik.password = SifreText.Text;
                DataBase.MEMBER_DATA_EKLE(Icerik);
                return true;
            }
            else
            {
                return false;
            }

        }
        bool BosVarmi()
        {
            if (AdText.Text.Trim() == "")
            {
                Toast.MakeText(this, "Lütfen adınızı giriniz!", ToastLength.Long).Show();
                return false;
            }
            else if (SoyadText.Text.Trim() == "")
            {
                Toast.MakeText(this, "Lütfen soyadınızı giriniz!", ToastLength.Long).Show();
                return false;
            }
            else if (inputmail.Text.Trim() == "")
            {
                Toast.MakeText(this, "Lütfen emalinizi giriniz!", ToastLength.Long).Show();
                return false;
            }
            else if (SifreText.Text.Trim() == "")
            {
                Toast.MakeText(this, "Lütfen şifrenizi giriniz!", ToastLength.Long).Show();
                return false;
            }
            else if (SifreTekrarText.Text.Trim() == "")
            {
                Toast.MakeText(this, "Lütfen şifre tekrarını giriniz!", ToastLength.Long).Show();
                return false;
            }

            else
            {
                return true;
            }
        }

        public class KayitIcinRoot
        {
            public string email { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string login { get; set; }
            public string password { get; set; }
            public List<string> authorities { get; set; }
        }
    }
}