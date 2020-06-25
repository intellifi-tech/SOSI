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
using Java.Util;
using Net.ArcanaStudio.ColorPicker;
using Plugin.LocalNotification;
using Plugin.LocalNotifications;
using SOSI.GenericClass;

namespace SOSI
{
    [Activity(Label = "TestaActivity", MainLauncher = false)]
    public class TestaActivity : Android.Support.V7.App.AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TestActivity);
            FindViewById<Button>(Resource.Id.button1).Click += TestaActivity_Click;
         
        }

        private void TestaActivity_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 50; i++)
            {
                Hatirlatma(DateTime.Now.AddSeconds(i*5), i.ToString());
            }
        }

        public void Hatirlatma(DateTime Alarmzamani,string id)
        {
            var alarmManager = this.GetSystemService("alarm").JavaCast<AlarmManager>();
            List<PendingIntent> intentArray = new List<PendingIntent>();
            DateTime AlarmZamani2 = Alarmzamani.AddSeconds(1); //YARIM SAAT ÖNCEDEN HABER VERSİN MK
            DateTime localAlarmTime = new DateTime(AlarmZamani2.Year, AlarmZamani2.Month, AlarmZamani2.Day, AlarmZamani2.Hour, AlarmZamani2.Minute, AlarmZamani2.Second, DateTimeKind.Local);
            var utcAlarmTime = TimeZoneInfo.ConvertTimeToUtc(localAlarmTime);
            var t = new DateTime(1970, 1, 1) - DateTime.MinValue;
            var epochDifferenceInSeconds = t.TotalSeconds;
            var utcAlarmTimeInMillis = utcAlarmTime.AddSeconds(-epochDifferenceInSeconds).Ticks / 10000;
            Intent intent = new Intent(this, typeof(BootReceiver));
            intent.PutExtra("id", id);
            var pending = PendingIntent.GetBroadcast(this, Convert.ToInt32(id), intent, PendingIntentFlags.UpdateCurrent);
            alarmManager.Set(AlarmType.RtcWakeup, utcAlarmTimeInMillis, pending);
            intentArray.Add(pending);
        }


        //public string Create(string title, string message, DateTime scheduleDate, Dictionary<string, string> extraInfo)
        //{
        //    // Create the unique identifier for this notifications.
        //    var notificationId = Guid.NewGuid().ToString();


        //    // Create the alarm intent to be called when the alarm triggers. Make sure
        //    // to add the id so we can find it later if the user wants to update or
        //    // cancel.
        //    var alarmIntent = new Intent(Application.Context, typeof(NotificationAlarmHandler));
        //    alarmIntent.SetAction(BuildActionName(notificationId));
        //    alarmIntent.PutExtra(TitleExtrasKey, title);
        //    alarmIntent.PutExtra(MessageExtrasKey, message);


        //    // Add the alarm intent to the pending intent.
        //    var pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);


        //    // Figure out the alaram in milliseconds.
        //    var utcTime = TimeZoneInfo.ConvertTimeToUtc(scheduleDate);
        //    var epochDif = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
        //    var notifyTimeInInMilliseconds = utcTime.AddSeconds(-epochDif).Ticks / 10000;


        //    // Set the notification.
        //    var alarmManager = Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
        //    alarmManager?.Set(AlarmType.RtcWakeup, notifyTimeInInMilliseconds, pendingIntent);

        //    // All done.
        //    return notificationId;
        //}

    }
}
