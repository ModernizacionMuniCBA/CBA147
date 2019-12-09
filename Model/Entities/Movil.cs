using Model.Comandos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Serializable()]
    public class Movil : BaseEntity
    {
        public virtual CerrojoArea Area { get; set; }

        //Datos Identificatorios
        public virtual TipoMovil Tipo { get; set; }
        public virtual string Modelo { get; set; }
        public virtual string Marca { get; set; }
        public virtual string Dominio { get; set; }
        public virtual DateTime? FechaIncorporacion { get; set; }
        public virtual string NumeroInterno { get; set; }
        public virtual string Carga { get; set; }
        public virtual int? Año { get; set; }
        public virtual int? Asientos { get; set; }
        public virtual Enums.TipoCombustible? TipoCombustible { get; set; }
        public virtual string Caracteristicas { get; set; }

        ////Datos Adicionales
        public virtual IList<ITVPorMovil> ITVsPorMovil { get; set; }
        public virtual IList<TUVPorMovil> TUVsPorMovil { get; set; }
        public virtual IList<KilometrajePorMovil> KilometrajesPorMovil { get; set; }
        public virtual IList<ValuacionPorMovil> ValuacionesPorMovil { get; set; }
        public virtual IList<NotaPorMovil> NotasPorMovil { get; set; }
        public virtual IList<ReparacionPorMovil> ReparacionesPorMovil { get; set; }

        //Estado del movil
        public virtual Enums.CondicionMovil? Condicion { get; set; }
        public virtual IList<EstadoMovilHistorial> Estados { get; set; }

        //Ordenes
        public virtual IList<MovilPorOrdenTrabajo> OrdenesTrabajo { get; set; }

        //Flota
        public virtual IList< Flota > Flotas { get; set; }
        public virtual Flota FlotaActiva { get; set; }

        public Movil()
        {

        }

        public Movil(Comando_Movil comando)
        {
            if ((int)comando.IdCondicion != -1)
            {
                this.Condicion = comando.IdCondicion;
            }
            this.Asientos = comando.Asientos;
            this.Carga = comando.Carga;
            this.Dominio = comando.Dominio;
            this.Año = comando.Año;
            this.Caracteristicas = comando.Caracteristicas;
            this.TipoCombustible = comando.IdTipoCombustible;
            this.FechaIncorporacion = comando.FechaIncorporacion;
            this.Marca = comando.Marca;
            this.Modelo = comando.Modelo;
            this.NumeroInterno = comando.NumeroInterno;
        }


        public virtual ValuacionPorMovil UltimaValuacion()
        {
            if (ValuacionesPorMovil == null || ValuacionesPorMovil.Count == 0) return null;
            return ValuacionesPorMovil.OrderByDescending(x => x.FechaAlta).FirstOrDefault();
        }

        public virtual KilometrajePorMovil UltimoKilometraje()
        {
            if (KilometrajesPorMovil == null || KilometrajesPorMovil.Count == 0) return null;
            return KilometrajesPorMovil.OrderByDescending(x => x.FechaAlta).FirstOrDefault();
        }

        public virtual ITVPorMovil UltimoITV()
        {
            if (ITVsPorMovil == null || ITVsPorMovil.Count == 0) return null;
            return ITVsPorMovil.OrderByDescending(x => x.FechaAlta).FirstOrDefault();
        }

        public virtual TUVPorMovil UltimoTUV()
        {
            if (TUVsPorMovil == null || TUVsPorMovil.Count == 0) return null;
            return TUVsPorMovil.OrderByDescending(x => x.FechaAlta).FirstOrDefault();
        }

        public virtual EstadoMovil UltimoEstado()
        {
            if (Estados == null || Estados.Count == 0) return null;
            return Estados.Where(x => x.FechaBaja == null).OrderByDescending(x => x.FechaAlta).FirstOrDefault().Estado;
        }

        public virtual ReparacionPorMovil UltimaReparacion()
        {
            if (ReparacionesPorMovil == null || ReparacionesPorMovil.Count == 0) return null;
            return ReparacionesPorMovil.Where(x => x.FechaBaja == null).OrderByDescending(x => x.FechaAlta).FirstOrDefault();
        }

    }
}
