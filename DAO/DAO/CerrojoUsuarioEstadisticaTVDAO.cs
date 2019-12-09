using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;
using Model.Resultados;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using NHibernate;

namespace DAO.DAO
{
    public class CerrojoUsuarioEstadisticaTVDAO : BaseDAO<_VecinoVirtualUsuario>
    {
        private static CerrojoUsuarioEstadisticaTVDAO instance;

        public static CerrojoUsuarioEstadisticaTVDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CerrojoUsuarioEstadisticaTVDAO();
                }
                return instance;
            }
        }

        public Result<bool> TengoPermiso(_Resultado_VecinoVirtualUsuario usuario)
        {
            var result = new Result<bool>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec UsuarioCerrojoEstadisticasTV @IdUsuarioCerrojo=:idUsuarioCerrojo");
                query.SetInt32("idUsuarioCerrojo", usuario.Id);

                var cantidad=query.List<int>();
                result.Return = cantidad[0] == 0 ? false : true;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

    }
}
