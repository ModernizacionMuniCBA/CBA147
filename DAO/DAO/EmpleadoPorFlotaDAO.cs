using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Transform;
using Model.Consultas;
using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NHibernate;
using System.Text;
using System.Diagnostics;
using NHibernate.Criterion;
using Model.Utiles;

namespace DAO.DAO
{
    public class EmpleadoPorFlotaDAO : BaseDAO<EmpleadoPorFlota>
    {
        private static EmpleadoPorFlotaDAO instance;

        public static EmpleadoPorFlotaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EmpleadoPorFlotaDAO();
                }
                return instance;
            }
        }

        public Result<EmpleadoPorFlota> GetByIdEmpleado(int idEmpleado)
        {
            var result = new Result<EmpleadoPorFlota>();

            try
            {
                var query = GetSession().QueryOver<EmpleadoPorFlota>();
                query.Where(x => x.Empleado.Id==idEmpleado);
                query.Where(x => x.FechaBaja == null);

                var resultado = query.List().ToList();
                result.Return = resultado.FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }


    }
}
