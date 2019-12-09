using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Utils
{
    public static class Distancia
    {
        public static double getDistanceFromLatLonInKm(string lat1, string lon1, string lat2, string lon2)
        {
            var lat1long = double.Parse(lat1);
            var lon1long = double.Parse(lon1);
            var lat2long = double.Parse(lat2);
            var lon2long = double.Parse(lon2);

            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2long - lat1long);  // deg2rad below
            var dLon = deg2rad(lon2long - lon1long);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1long)) * Math.Cos(deg2rad(lat2long)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }

        public static double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
}
    }
}