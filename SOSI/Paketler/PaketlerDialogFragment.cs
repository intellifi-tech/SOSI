using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Cooltechworks.Creditcarddesign;
using Java.IO;
using Newtonsoft.Json;
using SOSI.GenericClass;

namespace SOSI.Paketler
{
    class PaketlerDialogFragment : Android.Support.V7.App.AppCompatDialogFragment
    {
        #region Tanimlamlar
        Button DevamEt;
        CardView PlusCard, GoldCard, PlatinumCard;

        private int CREATE_NEW_CARD = 0;
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
            View view = inflater.Inflate(Resource.Layout.PaketlerDialogFragment, container, false);
            //view.FindViewById<RelativeLayout>(Resource.Id.rootView).ClipToOutline = true;
            DevamEt = view.FindViewById<Button>(Resource.Id.button1);
            DevamEt.Click += DevamEt_Click;
            PlusCard= view.FindViewById<CardView>(Resource.Id.card_view1);
            GoldCard = view.FindViewById<CardView>(Resource.Id.card_view2);
            PlatinumCard = view.FindViewById<CardView>(Resource.Id.card_view3);
            PlusCard.Tag = 0;
            GoldCard.Tag = 1;
            PlatinumCard.Tag = 2;

            PlusCard.Click += PlusCard_Click;
            GoldCard.Click += PlusCard_Click;
            PlatinumCard.Click += PlusCard_Click;
            MarginleriSifirla(1);
            return view;
        }

        private void PlusCard_Click(object sender, EventArgs e)
        {
            MarginleriSifirla((int)((CardView)sender).Tag);
        }

        int SecilenPaketTag = 1;
        private void DevamEt_Click(object sender, EventArgs e)
        {
            if (SecilenPaketTag == 0)
            {
                Intent intent = new Intent(this.Activity, typeof(CardEditActivity));
                this.Activity.StartActivityForResult(intent, CREATE_NEW_CARD);
            }
            else if (SecilenPaketTag == 1)
            {
            }
            else
            {
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