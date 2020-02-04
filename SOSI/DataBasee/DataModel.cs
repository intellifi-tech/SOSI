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
}