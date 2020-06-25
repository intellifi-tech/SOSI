using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SOSI.Splashh;

namespace SOSI.GenericClass
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootReceiver : BroadcastReceiver
    {
        const string TAG = "MyFirebaseMsgService";
        static readonly string CHANNEL_ID = "messages_notification";
        NotificationManager notificationManager = null;
        public static string codee { get; set; }
        //the interval currently every one minute
        //to set it to dayly change the value to 24 * 60 * 60 * 1000
        //public static long reminderInterval =  60 * 1000;
        //public static long FirstReminder()
        //{
        //    Java.Util.Calendar calendar = Java.Util.Calendar.Instance;
        //    calendar.Set(Java.Util.CalendarField.HourOfDay, 20);
        //    calendar.Set(Java.Util.CalendarField.Minute, 06);
        //    calendar.Set(Java.Util.CalendarField.Second, 00);
        //    return calendar.TimeInMillis;
        //}
        public override void OnReceive(Context context, Intent intent)
        {
            //string aaaa = "";
            var id = intent.GetStringExtra("id");
            NOTIFICATION_ID = Convert.ToInt32(id);
            //Toast.MakeText(context, "aaasdasdas", ToastLength.Long).Show();
            CreateNotificationChannel(context);
            SetNotification_OTHER("Contento", "Yeni paylaşım vakti!", context);
        }

        void CreateNotificationChannel(Context context)
        {
            if (notificationManager == null)
            {
                if (Build.VERSION.SdkInt < BuildVersionCodes.O)
                {
                    return;
                }

                var name = "Contento";
                var description = "Contento";
                var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.Default)
                {
                    Description = description
                };
                var alarmAttributes = new AudioAttributes.Builder()
                                 .SetContentType(AudioContentType.Sonification)
                                 .SetUsage(AudioUsageKind.Notification).Build();
                Android.Net.Uri alarmUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
                channel.EnableLights(true);
                channel.EnableVibration(true);
                channel.Importance = NotificationImportance.High;
                channel.SetSound(alarmUri, alarmAttributes);

                notificationManager = (NotificationManager)context.GetSystemService("notification");
                notificationManager.CreateNotificationChannel(channel);
            }
            
        }
        int NOTIFICATION_ID = 3000;
        void SetNotification_OTHER(string MesajTitle, string MesajIcerigi, Context context)
        {
            var resultIntent = new Intent(context, typeof(Splash));

            var stackBuilder = Android.App.TaskStackBuilder.Create(context);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(Splash)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:
            var resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);
            
            // Build the notification:
            var builder = new NotificationCompat.Builder(context, CHANNEL_ID)
                          .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                          .SetContentIntent(resultPendingIntent) // Start up this activity when the user clicks the intent.
                          .SetContentTitle(MesajTitle) // Set the title
                          .SetNumber(30) // Display the count in the Content Info
                          .SetSmallIcon(Resource.Mipmap.ic_launcher)
                          //.SetVibrate(new long[] { 0, 500, 1000 })
                          .SetPriority(NotificationCompat.PriorityHigh)
                          //.SetSound(uri)
                          .SetContentText(MesajIcerigi); // the message to display.

            //// Instantiate the Big Text style:
            //NotificationCompat.BigTextStyle textStyle = new NotificationCompat.BigTextStyle();

            //// Fill it with text:
            //textStyle.BigText(MesajIcerigi);

            //// Set the summary text:
            //textStyle.SetSummaryText(MesajIcerigi);

            //// Plug this style into the builder:
            //builder.SetStyle(textStyle);


            var notificationManager = NotificationManagerCompat.From(context);
            var newid = NOTIFICATION_ID++;
            notificationManager.Notify(newid, builder.Build());
        }
    }
}