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

namespace SOSI.GenericClass.Contento_Helpers
{
    public class Contento_HelperClasses
    {
        public class SetImageHelper
        {
            public void SetImage(Activity mContext, ImageView GelenView, string URLL, bool isCenterInsade = false, int PlaceHolderAndErrorImages = Resource.Mipmap.ic_launcher, bool setResize = false)
            {
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
                        options.Override(200, 200);
                    }
                    if (isCenterInsade)
                    {
                        options.CenterInside();
                    }

                    mContext.RunOnUiThread(delegate ()
                    {
                        Glide.With(mContext)
                                 .Load("http://46.45.185.15:9003/images/" + URLL)
                                 .Apply(options).Into(GelenView);
                    });
                }
                catch
                {
                }
            }
        }
    }
}