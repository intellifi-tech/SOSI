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
using Xamarin.Auth;
using static SOSI.GirisKayit.HesapOlusturActivity;

namespace SOSI.GirisKayit
{
    [Activity(Label = "Contento")]
    public class GirisBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        #region Tanimlamalar
        TextView SifremiUnuttum,UyeOlText;
        EditText MailText, SifreText;
        Button GirisButton;
        TextView FacebookLoginButton;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GirisBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.Trans(this,true);
            SifremiUnuttum = FindViewById<TextView>(Resource.Id.textView5);
            UyeOlText = FindViewById<TextView>(Resource.Id.textView6);
            FacebookLoginButton = FindViewById<TextView>(Resource.Id.facebookloginbutton);
            MailText = FindViewById<EditText>(Resource.Id.editText1);
            SifreText = FindViewById<EditText>(Resource.Id.editText2);
            GirisButton = FindViewById<Button>(Resource.Id.button1);
            GirisButton.Click += GirisButton_Click;
            SifremiUnuttum.Click += SifremiUnuttum_Click;
            UyeOlText.Click += UyeOlText_Click;
            FacebookLoginButton.Click += FacebookLoginButton_Click;
            //MailText.Text = "mesut1@intellifi.tech";
            //SifreText.Text = "qwer1234";
        }
        #region Facebook Login

        
        private void FacebookLoginButton_Click(object sender, EventArgs e)
        {
            var auth = new OAuth2Authenticator(
             clientId: "140942637360381",
             scope: "email",
             authorizeUrl: new System.Uri("https://m.facebook.com/dialog/oauth/"),
             redirectUrl: new System.Uri("https://www.facebook.com/connect/login_success.html"));
            auth.Completed += FacebookAuth_CompletedAsync;
            var ui = auth.GetUI(this);
            StartActivity(ui);
        }
        FacebookEmail facebookEmail = null;
        private void FacebookAuth_CompletedAsync(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                //ShowLoading.Show(this, setColorFilterr: true);
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    var authenticator = sender as OAuth2Authenticator;
                    if (authenticator.AuthorizeUrl.Host == "m.facebook.com")
                    {
                        WebService webService = new WebService();
                        var FacebookDonus = webService.OkuGetir($"https://graph.facebook.com/me?fields=id,name,first_name,last_name,email,picture.type(large)&access_token=" + e.Account.Properties["access_token"], true, true);
                        if (FacebookDonus != null)
                        {
                            var Durum = FacebookDonus.ToString();
                            facebookEmail = JsonConvert.DeserializeObject<FacebookEmail>(FacebookDonus);

                            #region FaceBook Login With Out API
                            string Ad = "", Soyad = "", email, sifre;
                            var parcala = facebookEmail.Name.Split(' ');
                            for (int i = 0; i < parcala.Length; i++)
                            {
                                if (i == 0)
                                {
                                    Ad = parcala[0];
                                }
                                else
                                {
                                    Soyad += parcala[1];
                                }
                            }
                            email = facebookEmail.Email;
                            sifre = facebookEmail.Id;

                            if (GirisYapmayiDene(email,sifre))//Önce giriş yapmayı dene
                            {
                                this.StartActivity(typeof(Splash));
                                this.Finish();
                            }
                            else//Yapamazsan Kayit olmayı dene
                            {
                                if (KayitOl(email, sifre, Ad, Soyad))//Kayıt olursan tekrar giriş yapmayı dene
                                {
                                    if (GirisYapmayiDene(email, sifre))
                                    {
                                        this.StartActivity(typeof(Splash));
                                        this.Finish();
                                    } 
                                }
                            }
                            #endregion
                        }
                    }
                })).Start();
            }
        }
        bool GirisYapmayiDene(string email, string sifre)
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
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        bool KayitOl(string email, string sifre,string Ad,string Soyad)
        {
            WebService webService = new WebService();
            KayitIcinRoot kayitIcinRoot = new KayitIcinRoot()
            {
                firstName = Ad,
                lastName = Soyad,
                password = sifre,
                login = email,
                email = email,
                authorities = new List<string>() { "ROLE_USER" }
            };
            string jsonString = JsonConvert.SerializeObject(kayitIcinRoot);
            var Responsee = webService.ServisIslem("register", jsonString, true);
            if (Responsee != "Hata")
            {
                return true;
            }
            else
            {
                AlertHelper.AlertGoster("Bir sorunla karşılaşıldı!", this);
                return false;
            }
        }

        #endregion
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
                GetCompanyInfo(Icerik.id);
                return true;
            }
            else
            {
                ShowLoading.Hide();

                this.RunOnUiThread(delegate {
                    Toast.MakeText(this, "Giriş Yapılamadı.", ToastLength.Long).Show();
                });
                //AlertHelper.AlertGoster("Giriş Yapılamadı!", this);
                return false;
            }
        }

        void GetCompanyInfo(string UserID)
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("company-informations/user/" + UserID);
            if (Donus!=null)
            {
                var aaa = Donus.ToString();
                var Icerik = Newtonsoft.Json.JsonConvert.DeserializeObject<COMPANY_INFORMATION>(Donus.ToString());
                if (Icerik!=null)
                {
                    DataBase.COMPANY_INFORMATION_TEMIZLE();
                    DataBase.COMPANY_INFORMATION_EKLE(Icerik);
                }
            }
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



        #region CompanyInfo
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class PaymentHistoryDTOList
        {
            public DateTime date { get; set; }
            public string id { get; set; }
            public string packageId { get; set; }
            public string packageName { get; set; }
            public string userId { get; set; }
            public string userName { get; set; }
        }

        public class Root
        {
            public string companyColor { get; set; }
            public string id { get; set; }
            public string logoPath { get; set; }
            public string name { get; set; }
            public string other { get; set; }
            //public List<PaymentHistoryDTOList> paymentHistoryDTOList { get; set; }
            public string sectorId { get; set; }
            public string sectorName { get; set; }
            public string serviceAreaId { get; set; }
            public string serviceAreaName { get; set; }
            //public List<TemplateDTOList> templateDTOList { get; set; }
            //public List<TemplateMediaDTOList> templateMediaDTOList { get; set; }
            public string userId { get; set; }
        }


        #endregion



        #endregion
    }
}