using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class RecursoPorOrdenTrabajoDAO : BaseDAO<RecursoPorOrdenTrabajo>
    {
        private static RecursoPorOrdenTrabajoDAO instance;

        public static RecursoPorOrdenTrabajoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RecursoPorOrdenTrabajoDAO();
                }
                return instance;
            }
        }

        public Result<bool> DeleteByIdOrdenTrabajo(int idOT)
        {
            var result = new Result<bool>();

            try
            {
                String delete = "DELETE FROM RecursoXOrdenTrabajo WHERE IdOrdenTrabajo = " + idOT;
                var query = GetSession().CreateQuery(delete);
                query.ExecuteUpdate();

                result.Return = true;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.Message);
                if (e.InnerException != null)
                {
                    result.AddErrorInterno(e.InnerException.Message);
                }
            }

            return result;
        }


        public Result<List<RecursoPorOrdenTrabajo>> GetByIdOrdenTrabajo(int idOT)
        {
            var result = new Result<List<RecursoPorOrdenTrabajo>>();

            try
            {
                var query = GetSession().QueryOver<RecursoPorOrdenTrabajo>();
                query.JoinQueryOver<OrdenTrabajo>(x => x.OrdenTrabajo).Where(x=> x.Id == idOT);

                var recursos = new List<RecursoPorOrdenTrabajo>(query.List());
                result.Return = recursos;
            }

            catch (Exception e)
            {
                result.AddErrorInterno(e.Message);
                if (e.InnerException != null)
                {
                    result.AddErrorInterno(e.InnerException.Message);
                }
            }
            return result;
        }
    }
}