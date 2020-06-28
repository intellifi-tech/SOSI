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
using SOSI.GenericClass;
using SOSI.WebServicee;
using static SOSI.TamamlanmisSablonlar.SablonIcerikleri.SablonIcerikleriBaseActivity;

namespace SOSI.TamamlanmisSablonlar.SablonDetay
{
    [Activity(Label = "SOSI")]
    public class RevizeGonderBaseActivity : Android.Support.V7.App.AppCompatActivity
    {
        ImageButton Geri;
        EditText RevizeMetin;
        Button GonderButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RevizeGonderBaseActivity);
            DinamikStatusBarColor dinamikStatusBarColor = new DinamikStatusBarColor();
            dinamikStatusBarColor.SetFullScreen(this);
            Geri = FindViewById<ImageButton>(Resource.Id.ımageButton1);
            RevizeMetin = FindViewById<EditText>(Resource.Id.editText1);
            GonderButton = FindViewById<Button>(Resource.Id.button1);
            GonderButton.Click += GonderButton_Click;
            Geri.Click += Geri_Click;
        }

        private void GonderButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RevizeMetin.Text.Trim()))
            {
                WebService webService = new WebService();
                RevizeGonderDTO RevizeGonderDTO1 = new RevizeGonderDTO()
                {
                    descriptionText = RevizeMetin.Text,
                    imageId = SecilenSablonDTO.SecilenSablon.id,
                    templateId = SecilenSablon.SablonID,
                };
                string jsonString = JsonConvert.SerializeObject(RevizeGonderDTO1);
                var Donus = webService.ServisIslem("revises", jsonString);
                if (Donus!="Hata")
                {
                    Toast.MakeText(this, "Revizeniz iletildi.", ToastLength.Long).Show();
                    this.Finish();
                }
            }
        }

        private void Geri_Click(object sender, EventArgs e)
        {
            this.Finish();
        }

        public class RevizeGonderDTO
        {
            public string descriptionText { get; set; }
            public string id { get; set; }
            public string imageId { get; set; }
            public string templateId { get; set; }
            //public TemplateMediaDTO templateMediaDTO { get; set; }
        }
    }
}