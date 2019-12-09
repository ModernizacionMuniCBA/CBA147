using System;
using System.Linq;

namespace Model.Utiles
{
    public class LatLng
    {
        public double Lat { get; set; }

        public double Lng { get; set; }

        public LatLng()
        {
        }

        public LatLng(double lat, double lng)
        {
            this.Lat = lat;
            this.Lng = lng;
        }

    }

}
