using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Model.Consultas;
using Model.Comandos;

namespace Rules.Rules
{
    public class RequerimientoFavoritoPorUsuarioRules : BaseRules<RequerimientoFavoritoPorUsuario>
    {
        private readonly RequerimientoFavoritoPorUsuarioDAO dao;

        public RequerimientoFavoritoPorUsuarioRules(UsuarioLogueado data)
            : base(data)
        {
            dao = RequerimientoFavoritoPorUsuarioDAO.Instance;
        }

        public Result<List<RequerimientoFavoritoPorUsuario>> GetByFilters(Consulta_RequerimientoFavoritoPorUsuario consulta)
        {
            return dao.GetByFilters(consulta);
        }

        public Result<RequerimientoFavoritoPorUsuario> ToggleFavorito(int idRequerimiento)
        {
            var resultado = new Result<RequerimientoFavoritoPorUsuario>();

            try
            {
                //Usuario
                var resultadoUsuario = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id);
                if (!resultadoUsuario.Ok)
                {
                    resultado.Copy(resultadoUsuario.Errores);
                    return resultado;
                }

                if (resultadoUsuario.Return == null || resultadoUsuario.Return.FechaBaja != null)
                {
                    resultado.AddErrorPublico("El ususario no existe o esta dado de baja");
                    return resultado;
                }
                var usuario = resultadoUsuario.Return;

                //Requerimiento
                var resultadoRQ = new RequerimientoRules(getUsuarioLogueado()).GetByIdObligatorio(idRequerimiento);
                if (!resultadoRQ.Ok)
                {
                    resultado.Copy(resultadoRQ.Errores);
                    return resultado;
                }

                var requerimiento = resultadoRQ.Return;


                var resultadoConsulta = GetByFilters(new Consulta_RequerimientoFavoritoPorUsuario()
                {
                    IdUser = usuario.Id,
                    IdRequerimiento = requerimiento.Id,
                    DadosDeBaja = null
                });

                if (!resultadoConsulta.Ok)
                {
                    resultado.Copy(resultadoConsulta.Errores);
                    return resultado;
                }

                RequerimientoFavoritoPorUsuario entity = null;
                var favorito = false;
                if (resultadoConsulta.Return == null || resultadoConsulta.Return.Count() == 0)
                {
                    entity = new RequerimientoFavoritoPorUsuario();
                    favorito = true;
                }
                else
                {
                    entity = resultadoConsulta.Return[0];
                    favorito = entity.FechaBaja != null;
                }

                if (favorito)
                {
                    entity.FechaBaja = null;
                }
                else
                {
                    entity.FechaBaja = DateTime.Now;
                }

                entity.User = resultadoUsuario.Return;
                entity.Requerimiento = resultadoRQ.Return;

                Result<RequerimientoFavoritoPorUsuario> resultadoOperacion;

                if (entity.Id != 0)
                {
                    resultadoOperacion = base.Update(entity);
                }
                else
                {
                    resultadoOperacion = base.Insert(entity);
                }

                if (!resultadoOperacion.Ok)
                {
                    resultado.Copy(resultadoOperacion.Errores);
                    return resultado;
                }

                resultado.Return = resultadoOperacion.Return;

            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<RequerimientoFavoritoPorUsuario> MarcarFavorito(Comando_RequerimientoFavoritoPorUsuario comando)
        {
            var resultado = new Result<RequerimientoFavoritoPorUsuario>();

            try
            {

                //Usuario
                var resultadoUsuario = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetById(comando.IdUser);
                if (!resultadoUsuario.Ok)
                {
                    resultado.Copy(resultadoUsuario.Errores);
                    return resultado;
                }

                if (resultadoUsuario.Return == null || resultadoUsuario.Return.FechaBaja != null)
                {
                    resultado.AddErrorPublico("El ususario no existe o esta dado de baja");
                    return resultado;
                }
                var usuario = resultadoUsuario.Return;


                //Requerimiento
                var resultadoRQ = new RequerimientoRules(getUsuarioLogueado()).GetById(comando.IdRequerimiento);
                if (!resultadoRQ.Ok)
                {
                    resultado.Copy(resultadoRQ.Errores);
                    return resultado;
                }

                if (resultadoRQ.Return == null || resultadoRQ.Return.FechaBaja != null)
                {
                    resultado.AddErrorPublico("El requerimiento no existe o esta dado de baja");
                    return resultado;
                }

                var requerimiento = resultadoRQ.Return;


                var resultadoConsulta = GetByFilters(new Consulta_RequerimientoFavoritoPorUsuario()
                {
                    IdUser = comando.IdUser,
                    IdRequerimiento = comando.IdRequerimiento
                });

                if (!resultadoConsulta.Ok)
                {
                    resultado.Copy(resultadoConsulta.Errores);
                    return resultado;
                }

                RequerimientoFavoritoPorUsuario entity = null;

                if (resultadoConsulta.Return == null || resultadoConsulta.Return.Count() == 0)
                {
                    entity = new RequerimientoFavoritoPorUsuario();
                }
                else
                {
                    entity = resultadoConsulta.Return[0];
                }

                if (comando.Favorito)
                {
                    entity.FechaBaja = null;
                }
                else
                {
                    entity.FechaBaja = DateTime.Now;
                }

                entity.User = resultadoUsuario.Return;
                entity.Requerimiento = resultadoRQ.Return;

                Result<RequerimientoFavoritoPorUsuario> resultadoOperacion;

                if (entity.Id != 0)
                {
                    resultadoOperacion = base.Update(entity);
                }
                else
                {
                    resultadoOperacion = base.Insert(entity);
                }

                if (!resultadoOperacion.Ok)
                {
                    resultado.Copy(resultadoOperacion.Errores);
                    return resultado;
                }

                resultado.Return = resultadoOperacion.Return;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }


        public Result<List<Requerimiento>> GetRequerimientosByFilters(Consulta_RequerimientoFavoritoPorUsuario consulta)
        {
            return dao.GetRequerimientoByFilters(consulta);
        }

        public Result<List<Resultado_Requerimiento>> GetResultadoRequerimientosByFilters(Consulta_RequerimientoFavoritoPorUsuario consulta)
        {
            var ids = dao.GetIdsRequerimientoByFilters(consulta);
            return new RequerimientoRules(getUsuarioLogueado()).GetResultadoByIds(ids.Return);
        }

        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTablaRequerimientosByFilters(Consulta_RequerimientoFavoritoPorUsuario consulta)
        {
            var ids = dao.GetIdsRequerimientoByFilters(consulta);
            return new RequerimientoRules(getUsuarioLogueado()).GetResultadoTablaByIds(ids.Return);
        }

        public Result<int> GetCantidadRequerimientosByFilters(Consulta_RequerimientoFavoritoPorUsuario consulta)
        {
            return dao.GetCantidadRequerimientosByFilters(consulta);
        }




    }
}
