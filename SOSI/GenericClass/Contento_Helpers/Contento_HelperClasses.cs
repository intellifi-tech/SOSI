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
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Request;
using Iyzipay;
using SOSI.DataBasee;

namespace SOSI.GenericClass.Contento_Helpers
{
    public class Contento_HelperClasses
    {
        public class SetImageHelper
        {
            public void SetImage(Activity mContext, ImageView GelenView, string URLL, bool isCenterInsade = false, int PlaceHolderAndErrorImages = Resource.Mipmap.logononbg, bool setResize = false)
            {
                var Me = DataBase.MEMBER_DATA_GETIR()[0];
                try
                {
                    //val file: File = Glide.with(activity).asFile().load(url).submit().get()
                    //val path: String = file.path


                    RequestOptions options = new RequestOptions()
                        .InvokeDiskCacheStrategy(Com.Bumptech.Glide.Load.Engine.DiskCacheStrategy.All)
                                            .CenterCrop()
                                            .Placeholder(PlaceHolderAndErrorImages)
                                            .Error(PlaceHolderAndErrorImages);
                    if (setResize)
                    {
                        options.Override(500, 500);
                    }
                    if (isCenterInsade)
                    {
                        options.CenterInside();
                    }

                    mContext.RunOnUiThread(delegate ()
                    {
                        Glide.With(mContext)
                                 .Load("https://contentoapp.co/app/"+ URLL)
                                 .Apply(options).Into(GelenView);
                    });
                }
                catch
                {
                }
            }
        }

        public static class Contento_Resources_Helper
        {
            public static string PlatinumUrunCode = "ffbf616c-b05a-4da8-99f5-da52a9da650a";
            public static string GoldUrunCode = "7a30471f-d262-467d-aaf2-f34a0be105e5";
            public static string SilverUrunCode = "ac3d918c-d63f-4486-a248-8a9ab05c5246";
            public static Options options = new Options()
            {
                ApiKey = "Gh7cJiCT6yExAmkAYDL6IP5wIDhKWPk7",
                SecretKey= "vUvBeTAvjrm4QJfoFOCy0qbwsWgaSYp5",
                BaseUrl = "https://api.iyzipay.com"
            };
        }
    }
}