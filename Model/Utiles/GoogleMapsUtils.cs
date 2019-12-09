using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Utiles
{
    public class GoogleMapsUtils
    {
        public static double[] Convertir(string latitud, string longitud)
        {
            System.Globalization.NumberFormatInfo nf = new System.Globalization.NumberFormatInfo()
            {
                NumberDecimalSeparator = ",",
            };

            double xGoogleDouble = double.Parse(latitud.Replace(".", ","), nf);
            double yGoogleDouble = double.Parse(longitud.Replace(".", ","), nf);
            return Convertir(xGoogleDouble, yGoogleDouble);
        }
        public static double[] Convertir(double latitud, double longitud)
        {
            var a = 6378137;
            var _f = 298.257223563;
            var f = 1 / (_f);
            var e2 = (2 * f) - (Math.Pow(f, 2));
            var ep2 = e2 / (1 - e2);
            var Q = 10001965.73;
            var ama = longitud - Q;
            var n = trunc(latitud / 1000000);
            var y = (latitud - n * 1000000 - 500000);
            var L0 = (3 * n - 75) * (Math.PI / 180);
            var a0 = 0.998324298;
            var a2 = 0.002514607;
            var a4 = 0.00000263905;
            var a6 = 0.00000000341805;

            var B1_ite11 = ama / (a0 * a);
            var B1_ite21 = B1_ite11 + (a2 * Math.Sin(2 * B1_ite11 / a0)) - (a4 * Math.Sin(4 * B1_ite11) / a0) + (a6 * Math.Sin(6 * B1_ite11) / a0);
            var B1_ite31 = B1_ite11 + (a2 * Math.Sin(2 * B1_ite21 / a0)) - (a4 * Math.Sin(4 * B1_ite21) / a0) + (a6 * Math.Sin(6 * B1_ite21) / a0);
            var B1_ite41 = B1_ite11 + (a2 * Math.Sin(2 * B1_ite31 / a0)) - (a4 * Math.Sin(4 * B1_ite31) / a0) + (a6 * Math.Sin(6 * B1_ite31) / a0);
            var B1_ite51 = B1_ite11 + (a2 * Math.Sin(2 * B1_ite41 / a0)) - (a4 * Math.Sin(4 * B1_ite41) / a0) + (a6 * Math.Sin(6 * B1_ite41) / a0);
            var B1_ite61 = B1_ite11 + (a2 * Math.Sin(2 * B1_ite51 / a0)) - (a4 * Math.Sin(4 * B1_ite51) / a0) + (a6 * Math.Sin(6 * B1_ite51) / a0);
            var B1_ite71 = B1_ite11 + (a2 * Math.Sin(2 * B1_ite61 / a0)) - (a4 * Math.Sin(4 * B1_ite61) / a0) + (a6 * Math.Sin(6 * B1_ite61) / a0);
            var B1_ite81 = B1_ite11 + (a2 * Math.Sin(2 * B1_ite71 / a0)) - (a4 * Math.Sin(4 * B1_ite71) / a0) + (a6 * Math.Sin(6 * B1_ite71) / a0);
            var B1_ite91 = B1_ite11 + (a2 * Math.Sin(2 * B1_ite81 / a0)) - (a4 * Math.Sin(4 * B1_ite81) / a0) + (a6 * Math.Sin(6 * B1_ite81) / a0);
            var B1_ite101 = B1_ite11 + (a2 * Math.Sin(2 * B1_ite91 / a0)) - (a4 * Math.Sin(4 * B1_ite91) / a0) + (a6 * Math.Sin(6 * B1_ite91) / a0);

            var M1 = a * (1 - e2) / (Math.Pow(1 - e2 * Math.Pow(Math.Sin(B1_ite101), 2), 1.5));

            var N1 = a / (Math.Pow(1 - e2 * Math.Pow(Math.Sin(B1_ite101), 2), 0.5));
            var t1 = Math.Tan(B1_ite101);
            var eta1c = ep2 * Math.Pow(Math.Cos(B1_ite101), 2);
            var B_pt = Math.Pow(y, 2) * t1 / (2 * N1 * M1);
            var B_st = Math.Pow(y, 4) * t1 * (5 + 3 * Math.Pow(t1, 2) + eta1c) / (24 * Math.Pow(N1, 3) * M1);
            var B_tt = Math.Pow(y, 6) * t1 * (61 + 90 * Math.Pow(t1, 2) + 45 * Math.Pow(t1, 4)) / (720 * Math.Pow(N1, 5) * M1);

            var LAT = B1_ite101 - B_pt + B_st - B_tt;
            var LON_pt = y / (N1 * Math.Cos(B1_ite101));
            var LON_st = Math.Pow(y, 3) * (1 + 2 * Math.Pow(t1, 2) + eta1c) / (6 * Math.Pow(N1, 3) * Math.Cos(B1_ite101));
            var LON_tt = Math.Pow(y, 5) * (5 + 28 * Math.Pow(t1, 2) + 24 * Math.Pow(t1, 4)) / (120 * Math.Pow(N1, 5) * Math.Cos(B1_ite101));
            var LON = L0 + LON_pt - LON_st + LON_tt;


            var LAT_gr = trunc((180 * (LAT)) / (Math.PI));
            var LAT_min = trunc(((180 * (LAT)) / (Math.PI) - LAT_gr) * 60);
            var LAT_seg = 3600 * ((180 * (LAT)) / (Math.PI) - LAT_min / 60 - LAT_gr);
            var LON_gr = trunc((180 * (LON)) / (Math.PI));
            var LON_min = trunc(((180 * (LON)) / (Math.PI) - LON_gr) * 60);
            var LON_seg = 3600 * ((180 * (LON)) / (Math.PI) - LON_min / 60 - LON_gr);

            var resultado = new double[2];
            resultado[0] = LAT_gr + (LAT_min / 60) + (LAT_seg / 3600);
            resultado[0] -= 0.000090;
            resultado[1] = LON_gr + (LON_min / 60) + (LON_seg / 3600);
            return resultado;
        }
        public static double Distancia(double[] coordenada1, double[] coordenada2)
        {
            var lat1 = coordenada1[0];
            var lon1 = coordenada1[1];
            var lat2 = coordenada2[0];
            var lon2 = coordenada2[1];

            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            dist = dist * 1.609344;
            return (dist);
        }

        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        private static double trunc(double x)
        {
            return x < 0 ? Math.Ceiling(x) : Math.Floor(x);
        }


    }
}
