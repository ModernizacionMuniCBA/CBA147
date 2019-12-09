using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate;
using NHibernate.Criterion;

namespace DAO.DAO
{
    public class PersonaFisicaDAO : BaseDAO<PersonaFisica>
    {
        private static PersonaFisicaDAO instance;

        public static PersonaFisicaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PersonaFisicaDAO();
                }
                return instance;
            }
        }

        public IQueryOver<PersonaFisica, PersonaFisica> GetQuery(Enums.TipoDocumento? tipoDocumento, string numeroDocumento, string cuil, string nombre, string apellido, string email, Enums.Sexo? sexo, int? idCpc, int? idBarrio, bool? dadosDeBaja)
        {
            var query = GetSession().QueryOver<PersonaFisica>();

            //TipoDocumento
            if (tipoDocumento.HasValue)
            {
                query.JoinQueryOver<TipoDocumento>(x => x.TipoDocumento).Where(x => x.KeyValue == tipoDocumento.Value);
            }

            //Numero Documento
            if (numeroDocumento != null)
            {
                query.Where(x => x.NroDoc == numeroDocumento);
            }

            //Cuil
            if (cuil != null)
            {
                query.Where(x => x.Cuil == cuil);
            }

            //Nombre
            if (!string.IsNullOrEmpty(nombre))
            {
                query.Where(x => x.Nombre.IsLike(nombre, MatchMode.Anywhere));
            }

            //Apellido
            if (!string.IsNullOrEmpty(apellido))
            {
                query.Where(x => x.Apellido.IsLike(apellido, MatchMode.Anywhere));
            }

            //Email
            //if (!string.IsNullOrEmpty(email))
            //{
            //    query.Where(x => x.Mail.IsLike(email, MatchMode.Exact));
            //}

            //Sexo
            if (sexo.HasValue)
            {
                query.Where(x => x.Sexo == sexo.Value);
            }

            if (idCpc.HasValue || idBarrio.HasValue)
            {
                var joinDomicilio = query.JoinQueryOver<Domicilio>(x => x.Domicilio);

                //CPC
                if (idCpc.HasValue && idCpc.Value != -1)
                {
                    joinDomicilio.JoinQueryOver<Cpc>(x => x.Cpc).Where(x => x.Id == idCpc.Value);
                }

                //Barrio
                if (idBarrio.HasValue && idBarrio.Value != -1)
                {
                    joinDomicilio.JoinQueryOver<Barrio>(x => x.Barrio).Where(x => x.Id == idBarrio.Value);
                }
            }

            if (!string.IsNullOrEmpty(email))
            {
                query.Where(x => x.Mail.IsLike(apellido, MatchMode.Exact));
            }

            //Dados de baja
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

            return query;
        }

        public Result<List<PersonaFisica>> GetByFilters(Enums.TipoDocumento? tipoDocumento, string numeroDocumento, string cuil, string nombre, string apellido, string email, Enums.Sexo? sexo, int? idCpc, int? idBarrio, bool? dadosDeBaja)
        {
            var result = new Result<List<PersonaFisica>>();

            try
            {
                var query = GetQuery(tipoDocumento, numeroDocumento, cuil, nombre, apellido, email, sexo, idCpc, idBarrio, dadosDeBaja);
                result.Return = new List<PersonaFisica>(query.List());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<int> GetCantidad(Enums.TipoDocumento? tipoDocumento, string numeroDocumento, string cuil, string nombre, string apellido, string email, Enums.Sexo? sexo, int? idCpc, int? idBarrio, bool? dadosDeBaja)
        {
            var result = new Result<int>();

            try
            {
                var query = GetQuery(tipoDocumento, numeroDocumento, cuil, nombre, apellido, email, sexo, idCpc, idBarrio, dadosDeBaja);
                result.Return = query.RowCount();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<int> GetCantidadDuplicados(int? id, Enums.TipoDocumento tipo, string nro, bool? dadosDeBaja)
        {
            var result = new Result<int>();

            try
            {
                var query = GetSession().QueryOver<PersonaFisica>();
                if (id.HasValue)
                {
                    query.Where(x => x.Id != id.Value);
                }
                if (tipo != null && nro!=null)
                {
                    query.JoinQueryOver(x => x.TipoDocumento).Where(x => x.KeyValue == tipo);
                    query.Where(x => x.NroDoc == nro);
                }
                else
                {
                    result.AddErrorInterno("Consulta invalida");
                    return result;
                }

                //Dados de baja
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

                result.Return = query.RowCount();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

    }
}
