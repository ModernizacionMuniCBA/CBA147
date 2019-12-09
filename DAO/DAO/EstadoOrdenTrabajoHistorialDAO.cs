using System.Collections.Generic;
using Model;
using Model.Entities;
using System;
using System.Linq;

namespace DAO.DAO
{
    public class EstadoOrdenTrabajoHistorialDAO : BaseDAO<EstadoOrdenTrabajoHistorial>
    {
        private static EstadoOrdenTrabajoHistorialDAO instance;

        public static EstadoOrdenTrabajoHistorialDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EstadoOrdenTrabajoHistorialDAO();
                }
                return instance;
            }
        }

        public Result<List<EstadoOrdenTrabajoHistorial>> GetByFilters(int? idOT, bool? dadosDeBaja)
        {
            var result = new Result<List<EstadoOrdenTrabajoHistorial>>();

            try
            {
                var query = GetSession().QueryOver<EstadoOrdenTrabajoHistorial>();

                //ID OT
                if (idOT.HasValue)
                {
                    query.JoinQueryOver<OrdenTrabajo>(x => x.OrdenTrabajo).Where(x => x.Id == idOT.Value);
                }

                //Dado de baja
                if (dadosDeBaja.HasValue)
                {
                    if (dadosDeBaja.Value)
                    {
                        query.Where(x => x.FechaBaja != null);
                    }
                    else
                    {
                        query.Where(x => x.FechaBaja == null);
                    }
                }
                
                result.Return = query.List().ToList();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

    }
}
