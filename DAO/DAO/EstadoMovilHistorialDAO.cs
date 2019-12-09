using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class EstadoMovilHistorialDAO : BaseDAO<EstadoMovilHistorial>
    {
        private static EstadoMovilHistorialDAO instance;

        public static EstadoMovilHistorialDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EstadoMovilHistorialDAO();
                }
                return instance;
            }
        }

        //public Result<List<EstadoRequerimientoHistorial>> GetByFilters(int? idRQ, bool? dadosDeBaja)
        //{
        //    var result = new Result<List<EstadoRequerimientoHistorial>>();

        //    try
        //    {
        //        var query = GetSession().QueryOver<EstadoRequerimientoHistorial>();

        //        //ID RQ
        //        if (idRQ.HasValue)
        //        {
        //            query.JoinQueryOver<Requerimiento>(x => x.Requerimiento).Where(x => x.Id == idRQ.Value);
        //        }

        //        //Dado de baja
        //        if (dadosDeBaja.HasValue)
        //        {
        //            if (dadosDeBaja.Value)
        //            {
        //                query.Where(x => x.FechaBaja != null);
        //            }
        //            else
        //            {
        //                query.Where(x => x.FechaBaja == null);
        //            }
        //        }

        //        result.Return = query.List().ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        result.AddErrorInterno(e);
        //    }
        //    return result;
        //}
    }
}
