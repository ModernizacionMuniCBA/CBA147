using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using System.Configuration;
using Model.Consultas;

namespace Rules.Rules
{
    public class _CerrojoAreaRules : BaseRules<CerrojoArea>
    {

        private readonly CerrojoAreaDAO dao;

        public _CerrojoAreaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = CerrojoAreaDAO.Instance;
        }

        //protected override void CorregirDatos(CerrojoArea entity)
        //{
        //    base.CorregirDatos(entity);
        //    entity.Nombre = entity.Nombre.ToUpper();
        //}

        //public override Result<int> BuscarCantidadDuplicados(CerrojoArea entity)
        //{
        //    var result = new Result<int>();

        //    var resultConsulta = GetByFilters(entity.Nombre, entity.FechaBaja != null);
        //    if (!resultConsulta.Ok)
        //    {
        //        result.Copy(resultConsulta.Errores);
        //        return result;
        //    }

        //    result.Return = resultConsulta.Return.Count();
        //    return result;
        //}

        //public override string MensajeDuplicado(Area entity)
        //{
        //    return "Ya existe un Area con el nombre: " + entity.Nombre;
        //}

        //public override Result<Area> ValidateDatosNecesarios(Area entity)
        //{
        //    var result = base.ValidateDatosNecesarios(entity);

        //    //Nombre
        //    if (string.IsNullOrEmpty(entity.Nombre))
        //    {
        //        result.AddErrorPublico("Debe ingresar el nombre");
        //        return result;
        //    }

        //    return result;
        //}


        public Result<List<CerrojoArea>> GetByFilters(Consulta_Area consulta)
        {
            return dao.GetByFilters(consulta);
        }
    }
}
