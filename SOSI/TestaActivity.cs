using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Net.ArcanaStudio.ColorPicker;

namespace SOSI
{
    [Activity(Label = "TestaActivity",MainLauncher =false)]
    public class TestaActivity : Android.Support.V7.App.AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
          //  Window.SetFormat(Format.Rgba8888);

            SetContentView(Resource.Layout.TestActivity);

            //var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            //var initialColor = new Color(prefs.GetInt("color_3", Color.Black.ToArgb()));

            //_colorPickerView = FindViewById<ColorPickerView>(Resource.Id.cpv_color_picker_view);
            //var colorPanelView = FindViewById<ColorPanelView>(Resource.Id.cpv_color_panel_old);
            //_newColorPanelView = FindViewById<ColorPanelView>(Resource.Id.cpv_color_panel_new);

            ////var btnOK = FindViewById<Button>(Resource.Id.okButton);
            ////var btnCancel = FindViewById<Button>(Resource.Id.cancelButton);

            //((LinearLayout)colorPanelView.Parent).SetPadding(_colorPickerView.PaddingLeft, 0,
            //    _colorPickerView.PaddingRight, 0);

            //_colorPickerView.SetOnColorChangedListener(this);
            //_colorPickerView.SetColor(initialColor, true);
            //colorPanelView.SetColor(initialColor);

            ////btnOK.SetOnClickListener(this);
            ////btnCancel.SetOnClickListener(this);
        }
    }
}