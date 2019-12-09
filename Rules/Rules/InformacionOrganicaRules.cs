using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using Model.Resultados;
using Model.Comandos;
using Model.Consultas;

namespace Rules.Rules
{
    public class InformacionOrganicaRules : BaseRules<InformacionOrganica>
    {

        private readonly InformacionOrganicaDAO dao;

        public InformacionOrganicaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = InformacionOrganicaDAO.Instance;
        }


        public Result<List<InformacionOrganica>> GetByFilters(Consulta_InformacionOrganica consulta)
        {
            return dao.GetByFilters(consulta);
        }

        public Result<List<Resultado_InformacionOrganica>> GetResultadoByFilters(Consulta_InformacionOrganica consulta)
        {
            var resultado = new Result<List<Resultado_InformacionOrganica>>();

            var resultadoConsulta = dao.GetByFilters(consulta);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            resultado.Return = Resultado_InformacionOrganica.ToList(resultadoConsulta.Return);
            return resultado;
        }

        public Result<Resultado_InformacionOrganica> GetByIdArea(int idArea)
        {
            var resultado = new Result<Resultado_InformacionOrganica>();

            var resultadoConsulta = dao.GetByFilters(new Consulta_InformacionOrganica()
            {
                IdArea = idArea
            });

            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return != null && resultadoConsulta.Return.Count != 0)
            {
                resultado.Return = new Resultado_InformacionOrganica(resultadoConsulta.Return[0]);
            }
            return resultado;
        }   

        public Result<Resultado_InformacionOrganica> Insertar(Comando_InformacionOrganica comando)
        {
            var resultado = new Result<Resultado_InformacionOrganica>();

            try
            {
                var resultadoConsulta = GetByFilters(new Consulta_InformacionOrganica()
                {
                    DadosDeBaja = null,
                    IdArea = comando.IdArea
                });

                if (!resultadoConsulta.Ok)
                {
                    resultado.Copy(resultadoConsulta.Errores);
                    return resultado;
                }

                InformacionOrganica entity;
                if (resultadoConsulta.Return.Count != 0)
                {
                    entity = resultadoConsulta.Return[0];
                    entity.FechaBaja = null;
                }
                else
                {
                    entity = new InformacionOrganica();
                    entity.UsuarioCreador = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetByIdObligatorio(getUsuarioLogueado().Usuario.Id).Return;
                }

                entity.Area = new _CerrojoAreaRules(getUsuarioLogueado()).GetByIdObligatorio(comando.IdArea).Return;
                entity.Direccion = new InformacionOrganicaDireccionRules(getUsuarioLogueado()).GetByIdObligatorio(comando.IdDireccion).Return;

                Result<InformacionOrganica> resultadoDB = null;
                if (entity.Id != 0)
                {
                    resultadoDB = base.Update(entity);
                }
                else
                {
                    resultadoDB = base.Insert(entity);

                }
                if (!resultadoDB.Ok)
                {
                    resultado.Copy(resultadoDB.Errores);
                    return resultado;
                }

                resultado.Return = new Resultado_InformacionOrganica(resultadoDB.Return);
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<Resultado_InformacionOrganica> DarDeBaja(int id)
        {
            var resultado = new Result<Resultado_InformacionOrganica>();
            var resultadoDelete = DeleteById(id);
            if (!resultadoDelete.Ok)
            {
                resultado.Errores.Copy(resultadoDelete.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_InformacionOrganica(resultadoDelete.Return);
            return resultado;
        }
    }
}
