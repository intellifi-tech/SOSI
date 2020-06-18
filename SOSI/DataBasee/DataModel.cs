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
using SQLite;

namespace SOSI.DataBasee
{
    class DataModel
    {
        public DataModel()
        {

        }
    }
    public class MEMBER_DATA
    {
        [PrimaryKey, AutoIncrement]
        public int localid { get; set; }
        public bool activated { get; set; }
        //public List<string> authorities { get; set; }
        public DateTime? birthday { get; set; }
        public string cityId { get; set; }
        public string cityName { get; set; }
        public int? coin { get; set; }
        public string countryId { get; set; }
        public string countryName { get; set; }
        public string countryCode { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string gender { get; set; }
        public string about { get; set; }
        public string id { get; set; }
        public string imageUrl { get; set; }
        public string langKey { get; set; }
        public string lastModifiedBy { get; set; }
        public DateTime? lastModifiedDate { get; set; }
        public string lastName { get; set; }
        public string login { get; set; }
        public string firebaseToken { get; set; }
        //------------------------------------
        public string API_TOKEN { get; set; }
        public string password { get; set; }
    }

    public class COMPANY_INFORMATION
    {
        [PrimaryKey, AutoIncrement]
        public int localid { get; set; }
        public string companyColor { get; set; }
        public string id { get; set; }
        public string logoPath { get; set; }
        public string name { get; set; }
        public string sectorId { get; set; }
        public string serviceAreaId { get; set; }
        public string other { get; set; }
        public string userId { get; set; }
    }

    public class YUKLENECEK_SABLON
    {
        [PrimaryKey, AutoIncrement]
        public int localid { get; set; }
        public string MediaUri { get; set; }
        public bool isVideo { get; set; }
        public bool isUploaded { get; set; }
        public int position { get; set; }
        public int maxMediaCount { get; set; }
        public string aciklama { get; set; }
    }

    public class GUNCEL_SABLON
    {
        [PrimaryKey, AutoIncrement]
        public int localid { get; set; }
        public bool complete { get; set; }
        public string id { get; set; }
        public int mediaCount { get; set; }
        public string userId { get; set; }
    }

    public class ODEME_GECMISI
    {
        [PrimaryKey, AutoIncrement]
        public int localid { get; set; }
        public string iyzicoReferanceCode { get; set; }
        public string UzakDB_ID { get; set; }
    }
}