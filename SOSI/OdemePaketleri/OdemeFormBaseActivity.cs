
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Model.V2;
using Iyzipay.Model.V2.Subscription;
using Iyzipay.Request.V2.Subscription;
using Java.Sql;
using Newtonsoft.Json;
using SOSI.DataBasee;
using SOSI.GenericClass;
using SOSI.GenericUI;
using SOSI.WebServicee;
using static SOSI.GenericClass.Contento_Helpers.Contento_HelperClasses;
using static SOSI.OdemePaketleri.OdemePaketleriBaseActivity;

namespace SOSI.OdemePaketleri
{
    [Activity(Label = "Contento")]
    public class OdemeFormBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        //protected Options options;
        EditText KartNumarasiText, KartuzerindekiIsim, CVC,SKT;
        TextView Pre_KarNumarasi, Pre_KartuzerindekiIsim, Pre_SonKullanim;
        ImageView KartTipImg;
        Button OdemeYapButton;
        List<PaketlerUzakDBdto> PaketlerUzakDBdto1 = new List<PaketlerUzakDBdto>();
        ResponseData<SubscriptionCreatedResource> response;
        ImageButton Geri;
        public enum CreditCardTypeType
        {
            Visa,
            MasterCard,
            Discover,
            Amex,
            Switch,
            Solo
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OdemeFormBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            //Initializee();
            CVC = FindViewById<EditText>(Resource.Id.cvc);
            SKT = FindViewById<EditText>(Resource.Id.skt);
            KartTipImg = FindViewById<ImageView>(Resource.Id.cardtypee);
            Geri = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            KartNumarasiText = FindViewById<EditText>(Resource.Id.cardnumberr);
            Pre_KarNumarasi = FindViewById<TextView>(Resource.Id.textView2);
            OdemeYapButton = FindViewById<Button>(Resource.Id.button1);
            KartuzerindekiIsim = FindViewById<EditText>(Resource.Id.cardnamee);
            Pre_KartuzerindekiIsim = FindViewById<TextView>(Resource.Id.textView3);
            Pre_SonKullanim = FindViewById<TextView>(Resource.Id.textView6);
            KartuzerindekiIsim.TextChanged += KartuzerindekiIsim_TextChanged;
            KartNumarasiText.TextChanged += KartNumarasiText_TextChanged;
            SKT.TextChanged += SKT_TextChanged;
            OdemeYapButton.Click += OdemeYapButton_Click;
            Geri.Click += Geri_Click;
            Pre_KarNumarasi.Text = "XXXX XXXX XXXX XXXX";
            Pre_KartuzerindekiIsim.Text = "xxxx xxxx";
            Pre_SonKullanim.Text = "xx / xx";

        }

        private void Geri_Click(object sender, EventArgs e)
        {
            this.Finish();
        }

        private void OdemeYapButton_Click(object sender, EventArgs e)
        {
            if (HepsiDolumu())
            {
                Should_Initialize_Subscription();
            }
        }
        bool HepsiDolumu()
        {
            if (KartNumarasiText.Text.Trim() == "")
            {
                Toast.MakeText(this, "Lütfen kart numaranızı belirtin", ToastLength.Long).Show();
                return false;
            }
            else if (CVC.Text.Trim() == "")
            {
                Toast.MakeText(this, "Lütfen CVC alanını doldurun", ToastLength.Long).Show();
                return false;
            }
            else if (KartuzerindekiIsim.Text.Trim() == "")
            {
                Toast.MakeText(this, "Lütfen kart üzerindeki ismi belirtin", ToastLength.Long).Show();
                return false;
            }
            else if (SKT.Text.Length<5)
            {
                Toast.MakeText(this, "Lütfen kartın son kullanma tarihini belirtin", ToastLength.Long).Show();
                return false;
            }
            else
            {
                return true;
            }
        }
        private void SKT_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (SKT.Text.Length == 2)
            {
                SKT.Text = SKT.Text + "/";
                SKT.SetSelection(SKT.Text.Length);
            }

