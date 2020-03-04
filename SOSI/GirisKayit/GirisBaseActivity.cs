using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Org.Json;
using SOSI.DataBasee;
using SOSI.GenericClass;
using SOSI.GenericUI;
using SOSI.IsletmeProfiliOlustur;
using SOSI.MainPage;
using SOSI.Splashh;
using SOSI.WebServicee;

namespace SOSI.GirisKayit
{
    [Activity(Label = "Contento")]
    public class GirisBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        TextView SifremiUnuttum,UyeOlText;
        EditText MailText, SifreText;
        Button GirisButton;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GirisBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            SifremiUnuttum = FindViewById<TextView>(Resource.Id.textView5);
            UyeOlText = FindViewById<TextView>(Resource.Id.textView6);
            MailText = FindViewById<EditText>(Resource.Id.editText1);
            SifreText = FindViewById<EditText>(Resource.Id.editText2);
            GirisButton = FindViewById<Button>(Resource.Id.button1);
            GirisButton.Click += GirisButton_Click;
            SifremiUnuttum.Click += SifremiUnuttum_Click;
            UyeOlText.Click += UyeOlText_Click;
            MailText.Text = "mesut1@intellifi.tech";
            SifreText.Text = "qwer1234";
        }

        private void UyeOlText_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(HesapOlusturActivity));
        }

        private void GirisButton_Click(object sender, EventArgs e)
        {
            if (BosVarmi())
            {
                var mail = MailText.Text;
                var sifre = SifreText.Text;
                ShowLoading.Show(this);
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    GirisYapMetod(mail, sifre);
                    ShowLoading.Hide();
                })).Start();
            }
        }

        void GirisYapMetod(string email, string sifre, string AdSoyadText = "", string genderr = "", string DogumTarihi = "", bool isNotSocial = true)
        {
            LoginRoot loginRoot = new LoginRoot()
            {
                password = sifre,
                rememberMe = true,
                username = email
            };
            string jsonString = JsonConvert.SerializeObject(loginRoot);
            WebService webService = new WebService();
            var Donus = webService.ServisIslem("authenticate", jsonString, true);
            if (Donus != "Hata")
            {
                JSONObject js = new JSONObject(Donus);
                var Token = js.GetString("id_token");
                if (Token != null && Token != "")
                {
                    APITOKEN.TOKEN = Token;
                    if (GetMemberData(sifre))
                    {
                        ShowLoading.Hide();
                        this.StartActivity(typeof(Splash));
                        this.Finish();
                    }
                }
            }
            else
            {
                this.RunOnUiThread(delegate { 
                    Toast.MakeText(this, "Giriş Yapılamadı.", ToastLength.Long).Show();
                });
            }
        }
        bool GetMemberData(string PassWord)
        {
            WebService webService = new WebService();
            var JSONData = webService.OkuGetir("account");
            if (JSONData != null)
            {
                var JsonSting = JSONData.ToString();

                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<MEMBER_DATA>(JSONData.ToString());
                Icerik.API_TOKEN = APITOKEN.TOKEN;
                Icerik.password = PassWord;

                DataBase.MEMBER_DATA_EKLE(Icerik);

                return true;
            }
            else
            {
                ShowLoading.Hide();
                Toast.MakeText(this, "Giriş Yapılamadı.", ToastLength.Long).Show();
                //AlertHelper.AlertGoster("Giriş Yapılamadı!", this);
                return false;
            }
        }

        void GetCompanyInfo(string UserID)
        {
            WebService webService = new WebService();
        }

        bool BosVarmi()
        {
            if (MailText.Text.Trim() == "")
            {
                Toast.MakeText(this, "Lütfen mail adresinizi yazın.", ToastLength.Long).Show();
                return false;
            }
            else
            {
                if (SifreText.Text.Trim() == "")
                {
                    Toast.MakeText(this, "Lütfen şifrenizi yazın.", ToastLength.Long).Show();
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        private void SifremiUnuttum_Click(object sender, EventArgs e)
        {
            //StartActivity(typeof(SifremiUnuttumMailGonder));
        }


        #region DTOS
        public class LoginRoot
        {
            public string password { get; set; }
            public bool rememberMe { get; set; }
            public string username { get; set; }
        }


        #region Facebook Login DTO
        public class FacebookEmail
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
            public string Email { get; set; }
            public Picture Picture { get; set; }
        }
        public class Picture
        {
            public Data Data { get; set; }
        }
        public class Data
        {
            public string Height { get; set; }
            public string Is_Silhouette { get; set; }
            public string Url { get; set; }
            public string Width { get; set; }
        }
        #endregion

        #endregion
    }
}