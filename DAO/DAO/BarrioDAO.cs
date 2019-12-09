using Model;
using Model.Entities;
using System;
using System.Linq;

namespace DAO.DAO
{
    public class BarrioDAO: BaseDAO<Barrio>
    {
        private static BarrioDAO instance;

        public static BarrioDAO Instance {
            get {
                if (instance == null)
                    instance = new BarrioDAO();
                return instance;
            }         
        }

        public Result<Barrio> GetByIdCatastro(int idCatastro)
        {
            var result = new Result<Barrio>();
            try
            {
                var query = GetSession().QueryOver<Barrio>();
                query.Where(x => x.IdCatastro == idCatastro);

                var barrios = query.List().ToList();
                if (barrios.Count == 0)
                {
                    result.Return = null;
                }
                else
                {
                    result.Return = barrios[0];
                }
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        //public Result<List<Barrio>> GetByFilters(int? idCPC, bool? dadosDeBaja)
        //{
        //    var result = new Result<List<Barrio>>();

        //        //try
        //        //{
        //        //    var query = GetSession().QueryOver<Barrio>();
         
        //        //    //CPC
        //        //    if (idCPC.HasValue)
        //        //    {
        //        //        query.JoinQueryOver<Cpc>(x => x.).Where(x => x.Id == idTema.Value);
        //        //    }

        //        //    //Nombre
        //        //    if (!string.IsNullOrEmpty(nombre))
        //        //    {
        //        //        query.Where(x => x.Nombre.IsLike(nombre, MatchMode.Anywhere));
        //        //    }

        //        //    //Dados de baja
        //        //    if (dadosDeBaja.HasValue)
        //        //    {
        //        //        if (dadosDeBaja.Value)
        //        //        {
        //        //            query.Where(x => x.FechaBaja != null);
        //        //        }
        //        //        else
        //        //        {
        //        //            query.Where(x => x.FechaBaja == null);
        //        //        }
        //        //    }
        //        //    result.Return = new List<Motivo>(query.List());
        //        //}
        //        //catch (Exception e)
        //        //{
        //        //    result.AddError(e.Message);
        //        //}
        //        //return result;
            
        //}
    }
}
