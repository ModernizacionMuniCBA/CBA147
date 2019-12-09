using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Comandos;
using Model.Resultados;

namespace Rules.Rules
{
    public class GrupoRubroMotivoRules : BaseRules<GrupoRubroMotivo>
    {

        private readonly GrupoRubroMotivoDAO dao;

        public GrupoRubroMotivoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = GrupoRubroMotivoDAO.Instance;
        }

        public Result<Resultado_GrupoCategoriaMotivo> Insertar(Comando_GrupoRubroMotivo comando)
        {
            var result = new Result<Resultado_GrupoCategoriaMotivo>();

            var tipo = new GrupoRubroMotivo();
            tipo.Nombre = comando.Nombre.ToUpper();

            var resultValidar = Equals(tipo);

            if (!resultValidar.Ok)
            {
                result.AddErrorPublico("Error al insertar el grupo");
                return result;
            }

            if (resultValidar.Return)
            {
                result.AddErrorPublico("Ya existe un grupo de categorías con el nombre'" + tipo.Nombre + "'");
                return result;
            }

            var resultInsert = Insert(tipo);
            if (!resultInsert.Ok)
            {
                result.AddErrorPublico("Error al insertar el grupo de categorías");
                return result;
            }

            result.Return = new Resultado_GrupoCategoriaMotivo(resultInsert.Return);
            return result;
        }

        public Result<Resultado_GrupoCategoriaMotivo> Update(Comando_GrupoRubroMotivo comando)
        {
            var result = new Result<Resultado_GrupoCategoriaMotivo>();

            var resultConsulta = GetById((int)comando.Id);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al actualizar el grupo");
                return result;
            }

            var tipo = resultConsulta.Return;
            tipo.Nombre = comando.Nombre.ToUpper();

            var resultValidar = Equals(tipo);

            if (!resultValidar.Ok)
            {
                result.AddErrorPublico("Error al actualizar el grupo");
                return result;
            }

            var resultUpdate = Update(tipo);
            if (!resultUpdate.Ok)
            {
                result.AddErrorPublico("Error al actualizar el grupo");
                return result;
            }

            result.Return = new Resultado_GrupoCategoriaMotivo(resultUpdate.Return);
            return result;

        }

        public Result<Resultado_GrupoCategoriaMotivo> Delete(Comando_GrupoRubroMotivo comando)
        {
            var result = new Result<Resultado_GrupoCategoriaMotivo>();

            var resultConsulta = GetById((int)comando.Id);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al actualizar el grupo");
                return result;
            }

            var resultUpdate = Delete(resultConsulta.Return);
            if (!resultUpdate.Ok)
            {
                result.AddErrorPublico("Error al actualizar el grupo");
                return result;
            }

            result.Return = new Resultado_GrupoCategoriaMotivo(resultUpdate.Return);
            return result;
        }


        public Result<bool> Equals(GrupoRubroMotivo obj)
        {
            return dao.Equals(obj);
        }

    }
}
