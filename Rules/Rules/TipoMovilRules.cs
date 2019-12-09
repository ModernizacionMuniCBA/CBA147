using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;

namespace Rules.Rules
{
    public class TipoMovilRules : BaseRules<TipoMovil>
    {

        private readonly TipoMovilDAO dao;

        public TipoMovilRules(UsuarioLogueado data)
            : base(data)
        {
            dao = TipoMovilDAO.Instance;
        }

        public Result<List<Resultado_TipoMovil>> GetAll(bool dadosDeBaja)
        {
            var result = new Result<List<Resultado_TipoMovil>>();
            var resultConsulta = dao.GetAll(dadosDeBaja);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            result.Return = Resultado_TipoMovil.ToList(resultConsulta.Return);
            return result;
        }

        public Result<Resultado_TipoMovil> Insert(Model.Comandos.Comando_TipoMovil comando)
        {
            var result = new Result<Resultado_TipoMovil>();

            var tipo = new TipoMovil();
            tipo.Nombre = comando.Nombre.ToUpper();

            var resultValidar = Equals(tipo);

            if (!resultValidar.Ok)
            {
                result.AddErrorPublico("Error al insertar el tipo de móvil");
                return result;
            }

            if (resultValidar.Return)
            {
                result.AddErrorPublico("Ya existe un tipo de móvil con el nombre '" + tipo.Nombre + "'");
                return result;
            }

            var resultInsert= Insert(tipo);
            if (!resultInsert.Ok)
            {
                result.AddErrorPublico("Error al insertar el tipo de móvil");
                return result;
            }

            result.Return = new Resultado_TipoMovil(resultInsert.Return);
            return result;
        }

        public Result<bool> Equals(TipoMovil obj)
        {
            return dao.Equals(obj);
        }

        public Result<Resultado_TipoMovil> Update(Model.Comandos.Comando_TipoMovil comando)
        {
            var result = new Result<Resultado_TipoMovil>();

            var resultConsulta= GetById((int)comando.Id);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al actualizar el tipo de móvil");
                return result;
            }

            var tipo = resultConsulta.Return;
            tipo.Nombre = comando.Nombre.ToUpper();

            var resultValidar = Equals(tipo);

            if (!resultValidar.Ok)
            {
                result.AddErrorPublico("Error al actualizar el tipo de móvil");
                return result;
            }

            var resultUpdate = Update(tipo);
            if (!resultUpdate.Ok)
            {
                result.AddErrorPublico("Error al actualizar el tipo de móvil");
                return result;
            }

            result.Return = new Resultado_TipoMovil(resultUpdate.Return);
            return result;

        }

        public Result<Resultado_TipoMovil> Delete(Model.Comandos.Comando_TipoMovil comando)
        {
            var result = new Result<Resultado_TipoMovil>();

            var resultMoviles = new MovilRules(getUsuarioLogueado()).HayMovilesConTipo((int)comando.Id);
            if (!resultMoviles.Ok)
            {
                result.AddErrorPublico("Error al actualizar el tipo de móvil");
                return result;
            }

            if (resultMoviles.Return)
            {
                result.AddErrorPublico("No se puede eliminar el tipo de móvil ya que existen móviles que lo usan");
                return result;
            }

            var resultConsulta= GetById((int)comando.Id);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al actualizar el tipo de móvil");
                return result;
            }

            var resultUpdate = Delete( resultConsulta.Return);
            if (!resultUpdate.Ok)
            {
                result.AddErrorPublico("Error al actualizar el tipo de móvil");
                return result;
            }

            result.Return = new Resultado_TipoMovil(resultUpdate.Return);
            return result;
        }

    }
}
