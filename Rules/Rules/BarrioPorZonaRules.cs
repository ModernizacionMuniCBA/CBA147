using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Consultas;
using Model.Resultados;
using Model.Comandos;

namespace Rules.Rules
{
    public class BarrioPorZonaRules : BaseRules<BarrioPorZona>
    {

        private readonly BarrioPorZonaDAO dao;
        public BarrioPorZonaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = BarrioPorZonaDAO.Instance;
        }

        /* Validaciones */

        public override Result<BarrioPorZona> ValidateDatosNecesarios(BarrioPorZona entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Requerimiento
            if (entity.Zona == null)
            {
                result.AddErrorPublico("Debe ingresar la zona");
                return result;
            }
            return result;
        }

        /* Busqueda */


        public Result<List<int>> GetIdsByFilters(Consulta_BarrioPorZona consulta)
        {
            return dao.GetIdsByFilters(consulta);
        }

        public Result<bool> ValidarBarrio(int? idZona, int idBarrio, int idArea)
        {
            return dao.ValidarBarrio(idZona, idBarrio, idArea);
        }


        public Result<List<Resultado_BarrioPorZona>> GetByFilters(Consulta_BarrioPorZona consulta)
        {
            return dao.GetByFilters(consulta);
        }

        public Result<List<int>> GetIdsBarrioByZona(int idZona)
        {
            return dao.GetIdsBarrioByZona(idZona);
        }

        public Result<List<int>> GetIdsBarrioByArea(int idArea)
        {
            return dao.GetIdsBarrioByArea(idArea);
        }

        public Result<List<int>> GetIdsBarriosYaSeleccionados(int? idZona, int idArea)
        {
            return dao.GetIdsBarriosYaSeleccionados(idZona, idArea);
        }

    }
}
