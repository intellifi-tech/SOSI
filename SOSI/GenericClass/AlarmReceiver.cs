using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using SOSI.Splashh;

namespace SOSI.GenericClass
{
    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {
        const string TAG = "MyFirebaseMsgService";
        static readonly string CHANNEL_ID = "messages_notification";
        NotificationManager notificationManager = null;
        public override void OnReceive(Context context, Intent intent)
        {
            var title = "Hello world!";
            var message = "Checkout this notification";

            Intent backIntent = new Intent(context, typeof(Splash));
            backIntent.SetFlags(ActivityFlags.NewTask);
            
            //The activity opened when we click the notification is SecondActivity
            //Feel free to change it to you own activity
            var resultIntent = new Intent(context, typeof(Splash));

            PendingIntent pending = PendingIntent.GetActivities(context, 0,
                new Intent[] { backIntent, resultIntent },
                PendingIntentFlags.OneShot);

            //var builder =
            //    new Notification.Builder(context)
            //        .SetContentTitle(title)
            //        .SetContentText(message)
            //        .SetAutoCancel(true)
            //        .SetSmallIcon(Resource.Drawable.Icon)
            //        .SetDefaults(NotificationDefaults.All);

            //builder.SetContentIntent(pending);
            //var notification = builder.Build();
            //var manager = NotificationManager.FromContext(context);
            //manager.Notify(1331, notification);
        


        }

        //void CreateNotificationChannel()
        //{
        //    if (Build.VERSION.SdkInt < BuildVersionCodes.O)
        //    {
        //        // Notification channels are new in API 26 (and not a part of the
        //        // support library). There is no need to create a notification
        //        // channel on older versions of Android.
        //        return;
        //    }

        //    var channelName = "contentonotichannel";
        //    var channelDescription = "contentonotichanneldesc";
        //    var channel = new NotificationChannel(CHANNEL_ID, channelName, NotificationImportance.Default)
        //    {
        //        Description = channelDescription
        //    };

        //    var notificationManager = (NotificationManager)GetSystemService(NotificationService);
        //    notificationManager.CreateNotificationChannel(channel);
        //}
    }
}