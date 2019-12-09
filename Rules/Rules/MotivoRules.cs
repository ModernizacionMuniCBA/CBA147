using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Model.Comandos;
using Model.Consultas;

namespace Rules.Rules
{
    public class MotivoRules : BaseRules<Motivo>
    {


        private readonly MotivoDAO dao;

        public MotivoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = MotivoDAO.Instance;
        }

        public override Result<Motivo> ValidateDatosNecesarios(Motivo entity)
        {
            var result = new Result<Motivo>();

            //Nombre
            if (string.IsNullOrEmpty(entity.Nombre))
            {
                result.AddErrorPublico("Debe ingresar el nombre");
                return result;
            }

            if (entity.Tema == null)
            {
                result.AddErrorPublico("Debe ingresar el tema");
                return result;
            }

            if (entity.Area == null)
            {
                result.AddErrorPublico("Debe ingresar el area");
                return result;
            }


            return result;
        }

        public Result< bool > Equals(Motivo entity)
        {
            return dao.Equals(entity);
        }
        public Result<Motivo> Validate(Motivo entity)
        {
            var result = new Result<Motivo>();
            var resultadoEquals = Equals(entity);

            if (!resultadoEquals.Ok)
            {
                result.AddErrorPublico("Error procesando la solicitud");
                return result;
            }

            if (resultadoEquals.Return)
            {
                result.AddErrorPublico("Ya existe un motivo con el nombre " + entity.Nombre);
                return result;
            }

            return ValidateDatosNecesarios(entity);
        }

        public Result<List<Resultado_Motivo>> GetByFilters(Consulta_Motivo consulta)
        {
            var result = new Result<List<Resultado_Motivo>>();
            var resultConsulta=dao.GetByFilters(consulta);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            result.Return = Resultado_Motivo.ToList(resultConsulta.Return);
            return result;
        }

        //public Result<List<Motivo>> GetByFilters(Enums.TipoMotivo tipo, bool? dadosDeBaja)
        //{

        //    return dao.GetByFilters(tipo, dadosDeBaja);
        //}

        public Result<List<Motivo>>GetDeServicio(int idServicio, bool? urgentes, bool? interno, bool? dadosDeBaja)
        {
            return dao.GetDeServicio(idServicio, urgentes, Enums.TipoMotivo.GENERAL, dadosDeBaja);
        }

        public Result<List<Motivo>> GetDeServicio(int idServicio, bool? urgentes, Enums.TipoMotivo? tipo, bool? dadosDeBaja)
        {
            return dao.GetDeServicio(idServicio, urgentes, tipo, dadosDeBaja);
        }

        public Result<List<Motivo>> GetByArea(int idArea, Enums.TipoMotivo? tipo, bool? dadosDeBaja)
        {
            return dao.GetByArea(idArea, tipo, dadosDeBaja);
        }


        public Result<List<Resultado_ServicioAreaMotivo>> GetInfo()
        {
            return dao.GetInfo(getUsuarioLogueado().Areas.Select(x=>x.Id).ToList());
        }

        public Result<Resultado_Servicio> GetServicioByIdArea(int idArea)
        {
            return dao.GetServicioByIdArea(idArea);
        }

        public Result<List<Resultado_CategoriaMotivoArea>> GetCategoriasByIdArea(int idArea)
        {
            return new CategoriaMotivoAreaRules(getUsuarioLogueado()).GetByIdArea(idArea);
        }

