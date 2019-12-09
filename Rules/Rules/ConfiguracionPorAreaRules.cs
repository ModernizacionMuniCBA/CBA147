using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Comandos;
using Model.Resultados;

namespace Rules.Rules
{
    public class ConfiguracionPorAreaRules
    {
        public virtual UsuarioLogueado usuarioLogeado { get; set; }

        public ConfiguracionPorAreaRules(UsuarioLogueado data)
        {
            usuarioLogeado = data;
        }

        public Result<Resultado_ConfiguracionPorAreaDataInicial> GetDataInicialConfiguraciones()
        {
            var result = new Result<Resultado_ConfiguracionPorAreaDataInicial>();
            result.Return = new Resultado_ConfiguracionPorAreaDataInicial();
            result.Return.TiposMotivo = new TipoMotivoRules(usuarioLogeado).GetAll();
            result.Return.EstadosCreacionOT = Resultado_EstadoOrdenTrabajo.ToList(new EstadoOrdenTrabajoRules(usuarioLogeado).GetEstadosValidosParaCrear().Return);
            return result;
        }


        public Result<Resultado_ConfiguracionPorArea> GetConfiguraciones(int idArea)
        {
            var result = new Result<Resultado_ConfiguracionPorArea>();

            //valido si estoy configurado para esa area
            if (!usuarioLogeado.IdsAreas.Contains(idArea))
            {
                result.AddErrorPublico("Usted no tiene permisos para configurar el área");
                return result;
            }

            result.Return = new Resultado_ConfiguracionPorArea();

            var resultBandeja = new ConfiguracionBandejaPorAreaRules(usuarioLogeado).GetConfiguracion(idArea);
            if (!resultBandeja.Ok)
            {
                result.Copy(resultBandeja.Errores);
                return result;
            }
            result.Return.TipoMotivoDefectoBandeja = new TipoMotivoRules(usuarioLogeado).GetByKeyValue(resultBandeja.Return.TipoMotivoPorDefecto);

            var resultEstadoCreacionOT= new ConfiguracionEstadoCreacionOTRules(usuarioLogeado).GetConfiguracion(idArea);
            if (!resultEstadoCreacionOT.Ok)
            {
                result.Copy(resultEstadoCreacionOT.Errores);
                return result;
            }

            result.Return.EstadoCreacionOT = new Resultado_EstadoOrdenTrabajo(resultEstadoCreacionOT.Return.EstadoCreacionOT);
            
            return result;
        }


        public Result<bool> SetConfiguraciones(Comando_ConfiguracionPorArea comando)
        {
            var result = new Result<bool>();

            //valido si estoy configurado para esa area
            if (!usuarioLogeado.IdsAreas.Contains(comando.IdArea))
            {
                result.AddErrorPublico("Usted no tiene permisos para configurar el área");
                return result;
            }

            var resultBandeja = new ConfiguracionBandejaPorAreaRules(usuarioLogeado).Insertar(comando.IdArea, comando.TipoMotivoDefectoBandeja);
            if (!resultBandeja.Ok)
            {
                result.Copy(resultBandeja.Errores);
                return result;
            }

            var resultEstadoCreacionOT = new ConfiguracionEstadoCreacionOTRules(usuarioLogeado).Insertar(comando.IdArea, comando.EstadoCreacionOT);
            if (!resultEstadoCreacionOT.Ok)
            {
                result.Copy(resultEstadoCreacionOT.Errores);
                return result;
            }

            result.Return = true;
            return result;
        }

    }
}
