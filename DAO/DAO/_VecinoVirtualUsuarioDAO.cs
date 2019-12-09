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
using Model.Consultas;

namespace DAO.DAO
{
    public class _VecinoVirtualUsuarioDAO : BaseDAO<_VecinoVirtualUsuario>
    {
        private static _VecinoVirtualUsuarioDAO instance;

        public static _VecinoVirtualUsuarioDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new _VecinoVirtualUsuarioDAO();
                }
                return instance;
            }
        }

        public Result<_VecinoVirtualUsuario> GetByIdVecinoVirtual(int idVecinoVirtual)
        {
            var result = new Result<_VecinoVirtualUsuario>();

            try
            {
                var query = GetSession().QueryOver<_VecinoVirtualUsuario>();
                query.Where(x => x.Id == idVecinoVirtual && x.FechaBaja == null);
                result.Return = query.SingleOrDefault();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<List<_VecinoVirtualUsuario>> GetByFilters(Consulta_VecinoVirtualUsuario consulta)
        {
            var result = new Result<List<_VecinoVirtualUsuario>>();

            try
            {
                var query = GetSession().QueryOver<_VecinoVirtualUsuario>();
                if (consulta.IdVecinoVirtual.HasValue && consulta.IdVecinoVirtual.Value != -1)
                {
                    query.Where(x => x.Id == consulta.IdVecinoVirtual.Value);
                }

                if (!string.IsNullOrEmpty(consulta.Nombre))
                {
                    query.Where(x => x.Nombre.IsLike(consulta.Nombre, MatchMode.Anywhere));
                }

                if (!string.IsNullOrEmpty(consulta.Apellido))
                {
                    query.Where(x => x.Apellido.IsLike(consulta.Apellido, MatchMode.Anywhere));
                }

                if (consulta.Dni.HasValue)
                {
                    query.Where(x => x.Dni == consulta.Dni.Value);
                }

                if (!string.IsNullOrEmpty(consulta.Email))
                {
                    query.Where(x => x.Email.IsLike(consulta.Email, MatchMode.Anywhere));
                }

                if (!string.IsNullOrEmpty(consulta.Username))
                {
                    query.Where(x => x.Username.IsLike(consulta.Username, MatchMode.Anywhere));
                }

                if (consulta.SoloEmpleados.HasValue && consulta.SoloEmpleados.Value)
                {
                    query.Where(x => x.Empleado);
                }

                //Dado de baja
                if (consulta.DadosDeBaja.HasValue)
                {
                    if (consulta.DadosDeBaja.Value)
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


        public Result<bool> EsAplicacionBloqueada()
        {
            var resultado = new Result<bool>();

            IQuery query = GetSession().CreateSQLQuery("exec AplicacionBloqueada");
            try
            {
                var data = query.List<object[]>().ToList();
                if (data.Count == 0)
                {
                    resultado.AddErrorInterno("Error en el procedimiento");
                    return resultado;
                }
                resultado.Return = (bool)data[0][0];
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public override Result<_VecinoVirtualUsuario> GetByIdObligatorio(int id)
        {
            var resultado = base.GetByIdObligatorio(id);
            if (resultado.Ok)
            {
                GetSession().Refresh(resultado.Return);
            }
            return resultado;
        }

        public Result<List<int>> GetIdsAreasByIdUsuario(int idUsuario)
        {
            var result = new Result<List<int>>();

            try
            {

                IQuery query = GetSession().CreateSQLQuery("exec VecinoVirtual_Areas @idUsuarioVecinoVirtual=:idUsuarioVecinoVirtual");
                query.SetInt32("idUsuarioVecinoVirtual", idUsuario);

                var data = query.List<int>().ToList();
                result.Return = data;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }
    }
}
