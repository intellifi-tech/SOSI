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
    class MedyaUploadServiceDialogFragment : Android.Support.V7.App.AppCompatDialogFragment
    {
        #region Tanimlamlar
        YeniSablonOlusturBaseActivity GleenBase1;
        ProgressBar Progressss;
        TextView CurrentCount, MaxCount;
        int MaxCount1;
        #endregion

        public MedyaUploadServiceDialogFragment(YeniSablonOlusturBaseActivity GleenBase1,int MaxCount2)
        {
            this.MaxCount1 = MaxCount2;
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
            View view = inflater.Inflate(Resource.Layout.MediaUploadLoading, container, false);
            Progressss = view.FindViewById<ProgressBar>(Resource.Id.progressBar1);
            CurrentCount = view.FindViewById<TextView>(Resource.Id.textView2);
            MaxCount = view.FindViewById<TextView>(Resource.Id.textView3);
            MaxCount.Text = MaxCount1.ToString();
            Progressss.Max = MaxCount1;

            return view;
        }
     
        public void UploadProgress(int count)
        {
            Progressss.Progress = count;
            CurrentCount.Text = count.ToString();
        }
    }
}