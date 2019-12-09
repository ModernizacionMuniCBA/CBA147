using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;

namespace Rules.Rules
{
    public class CategoriaMotivoAreaRules  : BaseRules<CategoriaMotivoArea>
    {

        private readonly CategoriaMotivoAreaDAO dao;

        public CategoriaMotivoAreaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = CategoriaMotivoAreaDAO.Instance;
        }

        public Result<List<Resultado_CategoriaMotivoArea>> GetAll(bool dadosDeBaja)
        {
            var result = new Result<List<Resultado_CategoriaMotivoArea>>();
            var resultConsulta = dao.GetAll(dadosDeBaja);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            result.Return = Resultado_CategoriaMotivoArea.ToList(resultConsulta.Return);
            return result;
        }

        public Result<Resultado_CategoriaMotivoArea> Insert(Model.Comandos.Comando_CategoriaMotivoArea comando)
        {
            var result = new Result<Resultado_CategoriaMotivoArea>();

            var categoria = new CategoriaMotivoArea();
            categoria.Nombre = comando.Nombre.ToUpper();

            var resultArea = new _CerrojoAreaRules(getUsuarioLogueado()).GetById(comando.IdArea);
            if (!resultArea.Ok)
            {
                result.Copy(resultArea.Errores);
                result.AddErrorPublico("Error al buscar el área");
                return result;
            }

            categoria.Area = resultArea.Return;

            var resultValidar = Equals(categoria);
            if (!resultValidar.Ok)
            {
                result.AddErrorPublico("Error al insertar la categoría de motivo");
                return result;
            }

            if (resultValidar.Return)
            {
                result.AddErrorPublico("Ya existe una categoría con el nombre '" + categoria.Nombre + " para el área seleccionada'");
                return result;
            }

            var resultInsert = Insert(categoria);
            if (!resultInsert.Ok)
            {
                result.AddErrorPublico("Error al insertar la categoría");
                return result;
            }

            result.Return = new Resultado_CategoriaMotivoArea(resultInsert.Return);
            return result;
        }

        public Result<bool> Equals(CategoriaMotivoArea obj)
        {
            return dao.Equals(obj);
        }

        public Result<Resultado_CategoriaMotivoArea> Update(Model.Comandos.Comando_CategoriaMotivoArea comando)
        {
            var result = new Result<Resultado_CategoriaMotivoArea>();

            var resultConsulta= GetById((int)comando.Id);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al actualizar la categoría");
                return result;
            }

            var tipo = resultConsulta.Return;
            tipo.Nombre = comando.Nombre.ToUpper();

            var resultValidar = Equals(tipo);

            if (!resultValidar.Ok)
            {
                result.AddErrorPublico("Error al actualizar la categoría");
                return result;
            }

            var resultUpdate = Update(tipo);
            if (!resultUpdate.Ok)
            {
                result.AddErrorPublico("Error al actualizar la categoría");
                return result;
            }

            result.Return = new Resultado_CategoriaMotivoArea(resultUpdate.Return);
            return result;

        }

        public Result<Resultado_CategoriaMotivoArea> Delete(int id)
        {
            var result = new Result<Resultado_CategoriaMotivoArea>();

            var resultConsulta = GetById(id);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al actualizar la categoría");
                return result;
            }

            var resultUpdate = Delete(resultConsulta.Return);
            if (!resultUpdate.Ok)
            {
                result.AddErrorPublico("Error al actualizar la categoría");
                return result;
            }

            result.Return = new Resultado_CategoriaMotivoArea(resultUpdate.Return);
            return result;
        }


        public Result<List<Resultado_CategoriaMotivoArea>> GetByIdArea(int idArea)
        {
            var result = new Result<List<Resultado_CategoriaMotivoArea>>();
            var resultado = dao.GetByIdArea(idArea);
            if (!resultado.Ok)
            {
                result.Copy(resultado.Errores);
                return result;
            }

            result.Return = Resultado_CategoriaMotivoArea.ToList(resultado.Return);
            return result;
        }
    }
}
