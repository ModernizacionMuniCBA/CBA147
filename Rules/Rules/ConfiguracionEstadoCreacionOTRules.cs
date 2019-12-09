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
    public class ConfiguracionEstadoCreacionOTRules : BaseRules<ConfiguracionEstadoCreacionOT>
    {

        private readonly ConfiguracionEstadoCreacionOTDAO dao;

        public ConfiguracionEstadoCreacionOTRules(UsuarioLogueado data)
            : base(data)
        {
            dao = ConfiguracionEstadoCreacionOTDAO.Instance;
        }

        public Result<ConfiguracionEstadoCreacionOT> GetByIdArea(int idArea)
        {
            return dao.GetByIdArea(idArea);
        }

        public Result<ConfiguracionEstadoCreacionOT> GetConfiguracion(int idArea)
        {
            var result = dao.GetByIdArea(idArea);
            if (!result.Ok)
            {
                return result;
            }

            //si no hay configuracion, creo una por defecto
            if (result.Return == null)
            {
                var resultInsertar = Insertar(idArea, Enums.EstadoOrdenTrabajo.ENPROCESO);
                if (!resultInsertar.Ok)
                {
                    result.Copy(resultInsertar.Errores);
                    return result;
                }

                result = dao.GetByIdArea(idArea);
            }

            return result;
        }

        public Result<bool> Insertar(int idArea, Enums.EstadoOrdenTrabajo estado)
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
                configuracionExistente = new ConfiguracionEstadoCreacionOT();
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

            var resultEstado = new EstadoOrdenTrabajoRules(getUsuarioLogueado()).GetByKeyValue(estado);
            if (!resultEstado.Ok)
            {
                result.Copy(resultEstado.Errores);
                return result;
            }

            configuracionExistente.EstadoCreacionOT = resultEstado.Return;

            var resultTransaction = new Result<ConfiguracionEstadoCreacionOT>();

            if (insertar)
            {
                resultTransaction = Insert(configuracionExistente);
            }
            else
            {
                resultTransaction = Update(configuracionExistente);
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
