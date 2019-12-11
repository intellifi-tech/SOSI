using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;

namespace SOSI.GenericClass
{
    [Register("com.SOSI.android.VerticalTextView")]
    public class VerticalTextView : Android.Support.V7.Widget.AppCompatTextView
    {
        private int _width, _height;
        private Rect _bounds = new Rect();
        public VerticalTextView(Context context) : base(context)
        {
            Initialize();
        }

        public VerticalTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public VerticalTextView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }

        private void Initialize()
        {
           
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(heightMeasureSpec, widthMeasureSpec);
            // vise versa
            _height = this.MeasuredWidth;
            _width = this.MeasuredHeight;
            SetMeasuredDimension(_width, _height);
        }

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            canvas.Save();

            canvas.Translate(_width, _height);
            canvas.Rotate(-90);

            TextPaint paint = this.Paint;
            //Android.Graphics.Color Colorr = Color.  //this.TextColors.DefaultColor;
            //
            //var defcolor = this.TextColors.DefaultColor;
            //var Redd = Color.GetRedComponent(defcolor);
            //var Blue = Color.GetBlueComponent(defcolor);
            //var Greenn = Color.GetGreenComponent(defcolor);
            //var Birlestir = Color.Rgb(Redd, Greenn, Blue);
            //ColorFilter cf = new PorterDuffColorFilter(Birlestir, PorterDuff.Mode.SrcAtop);
            //paint.SetColorFilter(cf);
            paint.Color = Color.White;
            String text = getTextt();
            
            paint.GetTextBounds(text, 0, text.Length, _bounds);
            canvas.DrawText(text, this.CompoundPaddingLeft, (_bounds.Height() - _width) / 2, paint);

            canvas.Restore();
        }
        private String getTextt()
        {
            return this.Text.ToString();
        }
    }
}