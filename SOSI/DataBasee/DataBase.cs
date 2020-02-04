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
using System.IO;
using SQLite;

namespace SOSI.DataBasee
{
    class DataBase
    {
        public DataBase() 
        {
            CreateDataBase();
        }
        public static string documentsFolder()
        {
            string path;
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Directory.CreateDirectory(path);
            return path;
        }
        public static void CreateDataBase()
        {
            var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Contento.db"), false);
            conn.CreateTable<MEMBER_DATA>();
            conn.CreateTable<COMPANY_INFORMATION>();
            conn.Close();
        }

        #region MEMBER_DATA
        public static bool MEMBER_DATA_EKLE(MEMBER_DATA GelenDoluTablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Contento.db"), false);
                conn.Insert(GelenDoluTablo);
                conn.Close();
                return true;
            }
            catch(Exception Ex)
            {
                var aa = Ex.Message;
                return false;
            }
        }
        public static List<MEMBER_DATA> MEMBER_DATA_GETIR()
        {
            Atla:
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Contento.db"), false);
                var gelenler = conn.Query<MEMBER_DATA>("Select * From MEMBER_DATA");
                conn.Close();
                return gelenler;
            }
            catch (Exception Ex)
            {
                goto Atla;
                var aa = Ex.Message;
                return null;
            }
           
        }
        public static bool MEMBER_DATA_TEMIZLE()
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Contento.db"), false);
                conn.Query<MEMBER_DATA>("Delete From MEMBER_DATA");
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        public static bool MEMBER_DATA_Guncelle(MEMBER_DATA Tablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Contento.db"), false);
                conn.Update(Tablo);
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        #endregion

        #region COMPANY_INFORMATION
        public static bool COMPANY_INFORMATION_EKLE(COMPANY_INFORMATION GelenDoluTablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Contento.db"), false);
                conn.Insert(GelenDoluTablo);
                conn.Close();
                return true;
            }
            catch (Exception Ex)
            {
                var aa = Ex.Message;
                return false;
            }
        }
        public static List<COMPANY_INFORMATION> COMPANY_INFORMATION_GETIR()
        {
            Atla:
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Contento.db"), false);
                var gelenler = conn.Query<COMPANY_INFORMATION>("Select * From COMPANY_INFORMATION");
                conn.Close();
                return gelenler;
            }
            catch (Exception Ex)
            {
                goto Atla;
                var aa = Ex.Message;
                return null;
            }

        }
        public static bool COMPANY_INFORMATION_TEMIZLE()
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Contento.db"), false);
                conn.Query<MEMBER_DATA>("Delete From COMPANY_INFORMATION");
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        public static bool COMPANY_INFORMATION_Guncelle(COMPANY_INFORMATION Tablo)
        {
            try
            {
                var conn = new SQLiteConnection(System.IO.Path.Combine(documentsFolder(), "Contento.db"), false);
                conn.Update(Tablo);
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return false;
            }

        }
        #endregion
    }
}