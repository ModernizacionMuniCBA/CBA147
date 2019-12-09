using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Model;
using Model.Entities;
using Rules.Rules;
using Rules;
using Intranet_Servicios2.MisRules;
using Intranet_Servicios2.Utils.MisRules;
using Intranet_Servicios2.Utils.Entities.Comando;
using System.Configuration;
using Model.Resultados;
using Model.Resultados.Estadisticas;

namespace Intranet_Servicios2.v1.MisRules
{
    public class WSRules_Estadisticas : _WSRules_Base<BaseEntity>
    {

        private readonly CerrojoUsuarioEstadisticaTVRules rules;
        private readonly _WSRules_BaseEstadistica rulesBase;

        public WSRules_Estadisticas(UsuarioLogueado data)
            : base(data)
        {
            rules = new CerrojoUsuarioEstadisticaTVRules(data);
            rulesBase = new _WSRules_BaseEstadistica(data);
        }


        public ResultadoServicio<Resultado_DatosEstadisticaPanel> GetDatosEstadisticaPanel(DateTime fechaDesde, DateTime fechaHasta)
        {
            return rulesBase.GetDatosEstadisticaPanel(fechaDesde, fechaHasta);
        }

        //public ResultServicio<List<Resultado_DatosEstadisticaPanel_Cpc>> GetMapaCritico(DateTime fechaDesde, DateTime fechaHasta)
        //{
        //    return rulesBase.GetMapaCritico(fechaDesde, fechaHasta);
        //}

        public ResultadoServicio<bool?> ValidarUsuarioEstadisticasTV()
        {
            return rulesBase.ValidarUsuarioEstadisticasTV();
        }
    }
}
