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
    class YeniPaylasimMetinDialogFragment : Android.Support.V7.App.AppCompatDialogFragment
    {
        #region Tanimlamlar
        Button DevamEt;
        SablonDTO gelenicerik;
        int positionn;
        YeniSablonOlusturBaseActivity GleenBase1;
        EditText AciklamaText;
        #endregion

        public YeniPaylasimMetinDialogFragment(SablonDTO gelenicerik, int positionn, YeniSablonOlusturBaseActivity GleenBase1)
        {
            this.gelenicerik = gelenicerik;
            this.positionn = positionn;
            this.GleenBase1 = GleenBase1;
        }
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
            View view = inflater.Inflate(Resource.Layout.YeniPaylasimMetinGirisiDialogFragment, container, false);
            //view.FindViewById<RelativeLayout>(Resource.Id.rootView).ClipToOutline = true;
            DevamEt = view.FindViewById<Button>(Resource.Id.button1);
            AciklamaText = view.FindViewById<EditText>(Resource.Id.editText1);
            DevamEt.Click += DevamEt_Click;
            return view;
        }
        private void DevamEt_Click(object sender, EventArgs e)
        {
            GleenBase1.MetinGirildiMedyayiKaydet(gelenicerik, positionn, AciklamaText.Text.Trim());
            this.Dismiss();
        }
    }
}