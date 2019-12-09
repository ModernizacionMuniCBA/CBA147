using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Comandos;

namespace Rules.Rules
{
    public class VersionSistemaRules : BaseRules<VersionSistema>
    {


        private readonly VersionSistemaDAO dao;

        public VersionSistemaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = VersionSistemaDAO.Instance;
        }

        public Result<VersionSistema> Get()
        {
            return dao.Get();
        }

        public Result<string> GetVersion()
        {
            var resultado = new Result<string>();

            var resultadoConsulta = Get();
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return == null)
            {
                resultado.Return = null;
                return resultado;
            }

            resultado.Return = resultadoConsulta.Return.Version;
            return resultado;
        }

        public override Result<VersionSistema> ValidateDatosNecesarios(VersionSistema entity)
        {
            var result = base.ValidateDatosNecesarios(entity);
            if (!result.Ok) return result;

            if (string.IsNullOrEmpty(entity.Version))
            {
                result.AddErrorPublico("Debe ingresar la versión");
                return result;
            }


            if (string.IsNullOrEmpty(entity.Observaciones))
            {
                result.AddErrorPublico("Debe ingresar una descripción");
                return result;
            }

            return result;
        }

        public Result<bool> Insertar(Comando_VersionSistema comando)
        {
            var resultado = new Result<bool>();

            //Valido permisos
            if (getUsuarioLogueado().Rol.Rol != "Administrador")
            {
                resultado.AddErrorPublico("No tiene permisos para esta operación");
                return resultado;
            }

            VersionSistema entity = new VersionSistema();
            entity.Version = comando.Version;
            entity.Observaciones = comando.Descripcion;

            var resultadoInsercion = Insert(entity);
            if (!resultadoInsercion.Ok)
            {
                resultado.Copy(resultadoInsercion.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }
    }
}
