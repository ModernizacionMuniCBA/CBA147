using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using System.Text;
using Model;
using Model.Entities;
using NHibernate.Criterion;
using Model.Consultas;




namespace DAO.DAO
{
    public class CerrojoAreaDAO : BaseDAO<CerrojoArea>
    {
        private static CerrojoAreaDAO instance;

        public static CerrojoAreaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CerrojoAreaDAO();
                }
                return instance;
            }
        }

        public Result<List<CerrojoArea>> GetByFilters(Consulta_Area consulta)
        {
            var result = new Result<List<CerrojoArea>>();

            try
            {
                var query = GetSession().QueryOver<CerrojoArea>();
                var joinMotivo = query.JoinQueryOver<Motivo>(x => x.Motivos);
                if (consulta.Tipos != null && consulta.Tipos.Count > 0)
                {
                  joinMotivo .Where(z => z.Tipo.IsIn(consulta.Tipos) && z.FechaBaja == null);
                }

                if (consulta.IdServicio.HasValue)
                {
                    joinMotivo.Where(z=> z.FechaBaja == null).JoinQueryOver<Tema>(x=>x.Tema).JoinQueryOver<Servicio>(x=>x.Servicio).Where(x=>x.Id==consulta.IdServicio.Value);
                }

                //Traigo los Dados de baja O los activos
                if (consulta.DadosDeBaja == true)
                {
                    query.Where(x => x.FechaBaja != null);
                }
                else
                {
                    query.Where(x => x.FechaBaja == null);
                }

                result.Return = new List<CerrojoArea>(query.List().Distinct());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

       

        public Result<bool> YaExiste(string nombre)
        {
            var result = new Result<bool>();

            try
            {
                var query = GetSession().QueryOver<CerrojoArea>();
                query.Where(x => x.Nombre == nombre);
                result.Return = query.RowCount() != 0;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }
    }
}
