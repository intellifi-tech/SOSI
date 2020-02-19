using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.IO;
using Newtonsoft.Json;
using Refractored.Controls;
using SOSI.DataBasee;
using SOSI.GenericClass;
using static SOSI.GenericClass.Contento_Helpers.Contento_HelperClasses;
using static SOSI.YeniSablonOlustur.YeniSablonOlusturBaseActivity;

namespace SOSI.YeniSablonOlustur
{
    class PaylasimSayisiDialogFragment : Android.Support.V7.App.AppCompatDialogFragment
    {
        #region Tanimlamlar
        Button DevamEt;
        CardView PlusCard, GoldCard, PlatinumCard;
        CircleImageView IsletmeLogo;
        #endregion
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation3;
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, DPX.dpToPx(this.Activity, 460));
            Dialog.Window.SetGravity(GravityFlags.FillHorizontal | GravityFlags.CenterHorizontal | GravityFlags.CenterVertical);

        }
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = base.OnCreateDialog(savedInstanceState);
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            return dialog;
        }
       
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PaylasimSayisiSecDialogFragment, container, false);
            //view.FindViewById<RelativeLayout>(Resource.Id.rootView).ClipToOutline = true;
            DevamEt = view.FindViewById<Button>(Resource.Id.button1);
            DevamEt.Click += DevamEt_Click;
            PlusCard= view.FindViewById<CardView>(Resource.Id.card_view1);
            GoldCard = view.FindViewById<CardView>(Resource.Id.card_view2);
            PlatinumCard = view.FindViewById<CardView>(Resource.Id.card_view3);
            IsletmeLogo = view.FindViewById<CircleImageView>(Resource.Id.profile_image);
            PlusCard.Tag = 0;
            GoldCard.Tag = 1;
            PlatinumCard.Tag = 2;

            PlusCard.Click += PlusCard_Click;
            GoldCard.Click += PlusCard_Click;
            PlatinumCard.Click += PlusCard_Click;
            MarginleriSifirla(1);
            var CompanyInfo = DataBase.COMPANY_INFORMATION_GETIR()[0];
            new SetImageHelper().SetImage(this.Activity, IsletmeLogo, CompanyInfo.logoPath);
            //DataBase.YUKLENECEK_SABLON_TEMIZLE();
            if (ContextCompat.CheckSelfPermission(this.Activity, Android.Manifest.Permission.ReadExternalStorage) == Permission.Granted
              && ContextCompat.CheckSelfPermission(this.Activity, Android.Manifest.Permission.WriteExternalStorage) == Permission.Granted)
            {
                var DevamEdenSablonVarmi = DataBase.YUKLENECEK_SABLON_GETIR();
                if (DevamEdenSablonVarmi.Count > 0)
                {
                    YuklenecekMediaCountHelper.Countt = DevamEdenSablonVarmi[0].maxMediaCount;
                    this.Activity.StartActivity(typeof(YeniSablonOlusturBaseActivity));
                }
            }
            return view;
        }
        
        private void PlusCard_Click(object sender, EventArgs e)
        {
            MarginleriSifirla((int)((CardView)sender).Tag);
        }

        int SecilenPaketTag = 1;
        private void DevamEt_Click(object sender, EventArgs e)
        {

            if (ContextCompat.CheckSelfPermission(this.Activity, Android.Manifest.Permission.ReadExternalStorage) == Permission.Granted
              && ContextCompat.CheckSelfPermission(this.Activity, Android.Manifest.Permission.WriteExternalStorage) == Permission.Granted)
            {

                YuklemeSayfasiniAc();
            }
            else
            {
                RequestPermissions(new String[] {Android.Manifest.Permission.ReadExternalStorage, Android.Manifest.Permission.WriteExternalStorage }, 111);
            }
        }
        void YuklemeSayfasiniAc()
        {
            var DevamEdenSablonVarmi = DataBase.YUKLENECEK_SABLON_GETIR();
            if (DevamEdenSablonVarmi.Count > 0)
            {
                YuklenecekMediaCountHelper.Countt = DevamEdenSablonVarmi[0].maxMediaCount;
                this.Activity.StartActivity(typeof(YeniSablonOlusturBaseActivity));
            }
            else
            {
                if (SecilenPaketTag == 0)
                {
                    YuklenecekMediaCountHelper.Countt = 10;
                    this.Activity.StartActivity(typeof(YeniSablonOlusturBaseActivity));
                }
                else if (SecilenPaketTag == 1)
                {
                    YuklenecekMediaCountHelper.Countt = 15;
                    this.Activity.StartActivity(typeof(YeniSablonOlusturBaseActivity));
                }
                else
                {
                    YuklenecekMediaCountHelper.Countt = 25;
                    this.Activity.StartActivity(typeof(YeniSablonOlusturBaseActivity));
                }
            }
        }

        void MarginleriSifirla(int Tagg)
        {
            SecilenPaketTag = Tagg;

            var UcMargin = DPX.dpToPx(this.Activity, 3);
            var OnMargin = DPX.dpToPx(this.Activity, 20);

            var PlusParams = ((LinearLayout.LayoutParams)PlusCard.LayoutParameters);
            PlusParams.SetMargins(UcMargin, OnMargin, UcMargin, UcMargin);

            var GoldParams = ((LinearLayout.LayoutParams)GoldCard.LayoutParameters);
            GoldParams.SetMargins(UcMargin, OnMargin, UcMargin, UcMargin);

            var PlatinumParams = ((LinearLayout.LayoutParams)PlatinumCard.LayoutParameters);
            PlatinumParams.SetMargins(UcMargin, OnMargin, UcMargin, UcMargin);

            switch (Tagg)
            {
                case 0:
                    PlusParams.SetMargins(UcMargin, UcMargin, UcMargin, 0);
                    break;
                case 1:
                    GoldParams.SetMargins(UcMargin, UcMargin, UcMargin, 0);
                    break;
                case 2:
                    PlatinumParams.SetMargins(UcMargin, UcMargin, UcMargin, 0);
                    break;
                default:
                    break;
            }

            ((ViewGroup)PlusCard).LayoutTransition.EnableTransitionType(Android.Animation.LayoutTransitionType.Changing);
            ((ViewGroup)GoldCard).LayoutTransition.EnableTransitionType(Android.Animation.LayoutTransitionType.Changing);
            ((ViewGroup)PlatinumCard).LayoutTransition.EnableTransitionType(Android.Animation.LayoutTransitionType.Changing);
            PlusCard.LayoutParameters = PlusParams;
            GoldCard.LayoutParameters = GoldParams;
            PlatinumCard.LayoutParameters = PlatinumParams;
        }
    }
}