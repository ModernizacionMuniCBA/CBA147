using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Model.Consultas;
using Model.Comandos;
using System.Configuration;

namespace Rules.Rules
{
    public class NotificacionParaUsuarioRules : BaseRules<NotificacionSistema>
    {
        private readonly NotificacionParaUsuarioDAO dao;

        public NotificacionParaUsuarioRules(UsuarioLogueado data)
            : base(data)
        {
            dao = NotificacionParaUsuarioDAO.Instance;
        }

        #region Validaciones




        public override Result<NotificacionSistema> ValidateDatosNecesarios(NotificacionSistema entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Titulo
            if (string.IsNullOrEmpty(entity.Titulo))
            {
                result.AddErrorPublico("Debe ingresar el nombre");
                return result;
            }

            //Contenido
            if (string.IsNullOrEmpty(entity.Contenido))
            {
                result.AddErrorPublico("Debe ingresar el key Alias");
            }

            return result;
        }

        #endregion

        public Result<List<Resultado_NotificacionSistema>> GetResultadoByFilters(Consulta_NotificacionParaUsuario consulta)
        {
            return dao.GetResultadoByFilters(consulta);
        }

        public Result<List<NotificacionSistema>> GetByFilters(Consulta_NotificacionParaUsuario consulta)
        {
            return dao.GetByFilters(consulta);
        }
        public Result<int> GetCantidadByFilters(Consulta_NotificacionParaUsuario consulta)
        {
            return dao.GetCantidadByFilters(consulta);
        }

        public Result<Resultado_NotificacionSistema> Insertar(Comando_NotificacionParaUsuario comando)
        {
            var resultado = new Result<Resultado_NotificacionSistema>();
            try
            {
                NotificacionSistema entity = new NotificacionSistema(comando);
                var resultadoInsertar = base.Insert(entity);
                if (!resultadoInsertar.Ok)
                {
                    resultado.Errores.Copy(resultadoInsertar.Errores);
                    return resultado;
                }

                resultado.Return = new Resultado_NotificacionSistema(resultadoInsertar.Return);
                return resultado;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<Resultado_NotificacionSistema> Editar(Comando_NotificacionParaUsuario comando)
        {
            var resultado = new Result<Resultado_NotificacionSistema>();
            try
            {
                if (!comando.Id.HasValue)
                {
                    resultado.AddErrorPublico("Debe indicar la notificacion a editar");
                    return resultado;
                }

                var resultadoConsulta = GetById(comando.Id.Value);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Errores.Copy(resultadoConsulta.Errores);
                    return resultado;
                }

                var entity = resultadoConsulta.Return;
                if (entity == null)
                {
                    resultado.AddErrorPublico("La notificacion no existe");
                    return resultado;
                }

                if (entity.FechaBaja != null)
                {
                    resultado.AddErrorPublico("La notificacion se encuentra dado de baja");
                    return resultado;
                }

                entity.Titulo = comando.Titulo;
                entity.Contenido = comando.Contenido;
                entity.Notificar = comando.Notificar;

                var resultadoUpdate = base.Update(entity);
                if (!resultadoUpdate.Ok)
                {
                    resultado.Errores.Copy(resultadoUpdate.Errores);
                    return resultado;
                }

                resultado.Return = new Resultado_NotificacionSistema(resultadoUpdate.Return);
                return resultado;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<Resultado_NotificacionSistema> DarDeBaja(int id)
        {
            var resultado = new Result<Resultado_NotificacionSistema>();
            var resultadoDelete = DeleteById(id);
            if (!resultadoDelete.Ok)
            {
                resultado.Errores.Copy(resultadoDelete.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_NotificacionSistema(resultadoDelete.Return);
            return resultado;
        }

        public Result<Resultado_NotificacionSistema> DarDeAlta(int id)
        {
            var resultado = new Result<Resultado_NotificacionSistema>();

            var resultadoConsulta = GetById(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Errores.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var entity = resultadoConsulta.Return;
            if (entity == null)
            {
                resultado.AddErrorPublico("La notificacion no existe");
                return resultado;
            }

            if (entity.FechaBaja == null)
            {
                resultado.AddErrorPublico("La notificacion no se encuentra dada de baja");
                return resultado;
            }

            entity.FechaBaja = null;
            var resultadoUpdate = Update(entity);
            if (!resultadoUpdate.Ok)
            {
                resultado.Errores.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_NotificacionSistema(resultadoUpdate.Return);
            return resultado;
        }

        public Result<Resultado_NotificacionSistema> SetNotificar(int id, bool notificar)
        {
            var resultado = new Result<Resultado_NotificacionSistema>();

            var resultadoConsulta = GetById(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var entity = resultadoConsulta.Return;
            if (entity == null)
            {
                resultado.AddErrorPublico("La notificacion no existe");
                return resultado;
            }

            entity.Notificar = notificar;

            var resultadoUpdate = Update(entity);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_NotificacionSistema(resultadoUpdate.Return);
            return resultado;
        }

    }
}
