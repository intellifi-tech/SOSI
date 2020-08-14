using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using Newtonsoft.Json;
using SOSI.Splashh;
using SOSI.TamamlanmisSablonlar.SablonDetay;

namespace SOSI
{
    [Service(Name = "co.contentoapp.android.MyFirebaseMessagingService")]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        static readonly string CHANNEL_ID = "messages_notification";
        NotificationManager notificationManager = null;


        public override void OnMessageReceived(RemoteMessage message)
        {
            CreateNotificationChannel(ApplicationContext);
            var GetMessageDTO1 = GetMessageDTO(message.Data);

            SetNotification_OTHER("Contento","Paylaşım Zamanı!", GetMessageDTO1.data.sablonId);
            //if (GetMessageDTO1!=null)
            //{
            //    switch (GetMessageDTO1.data.type)
            //    {
            //        case "NORMAL":
            //            if (!isBlocked(GetMessageDTO1.data.userName))
            //            {
            //                YeniMesajIslemleri(GetMessageDTO1);
            //            }
            //            break;
            //        case "ANONYMOUS":
            //            if (!isBlocked(GetMessageDTO1.data.userName))
            //            {
            //                YeniMesajIslemleri(GetMessageDTO1);
            //            }
            //            break;
            //        case "MACTH":
            //            if (!isBlocked(GetMessageDTO1.data.userName))
            //            {
            //                ShowNewMatchNotification(GetMessageDTO1);
            //            }
            //            break;
            //        case "PROFIL_VIEW":
            //            ShowProfilViewNoti();
            //            break;
            //        case "SEND_GIFT":
            //            GiftNoti();
            //            break;
            //        case "PROFILE_LIKE":
            //            ProfileLikeNoti();
            //            break;
            //    }
            //}
        }

        #region Chat Noti

        GelenMesajDTO GetMessageDTO(IDictionary<string, string> data)
        {
            GelenMesajDTO gelenMesajDTO = new GelenMesajDTO();
            gelenMesajDTO.data = new Data();
            foreach (var key in data.Keys)
            {
                switch (key)
                {
                    case "sablonId":
                        gelenMesajDTO.data.sablonId = data[key];
                        break;
                }
            }

            return gelenMesajDTO;
        }


        public class Data
        {
            public string sablonId { get; set; }
        }
        public class GelenMesajDTO
        {
            public string to { get; set; }
            public Data data { get; set; }
        }
        #endregion



        #region Notification Init
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
        void SetNotification_OTHER(string MesajTitle, string MesajIcerigi,string SablonId)
        {
            var resultIntent = new Intent(this, typeof(SablonDetayBaseActivity));
            resultIntent.PutExtra("sablonID", SablonId);
            var stackBuilder = Android.App.TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(SablonDetayBaseActivity)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:
            var resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);


            // Build the notification:
            var builder = new NotificationCompat.Builder(this, CHANNEL_ID)
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


            var notificationManager = NotificationManagerCompat.From(this);
            var newid = NOTIFICATION_ID++;
            notificationManager.Notify(newid, builder.Build());
        }
        #endregion
    }
}
