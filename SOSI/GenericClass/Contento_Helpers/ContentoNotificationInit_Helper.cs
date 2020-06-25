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
using SOSI.DataBasee;
using SOSI.WebServicee;

namespace SOSI.GenericClass.Contento_Helpers
{
    public class ContentoNotificationInit_Helper
    {
        Context context;
        public ContentoNotificationInit_Helper(Context context1)
        {
            context = context1;
        }
        MEMBER_DATA Me = DataBase.MEMBER_DATA_GETIR()[0];
        public void Init()
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("templates/user/" + Me.id);
            if (Donus != null)
            {
                var TamamlanmisSablonDTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TamamlanmisSablonDTO>>(Donus.ToString());
                if (TamamlanmisSablonDTO1.Count > 0)
                {
                    TamamlanmisSablonDTO1.Reverse();
                    GetMediaResources(TamamlanmisSablonDTO1[0].id);
                }
            }
        }

        void GetMediaResources(string ID)
        {
            WebService webService = new WebService();
            var Donus = webService.OkuGetir("template-medias/template/" + ID);
            if (Donus != null)
            {
                var sablonIcerikleriDTOs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SablonIcerikleriDTO>>(Donus.ToString());
                sablonIcerikleriDTOs = sablonIcerikleriDTOs.FindAll(item => item.processed == true);
                if (sablonIcerikleriDTOs.Count > 0)
                {
                    for (int i = 0; i < sablonIcerikleriDTOs.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(sablonIcerikleriDTOs[i].shareDateTime))
                        {
                            Hatirlatma(Convert.ToDateTime(sablonIcerikleriDTOs[i].shareDateTime), i.ToString());
                        }
                    }
                }
            }
        }

        public void Hatirlatma(DateTime Alarmzamani, string id)
        {
            var alarmManager = context.GetSystemService("alarm").JavaCast<AlarmManager>();
            List<PendingIntent> intentArray = new List<PendingIntent>();
            DateTime AlarmZamani2 = Alarmzamani.AddSeconds(1); //YARIM SAAT ÖNCEDEN HABER VERSİN MK
            DateTime localAlarmTime = new DateTime(AlarmZamani2.Year, AlarmZamani2.Month, AlarmZamani2.Day, AlarmZamani2.Hour, AlarmZamani2.Minute, AlarmZamani2.Second, DateTimeKind.Local);
            var utcAlarmTime = TimeZoneInfo.ConvertTimeToUtc(localAlarmTime);
            var t = new DateTime(1970, 1, 1) - DateTime.MinValue;
            var epochDifferenceInSeconds = t.TotalSeconds;
            var utcAlarmTimeInMillis = utcAlarmTime.AddSeconds(-epochDifferenceInSeconds).Ticks / 10000;
            Intent intent = new Intent(context, typeof(BootReceiver));
            intent.PutExtra("id", id);
            var pending = PendingIntent.GetBroadcast(context, Convert.ToInt32(id), intent, PendingIntentFlags.UpdateCurrent);
            alarmManager.Set(AlarmType.RtcWakeup, utcAlarmTimeInMillis, pending);
            intentArray.Add(pending);
        }

        #region  DTOS
        public class TamamlanmisSablonDTO
        {
            public bool? complete { get; set; }
            public string id { get; set; }
            public int mediaCount { get; set; }
            public string userId { get; set; }
        }


        public class SablonIcerikleriDTO
        {
            public string afterMediaPath { get; set; }
            public string beforeMediaPath { get; set; }
            public string id { get; set; }
            public int mediaCount { get; set; }
            public string postText { get; set; }
            public bool processed { get; set; }
            public string shareDateTime { get; set; }
            public string templateId { get; set; }
            public string type { get; set; }
            public string userId { get; set; }
            public bool video { get; set; }
        }

        #endregion
    }
}