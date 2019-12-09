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
    public class ConfiguracionBandejaPorAreaRules : BaseRules<ConfiguracionBandejaPorArea>
    {

        private readonly ConfiguracionBandejaPorAreaDAO dao;

        public ConfiguracionBandejaPorAreaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = ConfiguracionBandejaPorAreaDAO.Instance;
        }

        public Result<ConfiguracionBandejaPorArea> GetByIdArea(int idArea)
        {
            return dao.GetByIdArea(idArea);
        }

        public Result<ConfiguracionBandejaPorArea> GetConfiguracion(int idArea)
        {
            var result = dao.GetByIdArea(idArea);
            if (!result.Ok)
            {
                return result;
            }

            //si no hay configuracion, creo una por defecto
            if (result.Return == null)
            {
                var resultInsertar = Insertar(idArea, Enums.TipoMotivo.GENERAL);
                if (!resultInsertar.Ok)
                {
                    result.Copy(resultInsertar.Errores);
                    return result;
                }

                result = dao.GetByIdArea(idArea);
            }

            return result;
        }

        public Result<bool> Insertar(int idArea, Enums.TipoMotivo tipo)
        {
            var result = new Result<bool>();

            var tienePermiso = getUsuarioLogueado().IdsAreas.Contains(idArea);
            if (!tienePermiso)
            {
                result.AddErrorPublico("Usted no tiene permiso para realizar esta accion.");
                return result;
            }

            //consulto si ya hay una configuracion existente para esa area
            var resultConsulta = GetByIdArea(idArea);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            var configuracionExistente = resultConsulta.Return;
            var insertar = false;

            if (configuracionExistente == null)
            {
                configuracionExistente = new ConfiguracionBandejaPorArea();
                insertar = true;
                //si no hay configuraciones existentes, debo buscar el area para crearlas
                var resultArea = new _CerrojoAreaRules(getUsuarioLogueado()).GetById(idArea);
                if (!resultArea.Ok)
                {
                    result.Copy(resultArea.Errores);
                    return result;
                }

                configuracionExistente.Area = resultArea.Return;
            }

            configuracionExistente.TipoMotivoPorDefecto = tipo;
            configuracionExistente.PorDefecto = true;

            var resultTransaction = new Result<ConfiguracionBandejaPorArea>();

            if (insertar)
            {
                resultTransaction = new ConfiguracionBandejaPorAreaRules(getUsuarioLogueado()).Insert(configuracionExistente);
            }
            else
            {
                resultTransaction = new ConfiguracionBandejaPorAreaRules(getUsuarioLogueado()).Update(configuracionExistente);
            }

            if (!resultTransaction.Ok)
            {
                result.Copy(resultTransaction.Errores);
                return result;
            }

            return result;
        }
    }
}
