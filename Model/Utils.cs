using System;
using System.Linq;

namespace Model
{
    public class Utils
    {
        public const string DATE_FORMAT = "dd/MM/yyyy";
        public const string TIME_FORMAT = "HH:mm";
        public const string DATETIME_FORMAT = "dd/MM/yyyy HH:mm:ss";
        public const string SQLDATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public static string DateToString(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            try
            {
                return date.Value.ToString(DATE_FORMAT);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static DateTime? StringToDate(string date)
        {
            try
            {
                return DateTime.ParseExact(date, DATE_FORMAT, null);
            }
            catch
            {
                return null;
            }
        }
        public static string TimeToString(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            try
            {
                return date.Value.ToString(TIME_FORMAT);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static DateTime? StringToTime(string date)
        {
            try
            {
                return DateTime.ParseExact(date, TIME_FORMAT, null);
            }
            catch
            {
                return null;
            }
        }

        public static string DateTimeToString(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }
            try
            {
                return date.Value.ToString(DATETIME_FORMAT);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static DateTime? StringToDateTime(string date)
        {
            try
            {
                return DateTime.ParseExact(date, DATETIME_FORMAT, null);
            }
            catch
            {
                return null;
            }
        }

        public static string SexoToString(Enums.Sexo sexo)
        {
            switch (sexo)
            {
                case Enums.Sexo.MASCULINO:
                    return "Masculino";
                case Enums.Sexo.FEMENINO:
                    return "Femenino";
            }

            return "";
        }

        public static Int32 DateToEdad(DateTime? fecha)
        {
            if (!fecha.HasValue) return 0;

            var today = DateTime.Today;
            var age = today.Year - fecha.Value.Year;
            if (fecha > today.AddYears(-age)) age--;
            return age;
        }

        public static string toTitleCase(string str)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
            ToTitleCase(str.ToLower());
        }

        public static double ToMilliseconds(DateTime? time)
        {
            if (!time.HasValue) return 0;

            return time.Value.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds;
        }
    }
}