            switch (SKT.Text.Length)
            {
                case 0:
                    Pre_SonKullanim.Text = "xx / xx";
                    break;
                case 1:
                    Pre_SonKullanim.Text = SKT.Text+"x / xx";
                    break;
                case 2:
                    Pre_SonKullanim.Text = SKT.Text + " / xx";
                    break;
                case 4:
                    Pre_SonKullanim.Text = SKT.Text + "x";
                    break;
                case 5:
                    Pre_SonKullanim.Text = SKT.Text;
                    break;
                default:
                    break;
            }
        }

        private void KartNumarasiText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                // Pre_KarNumarasi.Text = String.Format("0:0000 0000 0000 0000", (Int64.Parse(KartNumarasiText.Text)));
                Pre_KarNumarasi.Text = SetMask(KartNumarasiText.Text);
                var Tip = KrediKartiniBulma(KartNumarasiText.Text);
                switch (Tip)
                {
                    case "VISA":
                        KartTipImg.SetImageResource(Resource.Mipmap.visa);
                        KartTipImg.Visibility = ViewStates.Visible;
                        break;
                    case "MASTER":
                        KartTipImg.SetImageResource(Resource.Mipmap.master_card);
                        KartTipImg.Visibility = ViewStates.Visible;
                        break;
                    case "AEXPRESS":
                        KartTipImg.SetImageResource(Resource.Mipmap.aex);
                        KartTipImg.Visibility = ViewStates.Visible;
                        break;
                    case "DISCOVERS":
                        KartTipImg.SetImageResource(Resource.Mipmap.discover);
                        KartTipImg.Visibility = ViewStates.Visible;
                        break;
                    case "Maestro":
                        KartTipImg.SetImageResource(Resource.Mipmap.aaestro);
                        KartTipImg.Visibility = ViewStates.Visible;
                        break;
                    case "invalid":
                        KartTipImg.Visibility = ViewStates.Invisible;
                        break;
                    default:
                        break;
                }
            }
            catch
            {
            }
        }

        private void KartuzerindekiIsim_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            Pre_KartuzerindekiIsim.Text = KartuzerindekiIsim.Text.ToUpper();
        }

        string SetMask(string kartN)
        {
            var kartNUzunluk = kartN.Length;
            string Part1 = "", Part2 = "", Part3 = "", Part4 = "";
            for (int i = 0; i < kartNUzunluk; i++)
            {
                if (i >= 0 && i <= 3)
                {
                    Part1 += kartN.Substring(i, 1);
                }
                if (i > 3 && i <= 7)
                {
                    Part2 += kartN.Substring(i, 1);
                }
                if (i > 7 && i <= 11)
                {
                    Part3 += kartN.Substring(i, 1);
                }
                if (i > 11 && i <= 15)
                {
                    Part4 += kartN.Substring(i, 1);
                }
            }
            return Part1 + " " + Part2 + " " + Part3 + " " + Part4;
        }

        public static string KrediKartiniBulma(string KrediKartiNo)
        {
            Regex visaRegex = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");
            Regex masterRegex = new Regex("^5[1-5][0-9]{14}$");
            Regex expressRegex = new Regex("^3[47][0-9]{13}$");
            Regex dinersRegex = new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
            Regex discoverRegex = new Regex("^6(?:011|5[0-9]{2})[0-9]{12}$");
            Regex jcbRegex = new Regex("^(?:2131|1800|35\\d{3})\\d{11}$");
            Regex maestro = new Regex(@"^(?:5[0678]\d\d|6304|6390|67\d\d)\d{8,15}$");

            if (visaRegex.IsMatch(KrediKartiNo))
                return "VISA";
            else if (masterRegex.IsMatch(KrediKartiNo))
                return "MASTER";
            else if (expressRegex.IsMatch(KrediKartiNo))
                return "AEXPRESS";
            else if (discoverRegex.IsMatch(KrediKartiNo))
                return "DISCOVERS";
            else if (maestro.IsMatch(KrediKartiNo))
                return "Maestro";
            else
                return "invalid";
        }


        #region Iyzico

        //public void Initializee()
        //{
        //    options = new Options();
        //    options.ApiKey = "sandbox-S8fBp3d3O6g2v4iLlweEymY7jRkFBQnV";
        //    options.SecretKey = "sandbox-trdXadVcZmdSN8GFnf6Cmb5pzGr8JIYE";
        //    options.BaseUrl = "https://sandbox-api.iyzipay.com";
        //}

        public void Should_Create_Customer()
        {
            string randomString = $"{DateTime.Now:yyyyMMddHHmmssfff}";
            CreateCustomerRequest createCustomerRequest = new CreateCustomerRequest
            {
                Email = $"iyzico-{randomString}@iyzico.com",
                Locale = Locale.TR.ToString(),
                Name = "customer-name",
                Surname = "customer-surname",
                BillingAddress = new Address
                {
                    City = "İstanbul",
                    Country = "Türkiye",
                    Description = "billing-address-description",
                    ContactName = "billing-contact-name",
                    ZipCode = "010101"
                },
                ShippingAddress = new Address
                {
                    City = "İstanbul",
                    Country = "Türkiye",
                    Description = "shipping-address-description",
                    ContactName = "shipping-contact-name",
                    ZipCode = "010102"
                },
                ConversationId = "123456789",
                GsmNumber = "+905350000000",
                IdentityNumber = "55555555555"
            };

            ResponseData<CustomerResource> response = Customer.Create(createCustomerRequest, Contento_Resources_Helper.options);
            if (response.Status == "success")
            {

            }
        }

        public void Should_Initialize_Subscription()
        {
            var meData = DataBase.MEMBER_DATA_GETIR()[0];
            var ad = "";
            var soyad = "";
            var ay = "";
            var yil = "";

            var ayyilbol = SKT.Text.Split("/");
            ay = ayyilbol[0];
            yil = "20"+ ayyilbol[1];

            var Bol = KartuzerindekiIsim.Text.Split(" ");
            for (int i = 0; i < Bol.Length; i++)
            {
                if (i==Bol.Length-1)
                {
                    soyad = Bol[i];
                }
                else
                {
                    ad = ad + Bol[i];
                }
            }

            if (ad =="")
            {
                ad = soyad;
            }
          
            // string randomString = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            SubscriptionInitializeRequest request = new SubscriptionInitializeRequest
            {
                Locale = Locale.TR.ToString(),
                Customer = new CheckoutFormCustomer
                {
                    Email = meData.email,
                    Name = ad,
                    Surname = soyad,
                    BillingAddress = new Address
                    {
                        City = "İstanbul",
                        Country = "Türkiye",
                        Description = "billing-address-description",
                        ContactName = "billing-contact-name",
                        ZipCode = "010101"
                    },
                    ShippingAddress = new Address
                    {
                        City = "İstanbul",
                        Country = "Türkiye",
                        Description = "shipping-address-description",
                        ContactName = "shipping-contact-name",
                        ZipCode = "010102"
                    },

                    GsmNumber = "+905415670793",
                    IdentityNumber = meData.id.ToString()
                },
                PaymentCard = new CardInfo
                {
                    CardNumber = KartNumarasiText.Text.Trim(),
                    CardHolderName = KartuzerindekiIsim.Text.Trim(),
                    ExpireMonth = ay,
                    ExpireYear = yil,
                    Cvc = CVC.Text.Trim(),
                    RegisterConsumerCard = true
                },
                ConversationId = "123456789",
                PricingPlanReferenceCode = OdemePaketleriBaseActivity_Helper.PricingPlanReferenceCode
            };

            response = Subscription.Initialize(request, Contento_Resources_Helper.options);
            var a = response;

            if (response.StatusCode != 200)
            {
                Toast.MakeText(this, response.ErrorMessage, ToastLength.Long).Show();
                //AlertHelper.AlertGoster(response.ErrorMessage, this);
            }
            else
            {
                ShowLoading.Show(this);
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    OdemeGecmisiOlustur();
                })).Start();
                
            }
        }
        #endregion

        void OdemeGecmisiOlustur()
        {
            WebService webService = new WebService();
            var JSONData = webService.OkuGetir("package-tariffs");
            if (JSONData != null)
            {
                var JsonSting = JSONData.ToString();
                PaketlerUzakDBdto1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PaketlerUzakDBdto>>(JSONData.ToString());
                GemiseEkle();
            }
        }    


        void GemiseEkle()
        {
            var MeData = DataBase.MEMBER_DATA_GETIR()[0];
            var SecilenPaket = PaketlerUzakDBdto1.FindLast(item => item.name == OdemePaketleriBaseActivity_Helper.PackageName);
            if (SecilenPaket!=null)
            {
                WebService webService = new WebService();
                OdemeGecmisiDTO OdemeGecmisiDTO1 = new OdemeGecmisiDTO() { 
                    date = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ssZ"),
                    packageId= SecilenPaket.id,
                    packageName= SecilenPaket.name,
                    userId = MeData.id,
                    userName= MeData.firstName + MeData.lastName
                };
                var jsonstringg = JsonConvert.SerializeObject(OdemeGecmisiDTO1);
                var Donus = webService.ServisIslem("payment-histories", jsonstringg);
                if (Donus!="Hata")
                {
                    var EklenenKayit = Newtonsoft.Json.JsonConvert.DeserializeObject<OdemeGecmisiDTO>(Donus.ToString());
                    DataBase.ODEME_GECMISI_EKLE(new ODEME_GECMISI() { 
                        iyzicoReferanceCode = response.Data.ReferenceCode,
                        UzakDB_ID= EklenenKayit.id
                    });
                    this.RunOnUiThread(delegate () {
                        ShowLoading.Hide();
                        Toast.MakeText(this, "Aboneliğiniz Başlatıldı", ToastLength.Long).Show();
                        OdemePaketleriBaseActivity_Helper.OdemePaketleriBaseActivity1.Finish();
                        this.Finish();
                    });
                }
            }
        }

        public class OdemeGecmisiDTO
        {
            public string date { get; set; }
            public string id { get; set; }
            public string packageId { get; set; }
            public string packageName { get; set; }
            public string userId { get; set; }
            public string userName { get; set; }
            //public string iyzicoReferanceCode { get; set; }

        }


        public class PaketlerUzakDBdto
        {
            public string id { get; set; }
            public int mediaCount { get; set; }
            public int mountCount { get; set; }
            public string name { get; set; }
            public int price { get; set; }
            public int reviseCount { get; set; }
        }
    }
}