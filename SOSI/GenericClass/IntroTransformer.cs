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
using Android.Support.V4.View;

namespace SOSI.GenericClass
{
    class IntroTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {

        View BaseView;
        public IntroTransformer(View GelenBaseView)
        {
            BaseView = GelenBaseView;
        }
        public void TransformPage(View page, float position)
        {
            int pageWidth = BaseView.Width;
            var BaslikText = page.FindViewById<TextView>(Resource.Id.textView1);
            var AciklamaText = page.FindViewById<TextView>(Resource.Id.textView2);
            var AltAciklama = page.FindViewById<TextView>(Resource.Id.textView3);
            var Imageee = page.FindViewById<ImageView>(Resource.Id.ımageView1);
            var Button = page.FindViewById<Button>(Resource.Id.button1);
            if (position < -1)
            { // [-Infinity,-1)
              // This page is way off-screen to the left.
                BaseView.Alpha = 1f;

            }
            else if (position <= 1)
            { // [-1,1]
                //AciklamaText.setTranslationX(-position * (pageWidth / 2)); //Half the normal speed
                try
                {
                    
                    
                    BaslikText.TranslationX = (position *0.2f) * (pageWidth / 2);
                    AciklamaText.TranslationX = (position * 0.5f) * (pageWidth / 2);
                    AltAciklama.TranslationX = (position * 1f) * (pageWidth / 2);
                    Imageee.TranslationX = (position * 1.5f) * (pageWidth / 2);
                    Button.TranslationX = (position * 2f) * (pageWidth / 2);
                    //Par2.TranslationX = (position * 1.5f) * (pageWidth / 2);
                    //Par3.TranslationX = (position * 2f) * (pageWidth / 2);
                    //Par4.TranslationX = (position * 1f) * (pageWidth / 2);
                }
                catch
                {

                }

            }
            else
            { // (1,+Infinity]
              // This page is way off-screen to the right.
                BaseView.Alpha = 1f;
            }
        }
    }
}