using Model.Utiles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class TerritorioIncumbencia : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual string Poligono { get; set; }
        public virtual CerrojoArea Area { get; set; }

        public virtual bool EstaEnMiTerritorio(double lat, double lon)
        {
            var latlongRq = new LatLng(lat, lon);

            var pol = CreatePolygon(Poligono);
            if (ContainsLocation(latlongRq, pol))
            {
                return true;
            }

            return false;
        }

        public virtual List<LatLng> CreatePolygon(string textoPoligono)
        {
            var poligono = new List<LatLng>();
            var list=textoPoligono.Split(';');

            foreach (var latLnt in list)
            {
                var tmp = latLnt.Split(',');
                var latLng = new LatLng(Double.Parse(tmp[1].Trim(), CultureInfo.InvariantCulture), Double.Parse(tmp[0].Trim(), CultureInfo.InvariantCulture));
                poligono.Add(latLng);
            }

            return poligono;
        }

        public static bool ContainsLocation(LatLng latLng, List<LatLng> polygon)
        {
            var inPoly = false;

            int i;
            int j = polygon.Count - 1;

            for (i = 0; i < polygon.Count; i++)
            {
                if (polygon[i].Lng < latLng.Lng && polygon[j].Lng >= latLng.Lng || polygon[j].Lng < latLng.Lng && polygon[i].Lng >= latLng.Lng)
                {
                    if (polygon[i].Lat + (latLng.Lng - polygon[i].Lng) / (polygon[j].Lng - polygon[i].Lng) * (polygon[j].Lat - polygon[i].Lat) < latLng.Lat)
                    {
                        inPoly = !inPoly;
                    }
                }
                j = i;
            }

            return inPoly;
        }


    }
}