        public Result<Resultado_Motivo> Insertar(Comando_Motivo comando)
        {
            var resultado = new Result<Resultado_Motivo>();

            try
            {
                dao.Transaction(() =>
                {
                    CerrojoArea area;
                    var resultadoArea = new _CerrojoAreaRules(getUsuarioLogueado()).GetByIdObligatorio(comando.IdArea);
                    if (!resultadoArea.Ok)
                    {
                        resultado.Copy(resultadoArea.Errores);
                        return false;
                    }

                    area = resultadoArea.Return;
                    if (area == null)
                    {
                        resultado.AddErrorPublico("El area indicada no existe");
                        return false;
                    }

                    CategoriaMotivoArea categoria;
                    if (comando.IdCategoria.HasValue)
                    {
                        //Busco la categoría
                        var resultadoCategoria = new CategoriaMotivoAreaRules(getUsuarioLogueado()).GetById(comando.IdCategoria.Value);
                        if (!resultadoCategoria.Ok)
                        {
                            resultado.Copy(resultadoCategoria.Errores);
                            return false;
                        }

                        //Compruebo que la categoria sea del árera
                        if (resultadoCategoria.Return.Area != area)
                        {
                            resultado.AddErrorPublico("La categoría seleccionada no corresponde al área del motivo");
                            return false;
                        }

                        categoria = resultadoCategoria.Return;
                    }
                    else
                    {
                        categoria = null;
                    }

                    //Busco el el servicio
                    var resultadoServicio = GetServicioByIdArea(area.Id);
                    if (!resultadoServicio.Ok)
                    {
                        resultado.Copy(resultadoServicio.Errores);
                        return false;
                    }

                    Servicio servicio;
                    if (resultadoServicio.Return == null)
                    {
                        if (!comando.IdServicio.HasValue)
                        {
                            resultado.AddErrorPublico("Debe indicar el servicio de su motivo");
                            return false;
                        }

                        var resultadoConsultaNuevoServicio = new ServicioRules(getUsuarioLogueado()).GetByIdObligatorio(comando.IdServicio.Value);
                        if (!resultadoConsultaNuevoServicio.Ok)
                        {
                            resultado.Copy(resultadoConsultaNuevoServicio.Errores);
                            return false;
                        }

                        servicio = resultadoConsultaNuevoServicio.Return;
                    }
                    else
                    {
                        var resultadoConsultaNuevoServicio = new ServicioRules(getUsuarioLogueado()).GetByIdObligatorio(resultadoServicio.Return.Id);
                        if (!resultadoConsultaNuevoServicio.Ok)
                        {
                            resultado.Copy(resultadoConsultaNuevoServicio.Errores);
                            return false;
                        }

                        servicio = resultadoConsultaNuevoServicio.Return;
                    }

                    if (servicio == null)
                    {
                        resultado.AddErrorPublico("El servicio indicado no existe");
                        return false;
                    }


                    //Busco el tema
                    var resultadoTema = new TemaRules(getUsuarioLogueado()).GetByFilters(servicio.Id, null, false);
                    if (!resultadoTema.Ok)
                    {
                        resultado.Copy(resultadoTema.Errores);
                        return false;
                    }

                    Tema tema;
                    if (resultadoTema.Return == null || resultadoTema.Return.Count == 0)
                    {
                        Tema temaNuevo = new Tema();
                        temaNuevo.Nombre = "General";
                        temaNuevo.Servicio = servicio;
                        var resultadoInsertarTema = new TemaRules(getUsuarioLogueado()).Insert(temaNuevo);
                        if (!resultadoInsertarTema.Ok)
                        {
                            resultado.Copy(resultadoInsertarTema.Errores);
                            return false;
                        }

                        tema = resultadoInsertarTema.Return;
                    }
                    else
                    {
                        tema = resultadoTema.Return[0];
                    }

                    if (tema == null)
                    {
                        resultado.AddErrorPublico("Error procesando la solicitud");
                        return false;
                    }

                    Motivo motivo = new Motivo();
                    motivo.Tema = tema;
                    motivo.Nombre = comando.Nombre;
                    motivo.Categoria = categoria;
                    motivo.Observaciones = comando.Descripcion;
                    motivo.Area = area;
                    motivo.Keywords = comando.Keywords;
                    motivo.Prioridad = comando.Prioridad;
                    motivo.Urgente = comando.Urgente;
                    motivo.Tipo = comando.Tipo;
                    motivo.Principal = comando.Principal;
                    motivo.Esfuerzo = comando.Esfuerzo;

                    var resultValidar=Validate(motivo);
                    if(!resultValidar.Ok){
                        resultado.Copy(resultValidar.Errores);
                        return false;
                    }

                    var resultadoInsertar = base.Insert(motivo);
                    if (!resultadoInsertar.Ok)
                    {
                        resultado.Copy(resultadoInsertar.Errores);
                        return false;
                    }

                    resultado.Return = new Resultado_Motivo(resultadoInsertar.Return);
                    return true;
                });
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }
        public Result<Resultado_Motivo> Editar(Comando_Motivo comando)
        {
            var resultado = new Result<Resultado_Motivo>();

            try
            {
                dao.Transaction(() =>
                {

                    //Busco el motivo
                    var resultadoMotivo = GetByIdObligatorio(comando.Id.Value);
                    if (!resultadoMotivo.Ok)
                    {
                        resultado.Copy(resultadoMotivo.Errores);
                        return false;
                    }

                    var motivo = resultadoMotivo.Return;


                    //Area
                    CerrojoArea area;
                    var resultadoArea = new _CerrojoAreaRules(getUsuarioLogueado()).GetByIdObligatorio(comando.IdArea);
                    if (!resultadoArea.Ok)
                    {
                        resultado.Copy(resultadoArea.Errores);
                        return false;
                    }

                    area = resultadoArea.Return;
                    if (area == null)
                    {
                        resultado.AddErrorPublico("El area indicada no existe");
                        return false;
                    }

                    CategoriaMotivoArea categoria;
                    if (comando.IdCategoria.HasValue)
                    {
                        //Busco la categoría
                        var resultadoCategoria = new CategoriaMotivoAreaRules(getUsuarioLogueado()).GetById(comando.IdCategoria.Value);
                        if (!resultadoCategoria.Ok)
                        {
                            resultado.Copy(resultadoCategoria.Errores);
                            return false;
                        }

                        //Compruebo que la categoria sea del árera
                        if (resultadoCategoria.Return.Area != area)
                        {
                            resultado.AddErrorPublico("La categoría seleccionada no corresponde al área del motivo");
                            return false;
                        }

                        categoria = resultadoCategoria.Return;
                    }
                    else
                    {
                        categoria = null;
                    }

                    //Busco el el servicio
                    var resultadoServicio = GetServicioByIdArea(area.Id);
                    if (!resultadoServicio.Ok)
                    {
                        resultado.Copy(resultadoServicio.Errores);
                        return false;
                    }

                    Servicio servicio;
                    if (resultadoServicio.Return == null)
                    {
                        if (!comando.IdServicio.HasValue)
                        {
                            resultado.AddErrorPublico("Debe indicar el servicio de su motivo");
                            return false;
                        }

                        var resultadoConsultaNuevoServicio = new ServicioRules(getUsuarioLogueado()).GetByIdObligatorio(comando.IdServicio.Value);
                        if (!resultadoConsultaNuevoServicio.Ok)
                        {
                            resultado.Copy(resultadoConsultaNuevoServicio.Errores);
                            return false;
                        }

                        servicio = resultadoConsultaNuevoServicio.Return;
                    }
                    else
                    {
                        var resultadoConsultaNuevoServicio = new ServicioRules(getUsuarioLogueado()).GetByIdObligatorio(resultadoServicio.Return.Id);
                        if (!resultadoConsultaNuevoServicio.Ok)
                        {
                            resultado.Copy(resultadoConsultaNuevoServicio.Errores);
                            return false;
                        }

                        servicio = resultadoConsultaNuevoServicio.Return;
                    }

                    if (servicio == null)
                    {
                        resultado.AddErrorPublico("El servicio indicado no existe");
                        return false;
                    }


                    //Busco el tema
                    var resultadoTema = new TemaRules(getUsuarioLogueado()).GetByFilters(servicio.Id, null, false);
                    if (!resultadoTema.Ok)
                    {
                        resultado.Copy(resultadoTema.Errores);
                        return false;
                    }

                    Tema tema;
                    if (resultadoTema.Return == null || resultadoTema.Return.Count == 0)
                    {
                        Tema temaNuevo = new Tema();
                        temaNuevo.Nombre = "General";
                        temaNuevo.Servicio = servicio;
                        var resultadoInsertarTema = new TemaRules(getUsuarioLogueado()).Insert(temaNuevo);
                        if (!resultadoInsertarTema.Ok)
                        {
                            resultado.Copy(resultadoInsertarTema.Errores);
                            return false;
                        }

                        tema = resultadoInsertarTema.Return;
                    }
                    else
                    {
                        tema = resultadoTema.Return[0];
                    }

                    if (tema == null)
                    {
                        resultado.AddErrorPublico("Error procesando la solicitud");
                        return false;
                    }

                    motivo.Tema = tema;
                    motivo.Categoria = categoria;
                    motivo.Nombre = comando.Nombre;
                    motivo.Observaciones = comando.Descripcion;
                    motivo.Area = area;
                    motivo.Keywords = comando.Keywords;
                    motivo.Urgente = comando.Urgente;
                    motivo.Tipo = comando.Tipo;
                    motivo.Principal = comando.Principal;
                    motivo.Prioridad = comando.Prioridad;
                    motivo.Esfuerzo = comando.Esfuerzo;

                    var resultValidar=Validate(motivo);
                    if(!resultValidar.Ok){
                        resultado.Copy(resultValidar.Errores);
                        return false;
                    }

                    var resultadoInsertar = base.Update(motivo);
                    if (!resultadoInsertar.Ok)
                    {
                        resultado.Copy(resultadoInsertar.Errores);
                        return false;
                    }

                    resultado.Return = new Resultado_Motivo(resultadoInsertar.Return);
                    return true;
                });
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }


        //public Result<List<ResultadoApp_ServicioMotivoParaBusqueda>> GetParaBusqueda()
        //{
        //    return dao.GetParaBusqueda();
        //}

        public Result<Resultado_CampoPorMotivo> InsertarCampo(Comando_Motivo_Campo comando)
        {
            var resultado = new Result<Resultado_CampoPorMotivo>();

            try
            {
                dao.Transaction(() =>
                {

                    //Busco el motivo
                    var resultadoMotivo = GetByIdObligatorio(comando.IdMotivo);
                    if (!resultadoMotivo.Ok)
                    {
                        resultado.Copy(resultadoMotivo.Errores);
                        return false;
                    }

                    var motivo = resultadoMotivo.Return;

                    //Campos
                    var campoPorMotivoRules = new CampoPorMotivoRules(getUsuarioLogueado());
                    var campo = new CampoPorMotivo();
                    campo.Nombre = comando.Nombre;
                    campo.Motivo = motivo;

                    var resultExiste = campoPorMotivoRules.Equals(campo);
                    if (!resultExiste.Ok)
                    {
                        resultado.AddErrorPublico(resultExiste.Error);
                        return false;
                    }

                    if (resultExiste.Return)
                    {
                        resultado.AddErrorPublico("Ya existe un campo llamado '" + comando.Nombre + "' para el motivo '" + motivo.Nombre + "'");
                        return false;
                    }

                    campo.Orden = comando.Orden;
                    campo.Observaciones = comando.Observaciones;

                    var resultTipo = new TipoCampoPorMotivoRules(getUsuarioLogueado()).GetById(comando.IdTipoCampoPorMotivo);
                    if (!resultTipo.Ok)
                    {
                        resultado.AddErrorPublico(resultTipo.Error);
                        return false;
                    }

                    campo.Grupo = comando.Grupo;
                    if (String.IsNullOrWhiteSpace(campo.Grupo))
                    {
                        campo.Grupo = null;
                    }
                    campo.Tipo = resultTipo.Return;

                    if (resultTipo.Return.KeyValue == Enums.TipoCampoPorMotivo.SELECTOR)
                    {
                        var stringOpciones = "";
                        var primero = true;
                        foreach (string o in comando.Opciones)
                        {
                            if (!primero) stringOpciones += "^";
                            stringOpciones += o;
                            primero = false;
                        }

                        campo.Opciones = stringOpciones;
                    }

                    if (resultTipo.Return.KeyValue != Enums.TipoCampoPorMotivo.LEYENDA)
                    {
                        campo.Obligatorio = comando.Obligatorio;
                    }

                    var resultInsert = campoPorMotivoRules.Insert(campo);
                    if (!resultInsert.Ok)
                    {
                        resultado.Copy(resultInsert.Errores);
                        return false;
                    }

                    resultado.Return = new Resultado_CampoPorMotivo(resultInsert.Return);
                    return true;
                });

                return resultado;
            }
            catch (Exception e)
            {
                resultado.AddErrorPublico("Error procesando la solicitud");
                resultado.AddErrorInterno(e.Message.ToString());
                return resultado;
            }
        }

        public Result<Resultado_CampoPorMotivo> EditarCampo(Comando_Motivo_Campo comando)
        {
            var resultado = new Result<Resultado_CampoPorMotivo>();

            try
            {
                dao.Transaction(() =>
                {

                    var campoPorMotivoRules = new CampoPorMotivoRules(getUsuarioLogueado());
                    //Busco el campo
                    var resultadoCampo = campoPorMotivoRules.GetByIdObligatorio((int)comando.Id);
                    if (!resultadoCampo.Ok)
                    {
                        resultado.Copy(resultadoCampo.Errores);
                        return false;
                    }

                    var campo = resultadoCampo.Return;
                    campo.Nombre = comando.Nombre;

                    var resultValidar = campoPorMotivoRules.Equals(campo);
                    if (!resultValidar.Ok)
                    {
                        resultado.AddErrorPublico(resultValidar.Error);
                        return false;
                    }

                    campo.Grupo = comando.Grupo;
                    campo.Orden = comando.Orden;
                    campo.Obligatorio = comando.Obligatorio;
                    campo.Observaciones = comando.Observaciones;

                    if (campo.Tipo.KeyValue == Enums.TipoCampoPorMotivo.SELECTOR)
                    {
                        var stringOpciones = "";
                        var primero = true;
                        foreach (string o in comando.Opciones)
                        {
                            if (!primero) stringOpciones += "^";
                            stringOpciones += o;

                            primero = false;
                        }

                        campo.Opciones = stringOpciones;
                    }

                    var resultInsert = campoPorMotivoRules.Update(campo);
                    if (!resultInsert.Ok)
                    {
                        resultado.Copy(resultInsert.Errores);
                        return false;
                    }

                    resultado.Return = new Resultado_CampoPorMotivo(resultInsert.Return);
                    return true;
                });

                return resultado;
            }
            catch (Exception e)
            {
                resultado.AddErrorPublico("Error procesando la solicitud");
                resultado.AddErrorInterno(e.Message.ToString());
                return resultado;
            }
        }

        public Result<Resultado_CampoPorMotivo> EliminarCampo(Comando_Motivo_Campo comando)
        {
            var resultado = new Result<Resultado_CampoPorMotivo>();

            try
            {
                dao.Transaction(() =>
                {
                    //Campos
                    var campoPorMotivoRules = new CampoPorMotivoRules(getUsuarioLogueado());

                    //Busco el campo
                    var resultadoCampo = campoPorMotivoRules.GetByIdObligatorio((int)comando.Id);
                    if (!resultadoCampo.Ok)
                    {
                        resultado.Copy(resultadoCampo.Errores);
                        return false;
                    }

                    var resultDelete = campoPorMotivoRules.Delete(resultadoCampo.Return);
                    if (!resultDelete.Ok)
                    {
                        resultado.Copy(resultDelete.Errores);
                        return false;
                    }

                    resultado.Return = new Resultado_CampoPorMotivo(resultDelete.Return);
                    return true;
                });

                return resultado;
            }
            catch (Exception e)
            {
                resultado.AddErrorPublico("Error procesando la solicitud");
                resultado.AddErrorInterno(e.Message.ToString());
                return resultado;
            }
        }

        public Result<Resultado_DataInicialControlMotivos> GetDataInicialControlMotivos(Enums.TipoMotivo tipo, bool modoBusqueda)
        {
            var result = new Result<Resultado_DataInicialControlMotivos>();
            result.Return = new Resultado_DataInicialControlMotivos();
            var tipos = new List<Enums.TipoMotivo>();
            tipos.Add(tipo);

            var resultServicios= new ServicioRules(getUsuarioLogueado()).GetByFilters(tipos, false);
            if (!resultServicios.Ok)
            {
                result.Copy(resultServicios.Errores);
                return result;
            }

            if (!(tipo==Enums.TipoMotivo.GENERAL && !modoBusqueda))
            {
                var resultAreas = new _CerrojoAreaRules(getUsuarioLogueado()).GetByFilters(new Consulta_Area() { Tipos = tipos });
                if (!resultAreas.Ok)
                {
                    result.Copy(resultAreas.Errores);
                    return result;
                }

                result.Return.Areas = Resultado_Area.ToList(resultAreas.Return);
            }

            result.Return.Servicios = Resultado_Servicio.ToList(resultServicios.Return);
            return result;
        }
    }
}
