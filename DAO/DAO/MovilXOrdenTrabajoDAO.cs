using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class MovilXOrdenTrabajoDAO : BaseDAO<MovilPorOrdenTrabajo>
    {
        private static MovilXOrdenTrabajoDAO instance;

        public static MovilXOrdenTrabajoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MovilXOrdenTrabajoDAO();
                }
                return instance;
            }
        }

        public Result<List<MovilPorOrdenTrabajo>> GetByIdOrdenTrabajo(int idOrdenTrabajo, bool dadosDeBaja)
        {
            var result = new Result<List<MovilPorOrdenTrabajo>>();
            try
            {
                var query = GetSession().QueryOver<MovilPorOrdenTrabajo>();
                query.Where(x => x.FechaBaja == null);
                query.JoinQueryOver<Movil>(x => x.Movil).Where(x => x.FechaBaja == null);
                query.JoinQueryOver(x => x.OrdenTrabajo).Where(x => x.Id == idOrdenTrabajo);
                result.Return = query.List().ToList();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<List<int>> GetIdsByIdOrdenTrabajo(int idOrdenTrabajo, bool dadosDeBaja)
        {
            var result = new Result<List<int>>();
            try
            {
                var query = GetSession().QueryOver<MovilPorOrdenTrabajo>();
                query.Where(x => x.FechaBaja == null);
                query.JoinQueryOver<Movil>(x => x.Movil).Where(x => x.FechaBaja == null);
                query.JoinQueryOver(x => x.OrdenTrabajo).Where(x => x.Id == idOrdenTrabajo);
                result.Return = query.Select(x => x.Movil.Id).List<int>().ToList();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }
    }
}
