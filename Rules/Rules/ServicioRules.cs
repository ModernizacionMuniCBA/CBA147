using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Comandos;
using Model.Resultados;
using Model.Consultas;

namespace Rules.Rules
{
    public class ServicioRules : BaseRules<Servicio>
    {
        private readonly ServicioDAO dao;

        public ServicioRules(UsuarioLogueado data)
            : base(data)
        {
            dao = ServicioDAO.Instance;
        }

        protected override void CorregirDatos(Servicio entity)
        {
            base.CorregirDatos(entity);
            entity.Nombre = entity.Nombre.ToUpper();
        }

        public override Result<int> BuscarCantidadDuplicados(Servicio entity)
        {
            var result = new Result<int>();

            int? id = null;
            if (entity.Id != 0)
            {
                id = entity.Id;
            }
            var resultConsulta = dao.GetCantidadDuplicados(id, entity.Nombre, entity.FechaBaja != null);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta);
                return result;
            }

            result.Return = resultConsulta.Return;
            return result;
        }

        public override string MensajeDuplicado(Servicio entity)
        {
            return "Ya existe un servicio con el nombre: " + entity.Nombre;
        }

        public override Result<Servicio> ValidateDatosNecesarios(Servicio entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Nombre
            if (string.IsNullOrEmpty(entity.Nombre))
            {
                result.AddErrorPublico("Debe ingresar el nombre");
            }

            return result;
        }

        public Result<List<Servicio>> GetByFilters(List<Enums.TipoMotivo> tiposMotivo, bool? dadosDeBaja)
        {
            return dao.GetByFilters(tiposMotivo, dadosDeBaja);
        }
        public Result<Servicio> GetByMotivo(int idMotivo, bool? dadosDeBaja)
        {
            return dao.GetByMotivo(idMotivo, dadosDeBaja);
        }

        public Result<Resultado_Servicio> Insertar(Comando_Servicio comando)
        {
            var result = new Result<Resultado_Servicio>();
            var servicio = new Servicio();
            servicio.Nombre = comando.Nombre;
            servicio.Observaciones = comando.Observaciones;
            servicio.Principal = comando.Principal;
            servicio.Icono = comando.Icono;
            servicio.UrlIcono = comando.UrlIcono;
            servicio.Color = comando.Color;


            var resultInsert = base.Insert(servicio);
            if (!resultInsert.Ok)
            {
                result.Copy(resultInsert.Errores);
                return result;
            }

            result.Return = new Resultado_Servicio(resultInsert.Return);
            return result;
        }

        public Result<Resultado_Servicio> Editar(Comando_Servicio comando)
        {
            var result = new Result<Resultado_Servicio>();

            try
            {
                var resultadoConsulta = GetByIdObligatorio(comando.Id.Value);
                if (!resultadoConsulta.Ok)
                {
                    result.Copy(resultadoConsulta.Errores);
                    return result;
                }

                var servicio = resultadoConsulta.Return;
                servicio.Nombre = comando.Nombre;
                servicio.Observaciones = comando.Observaciones;
                servicio.Principal = comando.Principal;
                servicio.Icono = comando.Icono;
                servicio.UrlIcono = comando.UrlIcono;
                servicio.Color = comando.Color;

                var resultInsert = base.Update(servicio);
                if (!resultInsert.Ok)
                {
                    result.Copy(resultInsert.Errores);
                    return result;
                }

                result.Return = new Resultado_Servicio(resultInsert.Return);
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<List<int>> GetIdsAreasById(Consulta_Area consulta)
        {
            var result = new Result<List<int>>();
            var resultadoConsulta=new _CerrojoAreaRules(getUsuarioLogueado()).GetByFilters(consulta);
            if (!resultadoConsulta.Ok)
            {
                result.Copy(resultadoConsulta.Errores);
                return result;
            }

            result.Return = resultadoConsulta.Return.Select(x => x.Id).ToList();
            return result;
        }
    }
}
