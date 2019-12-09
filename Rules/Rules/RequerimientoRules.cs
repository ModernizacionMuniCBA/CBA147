using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using DAO.DAO;
using Model;
using Model.Entities;
using NHibernate;
using Rules.Rules.Reportes;
using Telerik.Reporting.Processing;
using Model.Resultados;
using Rules.Rules.Mails;
using Model.Consultas;
using Model.Comandos;
using System.Configuration;
using DAO;
using System.Globalization;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace Rules.Rules
{
    public class RequerimientoRules : BaseRules<Requerimiento>
    {

        private readonly RequerimientoDAO dao;

        public RequerimientoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = RequerimientoDAO.Instance;
        }


        private static Random random = new Random();


        //Insertar
        public Result<Resultado_Requerimiento> Insertar(Comando_RequerimientoIntranet comando)
        {
            var resultado = new Result<Resultado_Requerimiento>();

            dao.Transaction(() =>
            {
                try
                {
                    var entity = new Requerimiento();

                    //Tipo
                    entity.Tipo = new TipoRules(getUsuarioLogueado()).GetByFilters(null, Enums.TipoRequerimiento.RECLAMO, false).Return[0];

                    //Motivo
                    var resultadoMotivo = new MotivoRules(getUsuarioLogueado()).GetById(comando.IdMotivo);
                    if (!resultadoMotivo.Ok)
                    {
                        resultado.Copy(resultadoMotivo.Errores);
                        return false;
                    }
                    entity.Motivo = resultadoMotivo.Return;

                    //Relevamiento interno
                    entity.RelevamientoInterno = getUsuarioLogueado().Usuario.Empleado;

                    //Prioridad
                    if (!Enum.IsDefined(typeof(Enums.PrioridadRequerimiento), (int)resultadoMotivo.Return.Prioridad))
                    {
                        entity.Prioridad = Enums.PrioridadRequerimiento.NORMAL;
                    }
                    else
                    {
                        entity.Prioridad = resultadoMotivo.Return.Prioridad;
                    }

                    //Usuario creador
                    entity.UsuarioCreador = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id).Return;

                    //Seteo los datos del numero
                    var numero = GenerarNumeroIdentificatorio();
                    entity.Año = int.Parse(numero[0].ToString());
                    entity.Numero = numero[1].ToString();

                    //Domicilio
                    var domicilioRules = new DomicilioRules(getUsuarioLogueado());
                    var resultadoDomicilio = domicilioRules.Buscar(comando.Domicilio.Latitud, comando.Domicilio.Longitud);
                    if (!resultadoDomicilio.Ok)
                    {
                        resultado.Copy(resultadoDomicilio.Errores);
                        return false;
                    }

                    if (resultadoDomicilio.Return == null || resultadoDomicilio.Return.Cpc == null || resultadoDomicilio.Return.Barrio == null)
                    {
                        resultado.AddErrorPublico("Domicilio inválido");
                        return false;
                    }


                    Domicilio domicilio = new Domicilio();
                    domicilio.Cpc = new CpcRules(getUsuarioLogueado()).GetByIdCatastro(resultadoDomicilio.Return.Cpc.IdCatastro).Return;
                    if (domicilio.Cpc == null)
                    {
                        resultado.AddErrorPublico("El cpc no existe");
                        return false;
                    }

                    domicilio.Barrio = new BarrioRules(getUsuarioLogueado()).GetByIdCatastro(resultadoDomicilio.Return.Barrio.IdCatastro).Return;
                    if (domicilio.Barrio == null)
                    {
                        resultado.AddErrorPublico("El barrio no existe");
                        return false;
                    }

                    domicilio.Observaciones = comando.Domicilio.Observaciones;

                    if (!string.IsNullOrEmpty(comando.Domicilio.Direccion))
                    {
                        domicilio.Sugerido = false;
                        domicilio.Direccion = comando.Domicilio.Direccion;
                    }
                    else
                    {
                        domicilio.Distancia = resultadoDomicilio.Return.Distancia;
                        domicilio.Sugerido = true;
                        domicilio.Direccion = resultadoDomicilio.Return.Direccion;
                    }

                    if (string.IsNullOrEmpty(domicilio.Direccion))
                    {
                        resultado.AddErrorPublico("Debe indicar la direccion del domicilio");
                        return false;
                    }

                    domicilio.Latitud = ("" + comando.Domicilio.Latitud).Replace(".", ",");
                    domicilio.Longitud = ("" + comando.Domicilio.Longitud).Replace(".", ",");

                    var resultDomicilio = domicilioRules.Insert(domicilio);
                    if (!resultDomicilio.Ok)
                    {
                        resultado.Copy(resultDomicilio.Errores);
                        return false;
                    }
                    entity.Domicilio = resultDomicilio.Return;

                    //Seteo al requerimiento el numero del cpc del domicilio
                    if (resultDomicilio.Return != null && resultDomicilio.Return.Cpc != null)
                    {
                        entity.NumeroCpc = resultDomicilio.Return.Cpc.Numero;
                    }
                    else
                    {
                        entity.NumeroCpc = null;
                    }

                    //Origen
                    if (string.IsNullOrEmpty(comando.OrigenAlias) || string.IsNullOrEmpty(comando.OrigenSecret))
                    {
                        resultado.AddErrorPublico("Debe indicar el origen");
                        return false;
                    }

                    var resultadoOrigen = new OrigenRules(getUsuarioLogueado()).GetByFilters(new Consulta_Origen()
                    {
                        KeyAlias = comando.OrigenAlias,
                        KeySecret = comando.OrigenSecret
                    });
                    if (!resultadoOrigen.Ok)
                    {
                        resultado.Errores.Copy(resultadoOrigen.Errores);
                        return false;
                    }

                    if (resultadoOrigen.Return == null || resultadoOrigen.Return.Count() == 0 || resultadoOrigen.Return[0].FechaBaja != null)
                    {
                        resultado.AddErrorPublico("El origen indicado no existe");
                        return false;
                    }

                    entity.Origen = new OrigenRules(getUsuarioLogueado()).GetById(resultadoOrigen.Return[0].Id).Return;


                    //Le pongo estado Nuevo
                    var resultEstado = ProcesarCambioEstado(entity, Enums.EstadoRequerimiento.NUEVO, "Creado", false);
                    if (!resultEstado.Ok)
                    {
                        resultado.Copy(resultEstado.Errores);
                        return false;
                    }

                    //Si mando estado lo muevo a ese estado
                    if (comando.EstadoKeyValue.HasValue && comando.EstadoKeyValue.Value != Enums.EstadoRequerimiento.NUEVO)
                    {
                        var resultadoPermiso = new PermisoEstadoRequerimientoRules(getUsuarioLogueado()).TienePermiso(comando.EstadoKeyValue.Value, Enums.PermisoEstadoRequerimiento.VerEnRequerimientoInternoNuevo);
                        if (!resultadoPermiso.Ok)
                        {
                            resultado.Copy(resultadoPermiso.Errores);
                            return false;
                        }

                        if (!resultadoPermiso.Return)
                        {
                            resultado.AddErrorPublico("Estado inválido");
                            return false;
                        }

                        if (string.IsNullOrEmpty(comando.EstadoMotivo))
                        {
                            resultado.AddErrorPublico("Debe indicar el motivo del estado");
                            return false;
                        }

                        var resultEstadoNuevo = ProcesarCambioEstado(entity, comando.EstadoKeyValue.Value, comando.EstadoMotivo, false);
                        if (!resultEstadoNuevo.Ok)
                        {
                            resultado.Copy(resultEstadoNuevo.Errores);
                            return false;
                        }
                    }


                    entity.UserAgent = comando.UserAgent;
                    entity.TipoDispositivo = comando.TipoDispositivo;

                    //area responsable
                    if (entity.Motivo.Area.Subareas != null && entity.Motivo.Area.Subareas.Count != 0)
                    {
                        foreach (CerrojoArea subarea in entity.Motivo.Area.Subareas)
                        {
                            if (subarea.TerritorioIncumbencia.EstaEnMiTerritorio(comando.Domicilio.Latitud, comando.Domicilio.Longitud))
                            {
                                entity.AreaResponsable = subarea;
                                break;
                            }
                        }
                    }

                    if (entity.AreaResponsable == null)
                    {
                        entity.AreaResponsable = entity.Motivo.Area;
                    }

                    if (comando.ReferenteProvisorio != null)
                    {
                        var referente = new ReferenteProvisorio();
                        referente.Apellido = comando.ReferenteProvisorio.Apellido;
                        referente.Nombre = comando.ReferenteProvisorio.Nombre;
                        referente.DNI = comando.ReferenteProvisorio.DNI;
                        referente.GeneroMasculino = comando.ReferenteProvisorio.GeneroMasculino;
                        referente.Telefono = comando.ReferenteProvisorio.Telefono;
                        referente.Observaciones = comando.ReferenteProvisorio.Observaciones;

                        var resultReferenteProvisorio = new ReferenteProvisorioRules(getUsuarioLogueado()).Insert(referente);
                        if (!resultReferenteProvisorio.Ok)
                        {
                            resultado.Copy(resultReferenteProvisorio.Errores);
                            return false;
                        }

                        entity.ReferenteProvisorio = referente;
                    }

                    //Lo marco o no
                    if (getUsuarioLogueado().EsAmbitoCPC())
                    {
                        var resultadoAreas = new _CerrojoAmbitoRules(getUsuarioLogueado()).GetIdsAreas(getUsuarioLogueado().Ambito.Id);
                        if (!resultadoAreas.Ok)
                        {
                            resultado.Copy(resultadoAreas.Errores);
                            return false;
                        }

                        if (resultadoAreas.Return.Contains(entity.AreaResponsable.Id))
                        {
                            entity.Marcado = true;
                        }
                    }

                    //Inserto
                    var resultadoInsertRequerimiento = base.Insert(entity);
                    if (!resultadoInsertRequerimiento.Ok)
                    {
                        resultado.Copy(resultadoInsertRequerimiento.Errores);
                        return false;
                    }


                    var user = new UsuarioReferentePorRequerimiento();
                    //Usuario referente
                    if (comando.IdUsuarioReferente.HasValue || entity.Motivo.Tipo == Enums.TipoMotivo.INTERNO)
                    {
                        var idUsuario = comando.IdUsuarioReferente;
                        if (entity.Motivo.Tipo == Enums.TipoMotivo.INTERNO)
                        {
                            idUsuario = getUsuarioLogueado().Usuario.Id;
                        }

                        var resultadoConsultaUsuario = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetById(idUsuario.Value);
                        if (!resultadoConsultaUsuario.Ok)
                        {
                            resultado.Copy(resultadoConsultaUsuario.Errores);
                            return false;
                        }

                        if (resultadoConsultaUsuario.Return == null)
                        {
                            resultado.AddErrorPublico("El usuario referente indicado no existe");
                            return false;
                        }

                        var usuarios = new List<UsuarioReferentePorRequerimiento>();
                        user.UsuarioReferente = resultadoConsultaUsuario.Return;
                        user.Observaciones = comando.ObservacionesUsuarioReferente;
                        user.Requerimiento = resultadoInsertRequerimiento.Return;

                        var resultInsertReferente = new UsuarioReferentePorRequerimientoRules(getUsuarioLogueado()).Insert(user);
                        if (!resultInsertReferente.Ok)
                        {
                            resultado.AddErrorPublico("Error al insertar al usuario referente");
                            return false;
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(comando.Descripcion))
                    {
                        //Descripcion
                        var descripcion = new DescripcionPorRequerimiento();
                        descripcion.Descripcion = comando.Descripcion;
                        descripcion.UsuarioReferente = user.UsuarioReferente;
                        descripcion.Requerimiento = resultadoInsertRequerimiento.Return;

                        var resultDescripcion = new DescripcionPorRequerimientoRules(getUsuarioLogueado()).Insert(descripcion);
                        if (!resultDescripcion.Ok)
                        {
                            resultado.AddErrorPublico("Error al insertar al usuario referente");
                            return false;
                        }
                    }

                    if (comando.CamposDinamicos == null)
                    {
                        comando.CamposDinamicos = new List<Model.Comandos.Comando_RequerimientoIntranet.Comando_CampoDinamico>();
                    }
                    //Campos dinámicos
                    foreach (Model.Comandos.Comando_RequerimientoIntranet.Comando_CampoDinamico comandoCampo in comando.CamposDinamicos)
                    {
                        var resultConsultaCampo = new CampoPorMotivoRules(getUsuarioLogueado()).GetById(comandoCampo.Id);
                        if (!resultConsultaCampo.Ok)
                        {
                            resultado.Copy(resultConsultaCampo.Errores);
                            return false;
                        }

                        var campoXRQ = new CampoPorMotivoPorRequerimiento();
                        campoXRQ.CampoPorMotivo = resultConsultaCampo.Return;
                        campoXRQ.Requerimiento = resultadoInsertRequerimiento.Return;
                        campoXRQ.Valor = comandoCampo.Valor;

                        var resultInsertCampo = new CampoPorMotivoPorRequerimientoRules(getUsuarioLogueado()).Insert(campoXRQ);
                        if (!resultInsertCampo.Ok)
                        {
                            resultado.Copy(resultInsertCampo.Errores);
                            return false;
                        }
                    }

                    //Archivos (va despues del insert porque esta relacionado al requerimiento y debo tener la roreign key)
                    var archivoRules = new ArchivoPorRequerimientoRules(getUsuarioLogueado());
                    if (comando.IdArchivos == null)
                    {
                        comando.IdArchivos = new List<int>();
                    }
                    foreach (int idArchivo in comando.IdArchivos)
                    {
                        var resultadoArchivo = archivoRules.GetById(idArchivo);
                        if (!resultadoArchivo.Ok)
                        {
                            resultado.Copy(resultadoArchivo.Errores);
                            return false;
                        }
                        var archivo = resultadoArchivo.Return;
                        archivo.Requerimiento = entity;
                        archivo.UsuarioReferente = user.UsuarioReferente;

                        //Actualizo el archivo
                        var resultadoUpdateArchivo = archivoRules.Update(archivo);
                        if (!resultadoUpdateArchivo.Ok)
                        {
                            resultado.Copy(resultadoUpdateArchivo.Errores);
                            return false;
                        }
                    }

                    //Imagen (viene desde internet)
                    if (!String.IsNullOrEmpty(comando.Imagen))
                    {
                        var rules = new ArchivoPorRequerimientoRules(getUsuarioLogueado());
                        var resultadoInsert = rules.Insertar(new Comando_Archivo()
                        {
                            Data = comando.Imagen,
                            Nombre = "Imagen subida desde App"
                        });

                        if (!resultadoInsert.Ok)
                        {
                            resultado.Copy(resultadoInsert.Errores);
                            return false;
                        }

                        var archivo = rules.GetByIdObligatorio(resultadoInsert.Return).Return;
                        archivo.Requerimiento = entity;
                        archivo.UsuarioReferente = user.UsuarioReferente;
                        var resultadoUpdate = rules.Update(archivo);
                        if (!resultadoUpdate.Ok)
                        {
                            resultado.Copy(resultadoUpdate.Errores);
                            return false;
                        }

                        List<ArchivoPorRequerimiento> archivos = new List<ArchivoPorRequerimiento>();
                        archivos.Add(archivo);
                        entity.Archivos = archivos;
                    }

                    //Notas (va despues del insert porque el rq debe estar insertado para poder usar la foreign key)
                    var notaPorRequerimientoRules = new NotaPorRequerimientoRules(getUsuarioLogueado());
                    if (comando.Notas == null)
                    {
                        comando.Notas = new List<Comando_Nota>();
                    }
                    entity.Notas = new List<NotaPorRequerimiento>();

                    foreach (var comandoNota in comando.Notas)
                    {
                        NotaPorRequerimiento nota = new NotaPorRequerimiento();
                        nota.Requerimiento = entity;
                        nota.Observaciones = comandoNota.Contenido;

                        //Valido el insert
                        var resultadoInsertNota = notaPorRequerimientoRules.Insert(nota);
                        if (!resultadoInsertNota.Ok)
                        {
                            resultado.Copy(resultadoInsertNota.Errores);
                            return false;
                        }
                    }

                    //devuelvo el resultado del requerimiento creado
                    Resultado_Requerimiento rq = new Resultado_Requerimiento(resultadoInsertRequerimiento.Return);
                    resultado.Return = rq;
                    return resultado.Ok;
                }
                catch (Exception e)
                {
                    resultado.AddErrorPublico(e.Message != null ? e.Message : "Error procesando la solicitud");
                    return false;
                }

            });

            return resultado;
        }

        public Result<Resultado_Requerimiento> UnirseARequerimiento(Comando_RequerimientoIntranet comando)
        {
            var resultado = new Result<Resultado_Requerimiento>();

            dao.Transaction(() =>
            {
                try
                {
                    if (!comando.IdRequerimientoUnir.HasValue)
                    {
                        resultado.AddErrorPublico("Debe indicar el requerimiento al cual unirse");
                        return false;
                    }

                    //consulto el rq al que me voy a unir
                    var resultadoRq = GetById((int)comando.IdRequerimientoUnir);
                    if (!resultadoRq.Ok)
                    {
                        resultado.Copy(resultadoRq.Errores);
                        return false;
                    }

                    var entity = resultadoRq.Return;
                    if (entity.UsuariosReferentes.Where(x => x.UsuarioReferente.Id == comando.IdUsuarioReferente.Value).FirstOrDefault() != null)
                    {
                        resultado.AddErrorPublico("El usuario ya es referente del reclamo seleccionado.");
                        return false;
                    }

                    //Origen
                    if (string.IsNullOrEmpty(comando.OrigenAlias) || string.IsNullOrEmpty(comando.OrigenSecret))
                    {
                        resultado.AddErrorPublico("Debe indicar el origen");
                        return false;
                    }

                    var resultadoOrigen = new OrigenRules(getUsuarioLogueado()).GetByFilters(new Consulta_Origen()
                    {
                        KeyAlias = comando.OrigenAlias,
                        KeySecret = comando.OrigenSecret
                    });
                    if (!resultadoOrigen.Ok)
                    {
                        resultado.Errores.Copy(resultadoOrigen.Errores);
                        return false;
                    }

                    if (resultadoOrigen.Return == null || resultadoOrigen.Return.Count() == 0 || resultadoOrigen.Return[0].FechaBaja != null)
                    {
                        resultado.AddErrorPublico("El origen indicado no existe");
                        return false;
                    }

                    //entity.Origen = new OrigenRules(getUsuarioLogueado()).GetById(resultadoOrigen.Return[0].Id).Return;

                    //entity.UserAgent = comando.UserAgent;
                    //entity.TipoDispositivo = comando.TipoDispositivo;
                    //entity.Descripcion = comando.Descripcion;

                    //Le hago un update al rq
                    var resultadoInsertRequerimiento = base.Update(entity);
                    if (!resultadoInsertRequerimiento.Ok)
                    {
                        resultado.Copy(resultadoInsertRequerimiento.Errores);
                        return false;
                    }

                    var resultadoConsultaUsuario = new Result<_VecinoVirtualUsuario>();
                    //Usuario referente
                    if (comando.IdUsuarioReferente.HasValue)
                    {
                        var idUsuario = comando.IdUsuarioReferente;
                        if (entity.Motivo.Tipo == Enums.TipoMotivo.INTERNO)
                        {
                            idUsuario = getUsuarioLogueado().Usuario.Id;
                        }

                        resultadoConsultaUsuario = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetById(idUsuario.Value);
                        if (!resultadoConsultaUsuario.Ok)
                        {
                            resultado.Copy(resultadoConsultaUsuario.Errores);
                            return false;
                        }

                        if (resultadoConsultaUsuario.Return == null)
                        {
                            resultado.AddErrorPublico("El usuario referente indicado no existe");
                            return false;
                        }

                        var user = new UsuarioReferentePorRequerimiento();
                        user.UsuarioReferente = resultadoConsultaUsuario.Return;
                        user.Requerimiento = resultadoInsertRequerimiento.Return;

                        var resultInsertReferente = new UsuarioReferentePorRequerimientoRules(getUsuarioLogueado()).Insert(user);
                        if (!resultInsertReferente.Ok)
                        {
                            resultado.AddErrorPublico("Error al insertar al usuario referente");
                            return false;
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(comando.Descripcion))
                    {
                        //Descripcion
                        var descripcion = new DescripcionPorRequerimiento();
                        descripcion.Descripcion = comando.Descripcion;
                        descripcion.UsuarioReferente = resultadoConsultaUsuario.Return;
                        descripcion.Requerimiento = resultadoInsertRequerimiento.Return;

                        var resultDescripcion = new DescripcionPorRequerimientoRules(getUsuarioLogueado()).Insert(descripcion);
                        if (!resultDescripcion.Ok)
                        {
                            resultado.AddErrorPublico("Error al insertar al usuario referente");
                            return false;
                        }
                    }

                    //Archivos (va despues del insert porque esta relacionado al requerimiento y debo tener la roreign key)
                    var archivoRules = new ArchivoPorRequerimientoRules(getUsuarioLogueado());
                    if (comando.IdArchivos == null)
                    {
                        comando.IdArchivos = new List<int>();
                    }
                    foreach (int idArchivo in comando.IdArchivos)
                    {
                        var resultadoArchivo = archivoRules.GetById(idArchivo);
                        if (!resultadoArchivo.Ok)
                        {
                            resultado.Copy(resultadoArchivo.Errores);
                            return false;
                        }
                        var archivo = resultadoArchivo.Return;
                        archivo.Requerimiento = entity;
                        archivo.UsuarioReferente = resultadoConsultaUsuario.Return;

                        //Actualizo el archivo
                        var resultadoUpdateArchivo = archivoRules.Update(archivo);
                        if (!resultadoUpdateArchivo.Ok)
                        {
                            resultado.Copy(resultadoUpdateArchivo.Errores);
                            return false;
                        }
                    }

                    //Imagen (viene desde internet)
                    if (!String.IsNullOrEmpty(comando.Imagen))
                    {
                        var rules = new ArchivoPorRequerimientoRules(getUsuarioLogueado());
                        var resultadoInsert = rules.Insertar(new Comando_Archivo()
                        {
                            Data = comando.Imagen,
                            Nombre = "Imagen subida desde App"
                        });

                        if (!resultadoInsert.Ok)
                        {
                            resultado.Copy(resultadoInsert.Errores);
                            return false;
                        }

                        var archivo = rules.GetByIdObligatorio(resultadoInsert.Return).Return;
                        archivo.Requerimiento = entity;
                        archivo.UsuarioReferente = resultadoConsultaUsuario.Return;
                        var resultadoUpdate = rules.Update(archivo);
                        if (!resultadoUpdate.Ok)
                        {
                            resultado.Copy(resultadoUpdate.Errores);
                            return false;
                        }

                        if (entity.Archivos == null)
                        {
                            List<ArchivoPorRequerimiento> archivos = new List<ArchivoPorRequerimiento>();
                            archivos.Add(archivo);
                            entity.Archivos = archivos;
                        }
                        else
                        {
                            entity.Archivos.Add(archivo);
                        }
                    }

                    //Notas (va despues del insert porque el rq debe estar insertado para poder usar la foreign key)
                    var notaPorRequerimientoRules = new NotaPorRequerimientoRules(getUsuarioLogueado());
                    if (comando.Notas == null)
                    {
                        comando.Notas = new List<Comando_Nota>();
                    }

                    if (entity.Notas == null)
                    {
                        entity.Notas = new List<NotaPorRequerimiento>();
                    }

                    foreach (var comandoNota in comando.Notas)
                    {
                        NotaPorRequerimiento nota = new NotaPorRequerimiento();
                        nota.Requerimiento = entity;
                        nota.Observaciones = comandoNota.Contenido;

                        //Valido el insert
                        var resultadoInsertNota = notaPorRequerimientoRules.Insert(nota);
                        if (!resultadoInsertNota.Ok)
                        {
                            resultado.Copy(resultadoInsertNota.Errores);
                            return false;
                        }
                    }

                    //devuelvo el resultado del requerimiento creado
                    Resultado_Requerimiento rq = new Resultado_Requerimiento(resultadoInsertRequerimiento.Return);
                    resultado.Return = rq;
                    return resultado.Ok;
                }
                catch (Exception e)
                {
                    resultado.AddErrorPublico(e.Message != null ? e.Message : "Error procesando la solicitud");
                    return false;
                }

            });

            return resultado;
        }

        //Numeracion 
        public Result<bool> ExisteNumero(string numero, int año)
        {
            return dao.ExisteNumero(numero, año);
        }

        public object[] GenerarNumeroIdentificatorio()
        {
            int anio = DateTime.Now.Year;
            string numero = null;
            bool yaexiste = true;


            do
            {
                numero = RandomString(6).ToUpper();

                //Compruebo si ya existe el numero y sigo buscando de ser asi.
                var resultYaExiste = ExisteNumero(numero, anio);
                if (!resultYaExiste.Ok)
                {
                    yaexiste = true;
                }
                else
                {
                    yaexiste = resultYaExiste.Return;
                }

            } while (yaexiste);


            var num = new object[2];
            num[0] = anio;
            num[1] = numero;
            return num;
        }


        public static string RandomString(int length)
        {
            const string chars = "ACDEFGHJKLMNPQRSTUWXZ123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        #region Para estadisticas (Debe irse)
        /*
        public Result<List<int>> GetIds(Enums.TipoRequerimiento? tipo, List<Enums.EstadoRequerimiento> estados, string numero, int? anio, int? idServicio, int? idMotivo, int? idArea, bool? esPersonaFisica, int? idPersona, int? idUsuarioCerrojo, int? idCalle, int? idCpc, List<int> idsBarrio, DateTime? fechaDesde, DateTime? fechaHasta, bool? relevamientoInterno, List<Enums.PrioridadRequerimiento> prioridades, bool? dadosDeBaja, int? altura, bool? marcado)
        {
            return dao.GetIds(tipo, estados, numero, anio, idServicio, idMotivo, idArea, esPersonaFisica, idPersona, idUsuarioCerrojo, idCalle, idCpc, idsBarrio, fechaDesde, fechaHasta, relevamientoInterno, prioridades, dadosDeBaja, altura, marcado);
        }

        public Result<List<int>> GetIdsConDomicilio(Enums.TipoRequerimiento? tipo, List<Enums.EstadoRequerimiento> estados, bool? conDomicilio, int? idCpc, DateTime? fechaDesde, DateTime? fechaHasta, bool? relevamientoInterno, bool? dadosDeBaja)
        {
            return dao.GetIdsConDomicilio(tipo, estados, conDomicilio, idCpc, fechaDesde, fechaHasta, relevamientoInterno, dadosDeBaja);
        }*/

        #endregion


        #region Consultas

        public Result<Requerimiento> GetByNumero(string numero, int año)
        {
            return dao.GetByNumero(numero, año);
        }

        public Result<int> GetIdByNumero(string numero, int año)
        {
            return dao.GetIdByNumero(numero, año);
        }

        public Result<int> GetDiasDesdeCreacion(int idRequerimiento)
        {
            var resultado = new Result<int>();
            var resultRq = dao.GetById(idRequerimiento);

            if (!resultRq.Ok)
            {
                resultado.Copy(resultRq.Errores);
                return resultado;
            }

            var rq = resultRq.Return;
            var fechaHoy = DateTime.Now.Date;
            var fechaCreacion = rq.FechaAlta.Value;
            int diferencia = Math.Abs((fechaHoy - fechaCreacion).Days);

            if (diferencia != null && diferencia == 0)
            {
                resultado.Return = 0;
            }
            resultado.Return = diferencia;

            return resultado;
        }
        public Result<Resultado_Requerimiento> GetResultadoByNumero(string numero, int año)
        {
            var resultado = new Result<Resultado_Requerimiento>();

            var resultadoConsulta = GetIdByNumero(numero, año);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return == 0)
            {
                resultado.Return = null;
            }


            return GetResultadoById(resultadoConsulta.Return);
        }


        public Result<Resultado_RequerimientoDetalle2> GetDetalleById(int id)
        {
            var resultado = new Result<Resultado_RequerimientoDetalle2>();

            int? idUsuario = null;
            if (getUsuarioLogueado() != null && getUsuarioLogueado().Usuario != null)
            {
                idUsuario = getUsuarioLogueado().Usuario.Id;
            }

            //Detalle
            var resultadoDetalle = dao.GetDetalleById(id, idUsuario);
            if (!resultadoDetalle.Ok)
            {
                resultado.Copy(resultadoDetalle.Errores);
                return resultado;
            }
            resultado.Return = resultadoDetalle.Return;

            //Historial Estados
            var resultadoEstados = GetDetalleHistorialEstadosById(id);
            if (!resultadoEstados.Ok)
            {
                resultado.Copy(resultadoEstados.Errores);
                return resultado;
            }
            resultado.Return.Estados = resultadoEstados.Return;

            //Comentarios
            var resultadoComentarios = GetDetalleComentariosById(id);
            if (!resultadoComentarios.Ok)
            {
                resultado.Copy(resultadoComentarios.Errores);
                return resultado;
            }
            resultado.Return.Comentarios = resultadoComentarios.Return;

            //Ordenes 
            var resultadoOrdenes = GetDetalleHistoricoOrdenesTrabajoById(id);
            if (!resultadoOrdenes.Ok)
            {
                resultado.Copy(resultadoOrdenes.Errores);
                return resultado;
            }
            resultado.Return.OrdenesDeTrabajo = resultadoOrdenes.Return;

            ////Inspecciones 
            //var resultadoInspecciones = GetDetalleHistoricoOrdenesInspeccionById(id);
            //if (!resultadoInspecciones.Ok)
            //{
            //    resultado.Copy(resultadoInspecciones.Errores);
            //    return resultado;
            //}
            //resultado.Return.OrdenesDeInspeccion = resultadoInspecciones.Return;

            //Tareas
            var resultadoTareas = GetDetalleTareasById(id);
            if (!resultadoTareas.Ok)
            {
                resultado.Copy(resultadoTareas.Errores);
                return resultado;
            }
            resultado.Return.Tareas = resultadoTareas.Return;

            //Campos dinámicos
            var resultadoCampos = GetDetalleCamposDinamicosById(id);
            if (!resultadoTareas.Ok)
            {
                resultado.Copy(resultadoTareas.Errores);
                return resultado;
            }
            resultado.Return.CamposDinamicos = resultadoCampos.Return;

            //Usuarios referentes
            var resultadoUsuarios = GetDetalleUsuariosReferentes(id);
            if (!resultadoUsuarios.Ok)
            {
                resultado.Copy(resultadoUsuarios.Errores);
                return resultado;
            }
            resultado.Return.UsuariosReferentes = resultadoUsuarios.Return;


            return resultado;

        }

        public Result<List<Resultado_RequerimientoDetalle2_HistoricoEstados>> GetDetalleHistorialEstadosById(int id)
        {
            return dao.GetDetalleHistorialEstadosById(id);
        }

        public Result<List<Resultado_RequerimientoDetalle2_Comentario>> GetDetalleComentariosById(int id)
        {
            return dao.GetDetalleComentariosById(id);
        }

        public Result<List<Resultado_RequerimientoDetalle2_HistoricoOrdenesTrabajo>> GetDetalleHistoricoOrdenesTrabajoById(int id)
        {
            return dao.GetDetalleHistoricoOrdenesTrabajoById(id);
        }
        //public Result<List<Resultado_RequerimientoDetalle2_HistoricoOrdenesInspeccion>> GetDetalleHistoricoOrdenesInspeccionById(int id)
        //{
        //    return dao.GetDetalleHistoricoOrdenesInspeccionById(id);
        //}
        public Result<List<Resultado_RequerimientoDetalle2_Tarea>> GetDetalleTareasById(int id)
        {
            return dao.GetDetalleTareasById(id);
        }
        public Result<List<Resultado_RequerimientoDetalle2_UsuarioReferente>> GetDetalleUsuariosReferentes(int id)
        {
            return dao.GetDetalleUsuariosReferentes(id);
        }
        public Result<List<Resultado_RequerimientoDetalle2_CampoDinamico>> GetDetalleCamposDinamicosById(int id)
        {
            return dao.GetDetalleCamposDinamicosById(id);
        }

        //Consulta Ultimos
        public Result<List<int>> GetIdsUltimos(Consulta_Requerimiento consulta, int cantidad)
        {
            return dao.GetIdsUltimos(consulta, cantidad);
        }

        public Result<List<int>> GetIdsPeligrososUltimos(Consulta_Requerimiento consulta, int cantidad)
        {
            return dao.GetIdsPeligrososUltimos(consulta, cantidad);
        }

        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetUltimos(int cantidad)
        {
            var consulta = new Consulta_Requerimiento()
            {
                DadosDeBaja = false,
                IdsArea = getUsuarioLogueado().IdsAreas
            };

            return GetResultadoTablaByIds(GetIdsUltimos(consulta, cantidad).Return);
        }

        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetIdsPeligrososUltimos(int cantidad)
        {
            var consulta = new Consulta_Requerimiento()
            {
                DadosDeBaja = false,
                Urgente = true,
                OrdenAtencionCritica = false,
                IdsArea = getUsuarioLogueado().IdsAreas
            };
            consulta.EstadoKeyValue(Enums.EstadoRequerimiento.NUEVO);

            return GetResultadoTablaByIds(GetIdsPeligrososUltimos(consulta, cantidad).Return);
        }

        /*PARA MIGRAR*/
        //public bool migrarRequerimientosRecoleccionResiduos()
        //{
        //    var consulta = new Consulta_Requerimiento();
        //    var area = new List<int>();
        //    area.Add(1396);
        //    consulta.IdsArea = area;
        //    var requerimientos = GetByFilters(consulta, null);

        //    foreach (Requerimiento r in requerimientos.Return)
        //    {
        //        ProcesarCambioEstado(r, Enums.EstadoRequerimiento.COMPLETADO, "Previos al Dic/18 De Recolección De Residuos");
        //    }

        //    return true;
        //}

        //Consulta By Filters
        public Result<List<Requerimiento>> GetByFilters(Consulta_Requerimiento consulta, bool? marcado)
        {
            return dao.GetByFilters(consulta, marcado);
        }

        public Result<List<Resultado_Requerimiento>> GetResultadoByFilters(Consulta_Requerimiento consulta, bool? marcado)
        {
            var resultadoIds = GetIdsByFilters(consulta, marcado);
            return GetResultadoByIds(resultadoIds.Return);
        }

        public Result<List<int>> GetIdsByFilters(Consulta_Requerimiento consulta, bool? marcado)
        {
            return dao.GetIdsByFilters(consulta, marcado);
        }

        public Result<List<int>> GetIdsAreaByFilters(Consulta_Requerimiento consulta, bool? marcado)
        {
            return dao.GetIdsAreaByFilters(consulta, marcado);
        }

        public Result<List<int>> GetIdsTipoByFilters(Consulta_Requerimiento consulta, bool? marcado)
        {
            return dao.GetIdsTipoByFilters(consulta, marcado);
        }
        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTablaByFilters(Consulta_Requerimiento consulta, bool? marcado)
        {
            var resultado = new Result<ResultadoTabla<ResultadoTabla_Requerimiento>>();
            var resultadoIds = dao.GetIdsByFilters(consulta, marcado);
            if (!resultadoIds.Ok)
            {
                resultado.Copy(resultadoIds.Errores);
                return resultado;
            }

            return GetResultadoTablaByIds(resultadoIds.Return);
        }
        public Result<ResultadoTabla<ResultadoWSExterno_Requerimiento>> GetResultadoExternoByFilters(Consulta_Requerimiento consulta, bool? marcado)
        {
            var resultado = new Result<ResultadoTabla<ResultadoWSExterno_Requerimiento>>();
            var resultadoIds = dao.GetIdsByFilters(consulta, marcado);
            if (!resultadoIds.Ok)
            {
                resultado.Copy(resultadoIds.Errores);
                return resultado;
            }

            return GetResultadoExternoByIds(resultadoIds.Return);
        }

        private Result<Consulta_Requerimiento> TransformarConsultaBandeja(Consulta_Requerimiento_Bandeja consulta)
        {
            var resultado = new Result<Consulta_Requerimiento>();

            //si no tengo ambito o el keyvalue es 0, quiere decir que mi ambito es la muni, sino es un cpc
            if (getUsuarioLogueado().Ambito != null && getUsuarioLogueado().Ambito.KeyValue > 0)
            {
                consulta.KeyValueCPC = getUsuarioLogueado().Ambito.KeyValue;
            }

            //Valido que el area que manda sea una de las areas del usuario
            if (consulta.IdArea != 0)
            {
                if (!getUsuarioLogueado().Areas.Where(x => x.Id == consulta.IdArea).Any())
                {
                    resultado.AddErrorPublico("El área seleccionada no es correspondiente a su ámbito de trabajo.");
                    return resultado;
                }
            }

            //Si no hay estados, le mando los validos para crear ot
            var keyValue = Enums.PermisoEstadoRequerimiento.AgregarEnOrdenDeTrabajo;

            var consultaEstadosValidos = new Rules.PermisoEstadoRequerimientoRules(getUsuarioLogueado()).GetEstadosKeyValueByPermiso(keyValue);
            if (!consultaEstadosValidos.Ok)
            {
                resultado.Copy(consultaEstadosValidos.Errores);
                return resultado;
            }

            if (consulta.EstadosKeyValue == null || consulta.EstadosKeyValue.Count == 0)
            {
                consulta.EstadosKeyValue = consultaEstadosValidos.Return;
            }


            //Valido que todos los estados mandados formen parte de los estados validos para crear ot
            if (consulta.EstadosKeyValue.Except(consultaEstadosValidos.Return).Count() != 0)
            {
                resultado.AddErrorPublico("Ingreso algun estado inválido.");
                return resultado;
            }

            resultado.Return = new Consulta_Requerimiento(consulta);

            //Si no mande Area, quiere decir que es Todas las areas mias
            if (consulta.IdArea == 0)
            {
                resultado.Return.IdsArea = getUsuarioLogueado().IdsAreas;
            }
            return resultado;
        }

        private Result<List<int>> GetIdsParaOrdenBandeja(Consulta_Requerimiento_Bandeja consulta)
        {
            var resultado = new Result<List<int>>();

            var resultadoTransformar = TransformarConsultaBandeja(consulta);
            if (!resultadoTransformar.Ok)
            {
                resultado.Copy(resultadoTransformar.Errores);
                return resultado;
            }

            var marcado = false;

            //si no tengo ambito o el keyvalue es 0, quiere decir que mi ambito es la muni, sino es un cpc
            if (getUsuarioLogueado().Ambito != null && getUsuarioLogueado().Ambito.KeyValue != 0)
            {
                marcado = true;
            }


            return GetIdsByFilters(resultadoTransformar.Return, marcado);
        }

        private Result<List<int>> GetIdsAreaParaOrdenTrabajo(Consulta_Requerimiento_Bandeja consulta)
        {
            var resultado = new Result<List<int>>();

            var resultadoTransformar = TransformarConsultaBandeja(consulta);
            if (!resultadoTransformar.Ok)
            {
                resultado.Copy(resultadoTransformar.Errores);
                return resultado;
            }

            var marcado = false;

            //si no tengo ambito o el keyvalue es 0, quiere decir que mi ambito es la muni, sino es un cpc
            if (getUsuarioLogueado().Ambito != null && getUsuarioLogueado().Ambito.KeyValue != 0)
            {
                marcado = true;
            }


            return GetIdsAreaByFilters(resultadoTransformar.Return, marcado);
        }

        private Result<List<int>> GetIdsTipoParaOrdenTrabajo(Consulta_Requerimiento_Bandeja consulta)
        {
            var resultado = new Result<List<int>>();

            var resultadoTransformar = TransformarConsultaBandeja(consulta);
            if (!resultadoTransformar.Ok)
            {
                resultado.Copy(resultadoTransformar.Errores);
                return resultado;
            }

            var marcado = false;

            //si no tengo ambito o el keyvalue es 0, quiere decir que mi ambito es la muni, sino es un cpc
            if (getUsuarioLogueado().Ambito != null && getUsuarioLogueado().Ambito.KeyValue != 0)
            {
                marcado = true;
            }


            return GetIdsTipoByFilters(resultadoTransformar.Return, marcado);
        }

        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTablaParaBandeja(Consulta_Requerimiento_Bandeja consulta)
        {
            var resultadoConsulta = GetIdsParaOrdenBandeja(consulta);
            if (!resultadoConsulta.Ok)
            {
                var resultado = new Result<ResultadoTabla<ResultadoTabla_Requerimiento>>();
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }
            return GetResultadoTablaByIds(resultadoConsulta.Return);
        }

        public Result<List<Resultado_RequerimientoTopBarrios>> GetTopPorBarrio()
        {
            var consulta = new Consulta_RequerimientoTopBarrios();
            return GetTopPorBarrio(consulta);
        }

        public Result<List<Resultado_RequerimientoTopBarrios>> GetTopPorBarrio(Consulta_RequerimientoTopBarrios consulta = null)
        {
            if (consulta == null) consulta = new Consulta_RequerimientoTopBarrios();

            var resultado = new Result<List<Resultado_RequerimientoTopBarrios>>();

            //si no tengo ambito o el keyvalue es 0, quiere decir que mi ambito es la muni, sino es un cpc
            var marcado = false;
            int? cpc = null;
            if (getUsuarioLogueado().Ambito != null && getUsuarioLogueado().Ambito.KeyValue != 0)
            {
                marcado = true;
                if (getUsuarioLogueado().Ambito.KeyValue != -1)
                {
                    cpc = getUsuarioLogueado().Ambito.KeyValue;
                }
            }

            //Si no hay estados, le mando los validos para crear ot
            var consultaEstadosValidos = new Rules.PermisoEstadoRequerimientoRules(getUsuarioLogueado()).GetEstadosKeyValueByPermiso(Enums.PermisoEstadoRequerimiento.AgregarEnOrdenDeTrabajo);
            if (!consultaEstadosValidos.Ok)
            {
                resultado.Copy(consultaEstadosValidos.Errores);
                return resultado;
            }

            //si viene una zona como filtro, valido que sea de alguna de mis areas
            int? idArea = null;
            int? idZona = null;
            int? idCategoria = null;
            if (consulta.IdZona.HasValue)
            {
                var zonaConsulta = new ZonaRules(getUsuarioLogueado()).GetById((int)consulta.IdZona);
                if (!zonaConsulta.Ok)
                {
                    resultado.Copy(zonaConsulta.Errores);
                    resultado.AddErrorPublico("Error procesando la solicitud");
                    resultado.AddErrorInterno("Error al leer la zona.");
                    return resultado;
                }

                var zonaEnArea = getUsuarioLogueado().Areas.Select(x => x.Id == zonaConsulta.Return.Area.Id);
                if (zonaEnArea == null)
                {
                    resultado.AddErrorPublico("La Zona no pertenece a ninguna de sus áreas");
                    return resultado;
                }


                idZona = zonaConsulta.Return.Id;
            }

            idArea = consulta.IdArea;

            idCategoria = consulta.IdCategoria;
            resultado.Return = new List<Resultado_RequerimientoTopBarrios>();

            var areas = getUsuarioLogueado().Areas;

            //Busco
            var resultadoTop = dao.GetTop(consultaEstadosValidos.Return, areas.Select(x => x.IdCerrojo).ToList(), marcado, cpc, idArea, idZona, idCategoria);
            if (!resultadoTop.Ok)
            {
                resultado.Copy(resultadoTop.Errores);
                return resultado;
            }
            resultado.Return = resultadoTop.Return;


            return resultado;
        }

        public Result<List<Resultado_MarcadorGoogleMaps>> GetTopMarcadoresPorBarrio(Consulta_RequerimientoTopBarrios consulta = null)
        {
            if (consulta == null) consulta = new Consulta_RequerimientoTopBarrios();

            var resultado = new Result<List<Resultado_MarcadorGoogleMaps>>();

            //si no tengo ambito o el keyvalue es 0, quiere decir que mi ambito es la muni, sino es un cpc
            var marcado = false;
            int? cpc = null;
            if (getUsuarioLogueado().Ambito != null && getUsuarioLogueado().Ambito.KeyValue != 0)
            {
                marcado = true;
                if (getUsuarioLogueado().Ambito.KeyValue != -1)
                {
                    cpc = getUsuarioLogueado().Ambito.KeyValue;
                }
            }

            //Si no hay estados, le mando los validos para crear ot
            var consultaEstadosValidos = new Rules.PermisoEstadoRequerimientoRules(getUsuarioLogueado()).GetEstadosKeyValueByPermiso(Enums.PermisoEstadoRequerimiento.AgregarEnOrdenDeTrabajo);
            if (!consultaEstadosValidos.Ok)
            {
                resultado.Copy(consultaEstadosValidos.Errores);
                return resultado;
            }


            //si viene una zona como filtro, valido que sea de alguna de mis areas
            int? idArea = null;
            int? idZona = null;
            int? idCategoria = null;
            if (consulta.IdZona.HasValue)
            {
                var zonaConsulta = new ZonaRules(getUsuarioLogueado()).GetById((int)consulta.IdZona);
                if (!zonaConsulta.Ok)
                {
                    resultado.Copy(zonaConsulta.Errores);
                    resultado.AddErrorPublico("Error procesando la solicitud");
                    resultado.AddErrorInterno("Error al leer la zona.");
                    return resultado;
                }

                var zonaEnArea = getUsuarioLogueado().Areas.Select(x => x.Id == zonaConsulta.Return.Area.Id);
                if (zonaEnArea == null)
                {
                    resultado.AddErrorPublico("La Zona no pertenece a ninguna de sus áreas");
                    return resultado;
                }
                idCategoria = consulta.IdCategoria;
                idArea = zonaConsulta.Return.Area.Id;
                idZona = zonaConsulta.Return.Id;
            }

            var areas = getUsuarioLogueado().Areas;
            return dao.GetTopMarcadores(consultaEstadosValidos.Return, areas.Select(x => x.IdCerrojo).ToList(), marcado, cpc, idArea, idZona, idCategoria);
        }

        public Result<int> GetCantidadUrgentesNuevos()
        {

            var consulta = new Consulta_Requerimiento()
            {
                Urgente = true,
                EstadosKeyValue = new List<Enums.EstadoRequerimiento>() { Enums.EstadoRequerimiento.NUEVO, Enums.EstadoRequerimiento.PENDIENTE }
            };

            bool? marcado = false;
            if (getUsuarioLogueado().EsAmbitoCPC())
            {
                consulta.KeyValuesCPC = new List<int>() { getUsuarioLogueado().Ambito.KeyValue };
                marcado = true;
            }

            if (!getUsuarioLogueado().Areas.Any(x => x.CrearOrdenEspecial))
            {
                consulta.IdsArea = getUsuarioLogueado().IdsAreas;
            }
            else
            {
                marcado = null;
            }

            return GetCantidadByFilters(consulta, marcado);
        }

        public Result<int> GetCantidadByFilters(Consulta_Requerimiento consulta, bool? marcado)
        {
            return dao.GetCantidadByFilters(consulta, marcado);
        }

        public Result<List<Resultado_RequerimientoInfo>> GetInfoGlobal()
        {
            var resultado = new Result<List<Resultado_RequerimientoInfo>>();
            var resultadoPermiso = new CerrojoUsuarioEstadisticaTVRules(getUsuarioLogueado()).TengoPermiso();
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //if (!resultadoPermiso.Return)
            //{
            //    resultado.AddErrorPublico("No tiene permisos para realizar esta accion");
            //    return resultado;
            //}

            return dao.GetInfoGlobal();
        }

        //Cercanos

        int metrosDefault = 50;

        public Result<List<Requerimiento>> GetCercanos(Consulta_RequerimientoCercano consulta)
        {
            return dao.GetCercanos(consulta);
        }

        public Result<int> GetCantidadCercanos(Consulta_RequerimientoCercano consulta)
        {
            if (consulta.Default.HasValue && consulta.Default.Value)
            {
                consulta.Distancia = metrosDefault;
                consulta.DadosDeBaja = false;
                consulta.EstadosKeyValue = new List<Enums.EstadoRequerimiento>() { Enums.EstadoRequerimiento.ENPROCESO, Enums.EstadoRequerimiento.PENDIENTE, Enums.EstadoRequerimiento.INSPECCION, Enums.EstadoRequerimiento.NUEVO };
            }

            return dao.GetCantidadCercanos(consulta);
        }

        public Result<List<int>> GetIdsCercanos(Consulta_RequerimientoCercano consulta)
        {
            return dao.GetIdsCercanos(consulta);
        }

        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTablaCercanos(Consulta_RequerimientoCercano consulta)
        {
            if (consulta.Default.HasValue && consulta.Default.Value)
            {
                consulta.Distancia = metrosDefault;
                consulta.DadosDeBaja = false;
                consulta.EstadosKeyValue = new List<Enums.EstadoRequerimiento>() { Enums.EstadoRequerimiento.ENPROCESO, Enums.EstadoRequerimiento.PENDIENTE, Enums.EstadoRequerimiento.INSPECCION, Enums.EstadoRequerimiento.NUEVO };
            }

            var resultado = new Result<ResultadoTabla<ResultadoTabla_Requerimiento>>();

            var resultadoIds = dao.GetIdsCercanos(consulta);
            if (!resultadoIds.Ok)
            {
                resultado.Copy(resultadoIds.Errores);
                return resultado;
            }

            return GetResultadoTablaByIds(resultadoIds.Return);
        }


        //BUsqueda Global

        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTablaBusquedaGlobal(string input)
        {
            var resultado = new Result<ResultadoTabla<ResultadoTabla_Requerimiento>>();

            var resultadoIds = dao.GetIdsBusquedaGlobal(input);
            if (!resultadoIds.Ok)
            {
                resultado.Copy(resultadoIds.Errores);
                return resultado;
            }

            return GetResultadoTablaByIds(resultadoIds.Return);
        }

        #endregion

        int LIMITE_CANTIDAD_TABLA = 5000;
        public Result<ResultadoTabla_Requerimiento> GetResultadoTablaById(int id)
        {
            var resultado = new Result<ResultadoTabla_Requerimiento>();

            var resultadoConsulta = GetResultadoTablaByIds(new List<int>() { id });
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return == null || resultadoConsulta.Return.Data.Count == 0)
            {
                resultado.Return = null;
                return resultado;
            }

            resultado.Return = resultadoConsulta.Return.Data[0];
            return resultado;
        }

        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTablaByIds(List<int> ids)
        {
            var resultado = dao.GetResultadoTablaByIds(LIMITE_CANTIDAD_TABLA, ids);

            if (!resultado.Ok)
            {
                return resultado;
            }

            if (getUsuarioLogueado() != null)
            {
                var resultadoFav = RequerimientoFavoritoPorUsuarioDAO.Instance.GetResultadoByFilters(new Consulta_RequerimientoFavoritoPorUsuario()
                {
                    IdUser = getUsuarioLogueado().Usuario.Id,
                    DadosDeBaja = false
                });

                if (!resultadoFav.Ok)
                {
                    return resultado;
                }

                if (resultado.Return != null)
                {
                    foreach (var rq in resultado.Return.Data)
                    {
                        rq.Favorito = resultadoFav.Return.Where(x => x.RequerimientoId == rq.Id).Any();
                    }

                }
            }

            return resultado;
        }

        public Result<ResultadoTabla<ResultadoTabla_RequerimientoExportar>> GetResultadoTablaByIdsExportar(List<int> ids)
        {
            var resultado = dao.GetResultadoTablaByIdsExportar(ids);

            if (!resultado.Ok)
            {
                return resultado;
            }
            return resultado;
        }

        public Result<ResultadoTabla<ResultadoWSExterno_Requerimiento>> GetResultadoExternoByIds(List<int> ids)
        {
            var resultado = dao.GetResultadoExternoByIds(LIMITE_CANTIDAD_TABLA, ids);

            if (!resultado.Ok)
            {
                resultado.AddErrorPublico("Error extrayendo los datos");
                return resultado;
            }
            return resultado;
        }

        public Result<Resultado_Requerimiento> GetResultadoById(int id)
        {
            var resultado = new Result<Resultado_Requerimiento>();

            var resultadoConsulta = GetResultadoByIds(new List<int>() { id });
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return == null || resultadoConsulta.Return.Count == 0)
            {
                resultado.Return = null;
                return resultado;
            }

            resultado.Return = resultadoConsulta.Return[0];
            return resultado;
        }

        public Result<List<Resultado_Requerimiento>> GetResultadoByIds(List<int> ids)
        {

            var resultado = dao.GetResultadoByIds(ids);

            if (!resultado.Ok)
            {
                return resultado;
            }

            if (getUsuarioLogueado() != null)
            {
                var resultadoFav = RequerimientoFavoritoPorUsuarioDAO.Instance.GetResultadoByFilters(new Consulta_RequerimientoFavoritoPorUsuario()
                {
                    IdUser = getUsuarioLogueado().Usuario.Id,
                    DadosDeBaja = false
                });

                if (!resultadoFav.Ok)
                {
                    return resultado;
                }

                foreach (var rq in resultado.Return)
                {
                    rq.Favorito = resultadoFav.Return.Where(x => x.RequerimientoId == rq.Id).Any();
                }
            }

            return resultado;
        }

        public Result<string> GetMailReferente(int idReclamo)
        {
            return dao.GetMailReferente(idReclamo);
        }

        public Result<List<int>> GetIdsByNumero(string numero, int? año)
        {
            return dao.GetIdsByNumero(numero, año);
        }

        /* Validaciones */
        public override Result<Requerimiento> ValidateDatosNecesarios(Requerimiento entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Tipo
            if (entity.Tipo == null)
            {
                result.AddErrorPublico("Debe seleccionar el tipo de requerimiento");
                return result;
            }

            //Motivo
            if (entity.Motivo == null)
            {
                result.AddErrorPublico("Debe seleccionar el motivo");
                return result;
            }

            //Numero
            if (entity.Numero == null)
            {
                result.AddErrorPublico("El requerimiento a registrar no tiene numero");
                return result;
            }

            //Año
            if (entity.Año == null)
            {
                result.AddErrorPublico("El requerimiento a registrar no año");
                return result;
            }

            //Tipo Dispositivo
            if (entity.TipoDispositivo == null)
            {
                result.AddErrorPublico("Falta el tipo de dispositivo");
                return result;
            }

            //Prioridad
            if (entity.Prioridad == null)
            {
                result.AddErrorPublico("La prioridad es un dato necesario");
                return result;
            }

            return result;
        }


        #region Mapa

        public Result<List<Resultado_MarcadorGoogleMaps>> GetMarcadoresGoogleMaps(List<int> ids)
        {
            return dao.GetMarcadoresGoogleMaps(ids);
        }

        public Result<List<Resultado_MarcadorGoogleMaps>> GetMarcadoresLatitudLongitud(List<int> ids)
        {
            return dao.GetMarcadoresLatitudLongitud(ids);
        }

        #endregion


        /* Estados */
        private Result<EstadoRequerimientoHistorial> CrearEstado(Requerimiento rq, Enums.EstadoRequerimiento e, string observaciones)
        {
            var result = new Result<EstadoRequerimientoHistorial>();
            try
            {
                var estadoRequerimientoRules = new EstadoRequerimientoRules(getUsuarioLogueado());
                var resultEstado = estadoRequerimientoRules.GetByKeyValue(e);
                if (!resultEstado.Ok)
                {
                    result.AddErrorInterno(resultEstado.Errores.ErroresInternos);
                    result.AddErrorPublico(resultEstado.Errores.ErroresPublicos);
                    return result;
                }

                var estado = resultEstado.Return;

                var estadoRQ = new EstadoRequerimientoHistorial();
                estadoRQ.Fecha = DateTime.Now;
                estadoRQ.FechaAlta = DateTime.Now;
                estadoRQ.Usuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id).Return;
                estadoRQ.Estado = estado;
                estadoRQ.Observaciones = observaciones;
                estadoRQ.Requerimiento = rq;

                result.Return = estadoRQ;
            }
            catch (Exception ex)
            {
                MiLog.Info(ex.Message);
                if (ex.InnerException != null)
                {
                    MiLog.Info(ex.InnerException.ToString());
                }
                result.AddErrorInterno(ex);
            }
            return result;
        }

        public Result<Requerimiento> ProcesarCambioEstado(Requerimiento rq, Enums.EstadoRequerimiento estado, string observaciones)
        {
            return ProcesarCambioEstado(rq, estado, observaciones, true);
        }

        public Result<Requerimiento> ProcesarCambioEstado(Requerimiento rq, Enums.EstadoRequerimiento estado, string observaciones, bool guardarCambios)
        {
            var result = new Result<Requerimiento>();

            try
            {
                var estadoRq = CrearEstado(rq, estado, observaciones);

                //Estados para enviar e-mail
                List<Enums.EstadoRequerimiento> keyValuesEstados = new List<Enums.EstadoRequerimiento>();
                keyValuesEstados.Add(Enums.EstadoRequerimiento.ENPROCESO);
                keyValuesEstados.Add(Enums.EstadoRequerimiento.COMPLETADO);
                keyValuesEstados.Add(Enums.EstadoRequerimiento.CANCELADO);
                keyValuesEstados.Add(Enums.EstadoRequerimiento.INSPECCION);
                keyValuesEstados.Add(Enums.EstadoRequerimiento.PENDIENTE);


                if (rq.Estados == null)
                {
                    rq.Estados = new List<EstadoRequerimientoHistorial>();
                }


                if (rq.GetUltimoEstado().Estado.KeyValue != estado)
                {
                    if (rq.Estados != null && rq.Estados.Count != 0)
                    {
                        foreach (var e in rq.Estados)
                        {
                            e.Ultimo = false;
                        }
                    }

                    estadoRq.Return.Ultimo = true;
                    rq.Estados.Add(estadoRq.Return);

                    if (guardarCambios)
                    {
                        var resultUpdate = ValidateUpdate(rq);
                        if (!resultUpdate.Ok)
                        {
                            result.Copy(resultUpdate);
                            return result;
                        }

                        resultUpdate = dao.Update(rq);
                        if (!resultUpdate.Ok)
                        {
                            result.Copy(resultUpdate);
                            return result;
                        }
                    }
                }


                result.Return = rq;

                if (keyValuesEstados.Contains(estado))
                {
                    new RequerimientoMailRules(getUsuarioLogueado()).EnviarCambioEstado(rq.Id);
                }
            }
            catch (Exception e)
            {
                MiLog.Info(e.Message);
                if (e.InnerException != null)
                {
                    MiLog.Info(e.InnerException.ToString());
                }
                result.AddErrorInterno(e);
            }


            return result;
        }



        /* Mail */

        public Result<string> GetEmail(int idRequerimiento)
        {
            var result = new Result<string>();

            //Busco el reclamo
            var resultConsulta = GetById(idRequerimiento);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            if (resultConsulta.Return == null)
            {
                result.AddErrorPublico("El requerimiento no existe");
                return result;
            }

            var rq = resultConsulta.Return;
            string mail = null;
            //if (rq.UsuarioReferente != null)
            //{
            //    mail = rq.UsuarioReferente.Email;
            //}
            //else
            //{
            //    if (rq.PersonaFisica != null)
            //    {
            //        mail = rq.PersonaFisica.Mail;
            //    }
            //}

            result.Return = mail;
            return result;
        }

        public Result<string> EnviarMailContacto(string nombreCompleto, string rol, List<string> areas, string asuntoUsuario, string descripcion, string mail, string mailContacto, string telefonoContacto)
        {
            var result = new Result<string>();


            try
            {
                //inicializo mi mensaje con todos los valores
                var emisorMail = "cba147@cordoba.gob.ar";
                var emisorNombre = "Municipalidad de Cordoba";
                var emisor = new MailAddress(emisorMail, emisorNombre);
                var receptorMail = "soporte-CBA147@cordoba.gov.ar";
                //Para testear
                //var receptorMail = "mbagnus@cordoba.gov.ar";
                var receptorNombre = nombreCompleto;

                var receptor = new MailAddress(receptorMail, receptorNombre);
                string asunto = "Soporte CBA147";
                string areasString = string.Join(", ", areas.ToArray());
                //var cuerpo = "Usuario: " + nombreCompleto + "\nRol: " + rol + "\nAreas:" + areasString + "\nMail de contacto: " + mailContacto + "\nTeléfono de Contacto: " + telefonoContacto + "\n\nAsunto: " + asuntoUsuario + "\nDescripcion " + descripcion;

                string html = @"<div style='font-size: 12px;font-family: Verdana;'><p><span style='text-decoration: underline;'>DATOS DEL USUARIO</span></p><p><strong>Nombre:</strong> {0}</p><p><strong>Rol:</strong> {1}</p><p><strong>Areas:</strong> {2}</p><p><strong>Mail de contacto:</strong> {3}</p><p><strong>Tel&eacute;fono de Contacto:</strong> {4}</p><p>&nbsp;</p><p><span style='text-decoration: underline;'>\n\nSOLICITUD DE SOPORTE</span></p><p><strong>Asunto<em>:</em></strong> {5}</p><p><strong>Descripcion:</strong></p><p style='text-align: justify;'>{6}</p></div>";
                html = string.Format(html, nombreCompleto, rol, areasString, mailContacto, telefonoContacto, asuntoUsuario, descripcion);

                //Mando el mail
                MailMessage message = new MailMessage(emisor, receptor);
                message.Subject = asunto;


                message.IsBodyHtml = true;


                message.Body = html;


                SmtpClient client = new SmtpClient();
                client.Credentials = CredentialCache.DefaultNetworkCredentials;
                client.Send(message);
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.Message);
                if (e.InnerException != null)
                {
                    result.AddErrorInterno(e.InnerException.Message);
                }
            }

            return result;
        }

        public Result<List<Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorArea>> GetCantidadRequerimientosParaOrdenDeTrabajoPorArea()
        {
            var resultado = new Result<List<Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorArea>>();
            resultado.Return = new List<Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorArea>();

            if (getUsuarioLogueado() == null)
            {
                resultado.AddErrorPublico("Debe iniciar sesion para realizar esta operacion");
                return resultado;
            }

            if (getUsuarioLogueado().Areas == null || getUsuarioLogueado().Areas.Count == 0)
            {
                resultado.AddErrorPublico("El usuario logeado no posee areas");
                return resultado;
            }

            var resultadoConsulta = GetIdsAreaParaOrdenTrabajo(new Consulta_Requerimiento_Bandeja()
            {
                DadosDeBaja = false
            });

            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var grupos = resultadoConsulta.Return.GroupBy(x => x).ToList();
            foreach (var g in grupos)
            {
                //g.Count();
                resultado.Return.Add(new Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorArea()
                {
                    IdArea = g.Key,
                    Cantidad = g.Count()
                });

            }

            resultado.Return = resultado.Return.OrderByDescending(x => x.Cantidad).ToList();

            return resultado;
        }

        public Result<List<Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorAreaYTipo>> GetCantidadRequerimientosParaOrdenDeTrabajoPorAreaYTipo(int idArea)
        {
            var resultado = new Result<List<Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorAreaYTipo>>();
            resultado.Return = new List<Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorAreaYTipo>();

            if (getUsuarioLogueado() == null)
            {
                resultado.AddErrorPublico("Debe iniciar sesion para realizar esta operacion");
                return resultado;
            }

            if (getUsuarioLogueado().Areas == null || getUsuarioLogueado().Areas.Count == 0)
            {
                resultado.AddErrorPublico("El usuario logeado no posee areas");
                return resultado;
            }

            var resultadoConsulta = GetIdsTipoParaOrdenTrabajo(new Consulta_Requerimiento_Bandeja()
            {
                DadosDeBaja = false,
                IdArea = idArea
            });

            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var grupos = resultadoConsulta.Return.GroupBy(x => x).ToList();
            foreach (var g in grupos)
            {
                //g.Count();
                resultado.Return.Add(new Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorAreaYTipo()
                {
                    IdTipo = g.Key,
                    Cantidad = g.Count()
                });

            }

            resultado.Return = resultado.Return.OrderByDescending(x => x.Cantidad).ToList();

            return resultado;
        }
        public Result<List<Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorArea>> GetCantidadRequerimientosParaOrdenDeInspeccionPorArea()
        {
            var resultado = new Result<List<Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorArea>>();
            resultado.Return = new List<Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorArea>();

            if (getUsuarioLogueado() == null)
            {
                resultado.AddErrorPublico("Debe iniciar sesion para realizar esta operacion");
                return resultado;
            }

            if (getUsuarioLogueado().Areas == null || getUsuarioLogueado().Areas.Count == 0)
            {
                resultado.AddErrorPublico("El usuario logeado no posee areas");
                return resultado;
            }

            var consulta = new Consulta_Requerimiento_Bandeja()
            {
                DadosDeBaja = false
            };
            consulta.OrdenInspeccion = true;
            var resultadoConsulta = GetIdsAreaParaOrdenTrabajo(consulta);

            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var grupos = resultadoConsulta.Return.GroupBy(x => x).ToList();
            foreach (var g in grupos)
            {
                //g.Count();
                resultado.Return.Add(new Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorArea()
                {
                    IdArea = g.Key,
                    Cantidad = g.Count()
                });

            }

            resultado.Return = resultado.Return.OrderByDescending(x => x.Cantidad).ToList();

            return resultado;
        }


        /* Archivos */

        public Result<List<Resultado_ArchivoPorRequerimiento_Imagen>> GetImagenes(string server, int id)
        {
            return new ArchivoPorRequerimientoRules(getUsuarioLogueado()).GetResultadoImagenesByFilters(server, new Consulta_ArchivoPorRequerimiento()
                {
                    IdRequerimiento = id,
                    DadosDeBaja = false,
                    Tipo = Enums.TipoArchivo.IMAGEN
                });
        }

        public Result<List<Resultado_ArchivoPorRequerimiento_Documento>> GetDocumentos(string server, int id)
        {
            return new ArchivoPorRequerimientoRules(getUsuarioLogueado()).GetResultadoDocumentosByFilters(server, new Consulta_ArchivoPorRequerimiento()
            {
                IdRequerimiento = id,
                DadosDeBaja = false,
                Tipo = Enums.TipoArchivo.DOCUMENTO
            });
        }

        public Result<bool> AgregarArchivo(int id, Comando_Archivo archivo)
        {
            var resultado = new Result<bool>();

            try
            {
                //Permiso
                var resultadoPermiso = ValidarPemiso(id, Enums.PermisoEstadoRequerimiento.EditarDocumentos);
                if (!resultadoPermiso.Ok)
                {
                    resultado.Copy(resultadoPermiso.Errores);
                    return resultado;
                }

                //Busco el RQ
                var resultadoConsulta = GetByIdObligatorio(id);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Copy(resultadoConsulta.Errores);
                    return resultado;
                }

                archivo.IdUsuarioCerrojoReferente = getUsuarioLogueado().Usuario.Id;

                //Inserto el archivo
                var archivoRules = new ArchivoPorRequerimientoRules(getUsuarioLogueado());
                var resultadoInsertar = archivoRules.Insertar(archivo);
                if (!resultadoInsertar.Ok)
                {
                    resultado.Copy(resultadoInsertar.Errores);
                    return resultado;
                }

                //Lo asocio al requerimiento
                var consultaArchivo = archivoRules.GetByIdObligatorio(resultadoInsertar.Return);
                if (!consultaArchivo.Ok)
                {
                    resultado.Copy(consultaArchivo.Errores);
                    return resultado;
                }

                consultaArchivo.Return.Requerimiento = resultadoConsulta.Return;
                var resultadoUpdate = archivoRules.Update(consultaArchivo.Return);
                if (!resultadoUpdate.Ok)
                {
                    resultado.Copy(resultadoUpdate.Errores);
                    return resultado;
                }

                resultado.Return = true;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<bool> QuitarArchivo(int idRequerimiento, int idArchivo)
        {
            var resultado = new Result<bool>();

            try
            {
                //Permiso
                var resultadoPermiso = ValidarPemiso(idRequerimiento, Enums.PermisoEstadoRequerimiento.EditarDocumentos);
                if (!resultadoPermiso.Ok)
                {
                    resultado.Copy(resultadoPermiso.Errores);
                    return resultado;
                }

                //Busco el RQ
                var resultadoConsulta = GetByIdObligatorio(idRequerimiento);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Copy(resultadoConsulta.Errores);
                    return resultado;
                }

                //Busco el archivo
                var archivoRules = new ArchivoPorRequerimientoRules(getUsuarioLogueado());
                var resultadoConsultaArchivo = archivoRules.GetByIdObligatorio(idArchivo);
                if (!resultadoConsultaArchivo.Ok)
                {
                    resultado.Copy(resultadoConsultaArchivo.Errores);
                    return resultado;
                }

                //Valido que sea un archivo del requerimiento
                if (resultadoConsultaArchivo.Return.Requerimiento.Id != idRequerimiento)
                {
                    resultado.AddErrorPublico("El requerimiento no posee el archivo indicado");
                    return resultado;
                }

                //Le doy de baja al archivo
                resultadoConsultaArchivo.Return.FechaBaja = DateTime.Now;
                var resultadoUpdate = archivoRules.Update(resultadoConsultaArchivo.Return);
                if (!resultadoUpdate.Ok)
                {
                    resultado.Copy(resultadoUpdate.Errores);
                    return resultado;
                }

                resultado.Return = true;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }


        /* Favorito */

        public Result<bool> ToggleFavorito(int id)
        {
            var resultado = new Result<bool>();

            var permiso = Enums.PermisoEstadoRequerimiento.EditarFavorito;
            var resultadoPermiso = ValidarPemiso(id, permiso);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco
            var consultaRequerimiento = GetByIdObligatorio(id);
            if (!consultaRequerimiento.Ok)
            {
                resultado.Copy(consultaRequerimiento.Errores);
                return resultado;
            }

            //Favorito
            var resultadoFavorito = new RequerimientoFavoritoPorUsuarioRules(getUsuarioLogueado()).ToggleFavorito(id);
            if (!resultadoFavorito.Ok)
            {
                resultado.Copy(resultadoFavorito.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        public Result<bool> SetFavorito(int id)
        {
            var resultado = new Result<bool>();

            var permiso = Enums.PermisoEstadoRequerimiento.EditarFavorito;
            var resultadoPermiso = ValidarPemiso(id, permiso);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco
            var consultaRequerimiento = GetByIdObligatorio(id);
            if (!consultaRequerimiento.Ok)
            {
                resultado.Copy(consultaRequerimiento.Errores);
                return resultado;
            }

            //Favorito
            var resultadoFavorito = new RequerimientoFavoritoPorUsuarioRules(getUsuarioLogueado()).MarcarFavorito(new Comando_RequerimientoFavoritoPorUsuario()
            {
                Favorito = true,
                IdRequerimiento = id,
                IdUser = getUsuarioLogueado().Usuario.Id
            });
            if (!resultadoFavorito.Ok)
            {
                resultado.Copy(resultadoFavorito.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        public Result<bool> SetNoFavorito(int id)
        {
            var resultado = new Result<bool>();

            var permiso = Enums.PermisoEstadoRequerimiento.EditarFavorito;
            var resultadoPermiso = ValidarPemiso(id, permiso);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco
            var consultaRequerimiento = GetByIdObligatorio(id);
            if (!consultaRequerimiento.Ok)
            {
                resultado.Copy(consultaRequerimiento.Errores);
                return resultado;
            }

            //Favorito
            var resultadoFavorito = new RequerimientoFavoritoPorUsuarioRules(getUsuarioLogueado()).MarcarFavorito(new Comando_RequerimientoFavoritoPorUsuario()
            {
                Favorito = false,
                IdRequerimiento = id,
                IdUser = getUsuarioLogueado().Usuario.Id
            });
            if (!resultadoFavorito.Ok)
            {
                resultado.Copy(resultadoFavorito.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }


        /* Cambio de estado */

        public Result<Resultado_Requerimiento> Cancelar(int id, string observaciones)
        {
            var resultado = new Result<Resultado_Requerimiento>();

            var permiso = Enums.PermisoEstadoRequerimiento.Cancelar;
            var resultadoPermiso = ValidarPemiso(id, permiso);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco
            var resultadoConsulta = GetByIdObligatorio(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var resultadoCambioEstado = ProcesarCambioEstado(resultadoConsulta.Return, Enums.EstadoRequerimiento.CANCELADO, observaciones, true);
            if (!resultadoCambioEstado.Ok)
            {
                resultado.Copy(resultadoCambioEstado.Errores);
                return resultado;
            }

            return GetResultadoById(resultadoCambioEstado.Return.Id);
        }

        public Result<Resultado_Requerimiento> CambiarEstado(int id, Enums.EstadoRequerimiento keyValueEstado, string observaciones)
        {
            var resultado = new Result<Resultado_Requerimiento>();

            //Valido permiso para la accion
            var permiso = Enums.PermisoEstadoRequerimiento.EditarEstado;
            var resultadoPermiso = ValidarPemiso(id, permiso);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco
            var resultadoConsulta = GetByIdObligatorio(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            //Valido que cambie a un estado valido
            var resultadoEstadosValidos = new PermisoEstadoRequerimientoRules(getUsuarioLogueado()).GetEstadosKeyValueByPermiso(permiso);
            if (!resultadoEstadosValidos.Ok)
            {
                resultado.Copy(resultadoEstadosValidos.Errores);
                return resultado;
            }

            if (!resultadoEstadosValidos.Return.Contains(resultadoConsulta.Return.GetUltimoEstado().Estado.KeyValue))
            {
                resultado.AddErrorPublico("El estado indicado no es válido");
                return resultado;
            }

            var resultadoCambioEstado = ProcesarCambioEstado(resultadoConsulta.Return, keyValueEstado, observaciones, true);
            if (!resultadoCambioEstado.Ok)
            {
                resultado.Copy(resultadoCambioEstado.Errores);
                return resultado;
            }

            return GetResultadoById(resultadoCambioEstado.Return.Id);
        }


        /* Favoritos */

        public Result<Enums.PrioridadRequerimiento> TogglePrioridad(int id)
        {
            var resultado = new Result<Enums.PrioridadRequerimiento>();

            //Busco
            var consultaRequerimiento = GetByIdObligatorio(id);
            if (!consultaRequerimiento.Ok)
            {
                resultado.Copy(consultaRequerimiento.Errores);
                return resultado;
            }
            var rq = consultaRequerimiento.Return;

            //Toggleo la prioridad
            Enums.PrioridadRequerimiento prioridad = Enums.PrioridadRequerimiento.NORMAL;
            switch (rq.Prioridad)
            {
                case Enums.PrioridadRequerimiento.ALTA:
                    {
                        prioridad = Enums.PrioridadRequerimiento.NORMAL;
                    } break;

                case Enums.PrioridadRequerimiento.MEDIA:
                    {
                        prioridad = Enums.PrioridadRequerimiento.ALTA;
                    } break;

                case Enums.PrioridadRequerimiento.NORMAL:
                    {
                        prioridad = Enums.PrioridadRequerimiento.MEDIA;
                    } break;
            }

            return SetPrioridad(id, prioridad);
        }

        public Result<Enums.PrioridadRequerimiento> SetPrioridad(int id, Enums.PrioridadRequerimiento prioridad)
        {
            var resultado = new Result<Enums.PrioridadRequerimiento>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(id, Enums.PermisoEstadoRequerimiento.EditarPrioridad);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco
            var consultaRequerimiento = GetByIdObligatorio(id);
            if (!consultaRequerimiento.Ok)
            {
                resultado.Copy(consultaRequerimiento.Errores);
                return resultado;
            }
            var rq = consultaRequerimiento.Return;
            rq.Prioridad = prioridad;

            //Actualizo
            var resultadoUpdate = base.Update(rq);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = rq.Prioridad;
            return resultado;
        }


        /* Marcado */

        public Result<bool> MarcarRequerimiento(int id)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(id, Enums.PermisoEstadoRequerimiento.EditarMarcado);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Lo busco
            var consulta = GetByIdObligatorio(id);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var rq = consulta.Return;

            //Marco
            rq.Marcado = true;
            var resultMarcar = Update(rq);
            if (!resultMarcar.Ok)
            {
                resultado.AddErrorPublico(resultMarcar.ToStringPublico());
                return resultado;
            }
            resultado.Return = rq.Marcado;
            return resultado;
        }

        public Result<bool> DesmarcarRequerimiento(int id)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(id, Enums.PermisoEstadoRequerimiento.EditarMarcado);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Lo busco
            var consulta = GetByIdObligatorio(id);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var rq = consulta.Return;

            //Marco
            rq.Marcado = false;
            var resultMarcar = Update(rq);
            if (!resultMarcar.Ok)
            {
                resultado.AddErrorPublico(resultMarcar.ToStringPublico());
                return resultado;
            }

            resultado.Return = rq.Marcado;
            return resultado;
        }

        public Result<bool> ToggleMarcado(int id)
        {
            var resultado = new Result<bool>();

            //Valido permiso
            var resultadoPermiso = ValidarPemiso(id, Enums.PermisoEstadoRequerimiento.EditarFavorito);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Lo busco
            var consulta = GetByIdObligatorio(id);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var rq = consulta.Return;

            //Toggleo el marcado
            if (rq.Marcado)
            {
                return DesmarcarRequerimiento(id);
            }
            else
            {
                return MarcarRequerimiento(id);
            }
        }


        /* Persona asociada */

        public Result<bool> AgregarReferente(int id, int idUsuario)
        {
            var resultado = new Result<bool>();

            //Valido permiso
            var resultadoPermiso = ValidarPemiso(id, Enums.PermisoEstadoRequerimiento.EditarReferente);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Lo busco
            var consulta = GetByIdObligatorio(id);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var rq = consulta.Return;
            if (rq.UsuariosReferentes.Where(x => x.UsuarioReferente.Id == idUsuario && x.FechaBaja == null).FirstOrDefault() != null)
            {
                resultado.AddErrorPublico("El usuario ya es referente del requerimiento");
                return resultado;
            }

            //Busco el referente
            var consultaReferente = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetByIdObligatorio(idUsuario);
            if (!consultaReferente.Ok)
            {
                resultado.Copy(consultaReferente.Errores);
                return resultado;
            }

            var urxrq = new UsuarioReferentePorRequerimiento();
            urxrq.UsuarioReferente = consultaReferente.Return;
            urxrq.Requerimiento = rq;

            var resultadoInsertar = new UsuarioReferentePorRequerimientoRules(getUsuarioLogueado()).Insert(urxrq);
            if (!resultadoInsertar.Ok)
            {
                resultado.Copy(resultadoInsertar.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }
        public Result<bool> QuitarReferente(int id, int idUsuario)
        {
            var resultado = new Result<bool>();

            //Valido permiso
            var resultadoPermiso = ValidarPemiso(id, Enums.PermisoEstadoRequerimiento.EditarReferente);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Lo busco
            var consulta = GetByIdObligatorio(id);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var rq = consulta.Return;
            var refAEliminar = rq.UsuariosReferentes.Where(x => x.UsuarioReferente.Id == idUsuario && x.FechaBaja == null).FirstOrDefault();

            if (refAEliminar == null)
            {
                resultado.AddErrorPublico("Error al intentar eliminar el usuario referente");
                return resultado;
            }

            var resultadoBorrar = new UsuarioReferentePorRequerimientoRules(getUsuarioLogueado()).Delete(refAEliminar);
            if (!resultadoBorrar.Ok)
            {
                resultado.Copy(resultadoBorrar.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        public Result<IList<_Resultado_VecinoVirtualUsuario>> GetUsuariosReferentesById(int id)
        {
            var resultado = new Result<IList<_Resultado_VecinoVirtualUsuario>>();
            var resultRQ = GetByIdObligatorio(id);
            if (!resultRQ.Ok)
            {
                return resultado;
            }

            resultado.Return = _Resultado_VecinoVirtualUsuario.ToList(resultRQ.Return.UsuariosReferentes.Select(x => x.UsuarioReferente).ToList());
            return resultado;
        }

        /* Referente provisorio*/

        public Result<bool> EditarReferenteProvisorio(Model.Comandos.Comando_RequerimientoIntranet.Comando_ReferenteProvisorio comando)
        {
            var resultado = new Result<bool>();

            resultado.Return = dao.Transaction(() =>
             {
                 //Valido permiso
                 var resultadoPermiso = ValidarPemiso(comando.IdRequerimiento, Enums.PermisoEstadoRequerimiento.EditarReferente);
                 if (!resultadoPermiso.Ok)
                 {
                     resultado.Copy(resultadoPermiso.Errores);
                     return false;
                 }

                 //Lo busco
                 var consulta = GetByIdObligatorio(comando.IdRequerimiento);
                 if (!consulta.Ok)
                 {
                     resultado.Copy(consulta.Errores);
                     return false;
                 }

                 var rq = consulta.Return;
                 var referente = new ReferenteProvisorio();
                 var insert = true;
                 if (rq.ReferenteProvisorio != null)
                 {
                     referente=rq.ReferenteProvisorio ;
                     insert = false;
                 }

                 if (comando != null)
                 {

                     referente.Apellido = comando.Apellido;
                     referente.Nombre = comando.Nombre;
                     referente.DNI = comando.DNI;
                     referente.GeneroMasculino = comando.GeneroMasculino;
                     referente.Telefono = comando.Telefono;
                     referente.Observaciones = comando.Observaciones;

                     var resultReferenteProvisorio = new Result<ReferenteProvisorio>();
                     if (insert)
                     {
                         resultReferenteProvisorio = new ReferenteProvisorioRules(getUsuarioLogueado()).Insert(referente);
                     }
                     else
                     {
                         resultReferenteProvisorio = new ReferenteProvisorioRules(getUsuarioLogueado()).Update(referente);
                     }

                     if (!resultReferenteProvisorio.Ok)
                     {
                         resultado.Copy(resultReferenteProvisorio.Errores);
                         return false;
                     }

                     rq.ReferenteProvisorio = referente;
                     var resultUpdate = Update(rq);
                     if (!resultUpdate.Ok)
                     {
                         resultado.Copy(resultUpdate.Errores);
                         return false;
                     }
                 }

                 return true;
             });

            return resultado;
        }


        /* Motivo */

        public Result<bool> CambiarMotivo(int id, int idMotivo)
        {
            var resultado = new Result<bool>();

            //Valido permiso
            var resultadoPermiso = ValidarPemiso(id, Enums.PermisoEstadoRequerimiento.EditarMotivo);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Lo busco
            var consulta = GetByIdObligatorio(id);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var rq = consulta.Return;


            //Busco el motivo
            var consultaMotivo = new MotivoRules(getUsuarioLogueado()).GetByIdObligatorio(idMotivo);
            if (!consultaMotivo.Ok)
            {
                resultado.Copy(consultaMotivo.Errores);
                return resultado;
            }

            rq.Motivo = consultaMotivo.Return;

            //area responsable
            rq.AreaResponsable = null;
            if (rq.Motivo.Area.Subareas != null && rq.Motivo.Area.Subareas.Count != 0)
            {
                foreach (CerrojoArea subarea in rq.Motivo.Area.Subareas)
                {
                    if (subarea.TerritorioIncumbencia.EstaEnMiTerritorio(Double.Parse(rq.Domicilio.Latitud.Trim().Replace(',', '.'), CultureInfo.InvariantCulture), Double.Parse(rq.Domicilio.Longitud.Trim().Replace(',', '.'), CultureInfo.InvariantCulture)))
                    {
                        rq.AreaResponsable = subarea;
                        break;
                    }
                }
            }

            if (rq.AreaResponsable == null)
            {
                rq.AreaResponsable = rq.Motivo.Area;
            }

            var resultadoUpdate = Update(rq);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        public Result<bool> CambiarMotivoDesdeOT(int id, int idMotivo)
        {
            var resultado = new Result<bool>();


            //Busco el requerimiento
            var consultaRQ = GetByIdObligatorio(id);
            if (!consultaRQ.Ok)
            {
                resultado.Copy(consultaRQ.Errores);
                return resultado;
            }

            var rq = consultaRQ.Return;

            if (rq.OrdenTrabajoActiva.Requerimientos().Count() == 1)
            {
                resultado.AddErrorPublico("No se puede eliminar el único requerimiento de la orden de trabajo.");
                return resultado;
            }

            dao.Transaction(() =>
            {
                //Quito el rq
                //Le hago un update a la ot para que se le ponga fecha de modificación 
                var resultUpdateOt = new OrdenTrabajoRules(getUsuarioLogueado()).Update(rq.OrdenTrabajoActiva);
                if (!resultUpdateOt.Ok)
                {
                    resultado.AddErrorPublico("Error al actualizar la orden de trabajo");
                    return false;
                }

                //Cambio el estado
                var resultCambiarEstadoRQ = ProcesarCambioEstado(rq, Enums.EstadoRequerimiento.PENDIENTE, "Por salir de Orden de trabajo N°" + rq.OrdenTrabajoActiva.Numero + "/" + rq.OrdenTrabajoActiva.Año, false);
                if (!resultCambiarEstadoRQ.Ok)
                {
                    resultado.Copy(resultCambiarEstadoRQ.Errores);
                    return false;
                }

                var rqXOT = rq.OrdenTrabajoActiva.GetRequerimientoPorOrdenTrabajo(rq.Id);
                if (rqXOT == null)
                {
                    resultado.AddErrorPublico("Error al actualizar la orden de trabajo.");
                    return false;
                }

                var resultDelete = new RequerimientoPorOrdenTrabajoRules(getUsuarioLogueado()).Delete(rqXOT);
                if (rqXOT == null)
                {
                    resultado.AddErrorPublico("Error al eliminar el requerimiento de la orden de trabajo.");
                    return false;
                }

                //Quito la OT activa
                rq.OrdenTrabajoActiva = null;

                var resultUpdateRQ = Update(rq);
                if (!resultUpdateRQ.Ok)
                {
                    resultado.Copy(resultUpdateRQ.Errores);
                    return false;
                }

                //Seteo el nuevo motivo
                //Busco el motivo
                var consultaMotivo = new MotivoRules(getUsuarioLogueado()).GetByIdObligatorio(idMotivo);
                if (!consultaMotivo.Ok)
                {
                    resultado.Copy(consultaMotivo.Errores);
                    return false;
                }

                rq.Motivo = consultaMotivo.Return;

                //area responsable
                rq.AreaResponsable = null;
                if (rq.Motivo.Area.Subareas != null && rq.Motivo.Area.Subareas.Count != 0)
                {
                    foreach (CerrojoArea subarea in rq.Motivo.Area.Subareas)
                    {
                        if (subarea.TerritorioIncumbencia.EstaEnMiTerritorio(Double.Parse(rq.Domicilio.Latitud.Trim().Replace(',', '.'), CultureInfo.InvariantCulture), Double.Parse(rq.Domicilio.Longitud.Trim().Replace(',', '.'), CultureInfo.InvariantCulture)))
                        {
                            rq.AreaResponsable = subarea;
                            break;
                        }
                    }
                }

                if (rq.AreaResponsable == null)
                {
                    rq.AreaResponsable = rq.Motivo.Area;
                }

                var resultadoUpdate = Update(rq);
                if (!resultadoUpdate.Ok)
                {
                    resultado.Copy(resultadoUpdate.Errores);
                    return false;
                }

                resultado.Return = true;
                return true;
            });

            return resultado;
        }

        /* Domicilio */

        public Result<bool> CambiarDomicilio(int id, Comando_Domicilio comando)
        {
            var resultado = new Result<bool>();

            var resultadoTransaccion = dao.Transaction(() =>
            {
                try
                {
                    //Valido permiso
                    var resultadoPermiso = ValidarPemiso(id, Enums.PermisoEstadoRequerimiento.EditarUbicacion);
                    if (!resultadoPermiso.Ok)
                    {
                        resultado.Copy(resultadoPermiso.Errores);
                        return false;
                    }

                    //Lo busco
                    var consulta = GetByIdObligatorio(id);
                    if (!consulta.Ok)
                    {
                        resultado.Copy(consulta.Errores);
                        return false;
                    }

                    var rq = consulta.Return;


                    //Genero el domicilio
                    var resultadoDomicilio = new DomicilioRules(getUsuarioLogueado()).Buscar(comando.Latitud, comando.Longitud);
                    if (!resultadoDomicilio.Ok)
                    {
                        resultado.Copy(resultadoDomicilio.Errores);
                        return false;
                    }

                    var domicilio = resultadoDomicilio.Return;
                    if (domicilio == null)
                    {
                        resultado.AddErrorPublico("Error procesando la solicitud");
                        return false;
                    }

                    domicilio.Direccion = comando.Direccion;
                    domicilio.Observaciones = comando.Observaciones;
                    var resultadoInsertarDomicilio = new DomicilioRules(getUsuarioLogueado()).Insert(domicilio);
                    if (!resultadoInsertarDomicilio.Ok)
                    {
                        resultado.Copy(resultadoInsertarDomicilio.Errores);
                        return false;
                    }
                    rq.Domicilio = resultadoInsertarDomicilio.Return;

                    //Subarea responsable
                    rq.AreaResponsable = null;
                    if (rq.Motivo.Area.Subareas != null && rq.Motivo.Area.Subareas.Count != 0)
                    {
                        foreach (CerrojoArea subarea in rq.Motivo.Area.Subareas)
                        {
                            if (subarea.TerritorioIncumbencia.EstaEnMiTerritorio(comando.Latitud, comando.Longitud))
                            {
                                rq.AreaResponsable = subarea;
                                break;
                            }
                        }
                    }

                    if (rq.AreaResponsable == null)
                    {
                        rq.AreaResponsable = rq.Motivo.Area;
                    }

                    //Actualizo
                    var resultadoUpdate = Update(rq);
                    if (!resultadoUpdate.Ok)
                    {
                        resultado.Copy(resultadoUpdate.Errores);
                        return false;
                    }

                    resultado.Return = true;
                    return true;

                }
                catch (Exception e)
                {
                    resultado.AddErrorInterno(e);
                    return false;
                }
            });


            if (!resultadoTransaccion)
            {
                resultado.AddErrorInterno("Error en la transaccion");
            }

            return resultado;
        }

        /* Comentarios */
        public Result<bool> AgregarComentario(Comando_RequerimientoComentario comando)
        {
            var result = new Result<bool>();

            RequerimientoDAO.Instance.Transaction(() =>
            {
                //Busco el Requerimiento
                var resultQueryRequerimiento = GetByIdObligatorio(comando.IdRequerimiento);
                if (!resultQueryRequerimiento.Ok)
                {
                    result.Copy(resultQueryRequerimiento.Errores);
                    return false;
                }

                var requerimiento = resultQueryRequerimiento.Return;


                var ordenTrabajoRules = new OrdenTrabajoRules(getUsuarioLogueado());

                //Busco la Orden de trabajo
                OrdenTrabajo orden = null;
                if (comando.IdOrdenTrabajo.HasValue)
                {
                    var resultQueryOrden = ordenTrabajoRules.GetByIdObligatorio(comando.IdOrdenTrabajo.Value);
                    if (!resultQueryOrden.Ok)
                    {
                        result.Copy(resultQueryOrden.Errores);
                        return false;
                    }

                    orden = resultQueryOrden.Return;
                }

                var notaPorRequerimientoRules = new NotaPorRequerimientoRules(getUsuarioLogueado());

                NotaPorRequerimiento notaRequerimiento = new NotaPorRequerimiento();
                notaRequerimiento.Observaciones = comando.Comentario;
                notaRequerimiento.Requerimiento = requerimiento;
                notaRequerimiento.OrdenTrabajo = orden;

                //Inserto la nota
                var resultInsert = notaPorRequerimientoRules.Insert(notaRequerimiento);
                if (!resultInsert.Ok)
                {
                    result.Copy(resultInsert.Errores);
                    return false;
                }

                //Cambio al fecha del requerimiento
                var resultUpdateRequerimiento = ValidateUpdate(requerimiento);
                if (!resultUpdateRequerimiento.Ok)
                {
                    result.Copy(resultUpdateRequerimiento.Errores);
                    return false;
                }

                resultUpdateRequerimiento = RequerimientoDAO.Instance.Update(requerimiento);
                if (!resultUpdateRequerimiento.Ok)
                {
                    result.Copy(resultUpdateRequerimiento.Errores);
                    return false;
                }

                result.Return = true;
                return true;
            });

            return result;
        }


        /* Tareas */
        //public Result<bool> EditarTareas(Comando_RequerimientoTareas comando)
        //{
        //    var resultado = new Result<bool>();

        //    //Validar permiso
        //    var resultadoPermiso = ValidarPemiso(comando.IdRequerimiento, Enums.PermisoEstadoRequerimiento.AgregarTareas);
        //    if (!resultadoPermiso.Ok)
        //    {
        //        resultado.Copy(resultadoPermiso.Errores);
        //        return resultado;
        //    }

        //    //Busco el requerimiento
        //    var consulta = GetByIdObligatorio(comando.IdRequerimiento);
        //    if (!consulta.Ok)
        //    {
        //        resultado.Copy(consulta.Errores);
        //        return resultado;
        //    }

        //    var rq = consulta.Return;

        //    var resultadoTransaccion = dao.Transaction(() =>
        //    {
        //        //Le hago un update al rq para que se le ponga fecha de modificación 
        //        var resultUpdate = base.Update(rq);
        //        if (!resultUpdate.Ok)
        //        {
        //            resultado.AddErrorPublico("Error al actualizar el requerimiento");
        //            return false;
        //        }

        //        var tareasXRq = new TareaPorAreaPorRequerimientoRules(getUsuarioLogueado());
        //        var tareaRules = new TareaPorAreaRules(getUsuarioLogueado());

        //        if (rq.getTareas() != null)
        //        {
        //            //borro las tareas que habia
        //            foreach (TareaPorAreaPorRequerimiento t in rq.getTareas())
        //            {
        //                var resultQuitar = tareasXRq.Delete(t);
        //                if (!resultQuitar.Ok)
        //                {
        //                    resultado.AddErrorPublico("Error al quitar uno de las tareas");
        //                    return false;
        //                }
        //            }
        //        }

        //        var idsDadosDeBaja= rq.getTareas().Where(x => x.Tarea.FechaBaja != null).Select(z=>z.Id).ToList();

        //        if (comando.IdsTareas != null)
        //        {
        //            var a= comando.IdsTareas.Where(x => !idsDadosDeBaja.Contains(x)).ToList();
        //            foreach (int idT in a)
        //            {
        //                var resultConsulta = tareaRules.GetByIdObligatorio(idT);
        //                if (!resultConsulta.Ok)
        //                {
        //                    resultado.AddErrorPublico("Error al agregar una de las tareas");
        //                    return false;
        //                }

        //                if (resultConsulta.Return.Area.Id != rq.AreaResponsable.Id)
        //                {
        //                    resultado.AddErrorPublico("Una de las tareas no pertenece al área del requerimiento");
        //                    return false;
        //                }

        //                var txaxr = new TareaPorAreaPorRequerimiento();
        //                txaxr.Tarea = resultConsulta.Return;
        //                txaxr.Requerimiento = rq;

        //                var resultInsert = tareasXRq.Insert(txaxr);
        //                if (!resultInsert.Ok)
        //                {
        //                    resultado.AddErrorPublico("Error al agregar una de las tareas");
        //                    return false;
        //                }
        //            }
        //        }


        //        return true;
        //    });

        //    if (!resultadoTransaccion)
        //    {
        //        resultado.AddErrorPublico("Error procesando la solicitud.");
        //    }

        //    resultado.Return = true;
        //    return resultado;
        //}


        public Result<bool> EditarTareas(Comando_RequerimientoTareas comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.IdRequerimiento, Enums.PermisoEstadoRequerimiento.AgregarTareas);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco el requerimiento
            var consulta = GetByIdObligatorio(comando.IdRequerimiento);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var rq = consulta.Return;

            var resultadoTransaccion = dao.Transaction(() =>
            {
                //Le hago un update al rq para que se le ponga fecha de modificación 
                var resultUpdate = base.Update(rq);
                if (!resultUpdate.Ok)
                {
                    resultado.AddErrorPublico("Error al actualizar el requerimiento");
                    return false;
                }

                var tareasXRQRules = new TareaPorAreaPorRequerimientoRules(getUsuarioLogueado());
                var tareaRules = new TareaPorAreaRules(getUsuarioLogueado());

                var idsTareasAgregar = comando.IdsTareas.Where(x => rq.getTareas().All(y => y.Tarea.Id != x)).ToList();
                if (idsTareasAgregar.Count > 0)
                {
                    foreach (int m in idsTareasAgregar)
                    {
                        var resultConsulta = tareaRules.GetByIdObligatorio(m);
                        if (!resultConsulta.Ok)
                        {
                            resultado.AddErrorPublico("Error al agregar una de las tareas");
                            return false;
                        }

                        if (resultConsulta.Return.Area.Id != rq.AreaResponsable.Id)
                        {
                            resultado.AddErrorPublico("Una de las tareas no pertenece al área de la orden de trabajo");
                            return false;
                        }

                        var mxot = new TareaPorAreaPorRequerimiento();
                        mxot.Tarea = resultConsulta.Return;
                        mxot.Requerimiento = rq;

                        var resultInsert = tareasXRQRules.Insert(mxot);
                        if (!resultInsert.Ok)
                        {
                            resultado.AddErrorPublico("Error al agregar una de las tareas");
                            return false;
                        }
                    }
                }

                var tareasQuitar = rq.getTareas().Where(x => comando.IdsTareas.All(y => y != x.Tarea.Id)).ToList();
                if (tareasQuitar.Count > 0)
                {
                    foreach (TareaPorAreaPorRequerimiento m in tareasQuitar)
                    {
                        var resultQuitar = tareasXRQRules.Delete(m);
                        if (!resultQuitar.Ok)
                        {
                            resultado.AddErrorPublico("Error al quitar una de las tareas");
                            return false;
                        }
                    }
                }

                return true;
            });

            if (!resultadoTransaccion)
            {
                resultado.AddErrorPublico("Error procesando la solicitud.");
            }

            resultado.Return = true;
            return resultado;
        }

        public Result<bool> EditarCamposDinamicos(Comando_RequerimientoEditarCamposDinamicos comando)
        {
            var resultado = new Result<bool>();

            var resultadoPermiso = ValidarPemiso(comando.IdRequerimiento, Enums.PermisoEstadoRequerimiento.EditarCamposDinamicos);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco el requerimiento
            var consulta = GetByIdObligatorio(comando.IdRequerimiento);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var rq = consulta.Return;

            foreach (CampoPorMotivo c in rq.Motivo.Campos)
            {
                if (c.Obligatorio && comando.CamposDinamicos.Where(x => x.Id == c.Id) == null)
                {
                    resultado.AddErrorPublico("El campo " + c.Nombre + "es obligatorio");
                    return resultado;
                }
            }

            var resultadoTransaccion = dao.Transaction(() =>
            {
                //Le hago un update al rq para que se le ponga fecha de modificación 
                var resultUpdate = base.Update(rq);
                if (!resultUpdate.Ok)
                {
                    resultado.AddErrorPublico("Error al actualizar el requerimiento");
                    return false;
                }

                //Campos dinámicos
                foreach (Model.Comandos.Comando_RequerimientoIntranet.Comando_CampoDinamico comandoCampo in comando.CamposDinamicos)
                {
                    var insertar = false;
                    var campoXRQ = rq.CamposDinamicos.Where(y => y.CampoPorMotivo.Id == comandoCampo.Id).FirstOrDefault();
                    var resultUpdateCampo = new Result<CampoPorMotivoPorRequerimiento>();

                    if (campoXRQ != null && String.IsNullOrWhiteSpace(comandoCampo.Valor))
                    {
                        resultUpdateCampo = new CampoPorMotivoPorRequerimientoRules(getUsuarioLogueado()).Delete(campoXRQ);

                        if (!resultUpdateCampo.Ok)
                        {
                            resultado.Copy(resultUpdateCampo.Errores);
                            return false;
                        }

                        continue;
                    }

                    if (campoXRQ == null && String.IsNullOrWhiteSpace(comandoCampo.Valor))
                    {
                        continue;
                    }

                    if (campoXRQ == null)
                    {
                        campoXRQ = new CampoPorMotivoPorRequerimiento();
                        var resultConsultaCampo = new CampoPorMotivoRules(getUsuarioLogueado()).GetById(comandoCampo.Id);
                        if (!resultConsultaCampo.Ok)
                        {
                            resultado.Copy(resultConsultaCampo.Errores);
                            return false;
                        }

                        insertar = true;
                        campoXRQ.CampoPorMotivo = resultConsultaCampo.Return;
                    }

                    campoXRQ.Requerimiento = rq;
                    campoXRQ.Valor = comandoCampo.Valor;

                    var resultadoValidar = ValidarCampoDinamico(campoXRQ);
                    if (!resultadoValidar.Ok)
                    {
                        resultado.Copy(resultadoValidar.Errores);
                        return false;
                    }

                    if (insertar)
                    {
                        resultUpdateCampo = new CampoPorMotivoPorRequerimientoRules(getUsuarioLogueado()).Insert(campoXRQ);

                    }
                    else
                    {
                        resultUpdateCampo = new CampoPorMotivoPorRequerimientoRules(getUsuarioLogueado()).Update(campoXRQ);
                    }

                    if (!resultUpdateCampo.Ok)
                    {
                        resultado.Copy(resultUpdateCampo.Errores);
                        return false;
                    }
                }

                return true;
            });


            resultado.Return = true;
            return resultado;
        }

        public Result<bool> ValidarCampoDinamico(CampoPorMotivoPorRequerimiento campo)
        {
            var resultado = new Result<bool>();
            if (campo.CampoPorMotivo.Motivo != campo.Requerimiento.Motivo)
            {
                resultado.AddErrorPublico("El campo " + campo.CampoPorMotivo.Nombre + " no pertenece al motivo");
                return resultado;
            }

            switch (campo.CampoPorMotivo.Tipo.KeyValue)
            {
                case Enums.TipoCampoPorMotivo.NUMERO:
                    int n;
                    if (!Int32.TryParse(campo.Valor, out n))
                    {
                        resultado.AddErrorPublico("El valor ingresado para " + campo.CampoPorMotivo.Nombre + " xxno es un número");
                        return resultado;
                    }
                    break;
                case Enums.TipoCampoPorMotivo.FECHA:
                    string[] format = new string[] { "dd/MM/yyyy" };
                    DateTime datetime;

                    if (!DateTime.TryParseExact(campo.Valor, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out datetime))
                    {
                        resultado.AddErrorPublico("El valor ingresado para " + campo.CampoPorMotivo.Nombre + " no es una fecha");
                        return resultado;
                    }
                    break;

                case Enums.TipoCampoPorMotivo.SINO:
                    if (campo.Valor != "true" && campo.Valor != "false")
                    {
                        resultado.AddErrorPublico("El valor ingresado para " + campo.CampoPorMotivo.Nombre + " es incorrecto");
                        return resultado;
                    }
                    break;
            }

            return resultado;
        }

        public Result<bool> QuitarTarea(Comando_RequerimientoTareas comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.IdRequerimiento, Enums.PermisoEstadoRequerimiento.AgregarTareas);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco el requerimiento
            var consulta = GetByIdObligatorio(comando.IdRequerimiento);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var rq = consulta.Return;

            var resultadoTransaccion = dao.Transaction(() =>
            {
                //Le hago un update al rq para que se le ponga fecha de modificación 
                var resultUpdate = base.Update(rq);
                if (!resultUpdate.Ok)
                {
                    resultado.AddErrorPublico("Error al actualizar el requerimiento");
                    return false;
                }

                var tareaABorrar = rq.getTareas().Where(x => x.Tarea.Id == comando.IdTarea).FirstOrDefault();
                if (tareaABorrar != null)
                {
                    var resultQuitar = new TareaPorAreaPorRequerimientoRules(getUsuarioLogueado()).Delete(tareaABorrar);
                    if (!resultQuitar.Ok)
                    {
                        resultado.AddErrorPublico("Error al quitar la tarea");
                        return false;
                    }
                }

                return true;
            });

            if (!resultadoTransaccion)
            {
                resultado.AddErrorPublico("Error procesando la solicitud.");
            }

            resultado.Return = true;
            return resultado;
        }
        /* Permiso */

        public Result<bool> ValidarPemiso(int id, Enums.PermisoEstadoRequerimiento permiso)
        {
            var resultado = new Result<bool>();

            //Lo busco
            var consulta = GetById(id);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var rq = consulta.Return;
            if (rq == null)
            {
                resultado.AddErrorPublico("El requerimiento no existe");
                return resultado;
            }
            if (rq.FechaBaja != null)
            {
                resultado.AddErrorPublico("El requerimiento se encuentra dado de baja");
                return resultado;
            }

            //Valido el permiso
            var keyValueEstado = rq.GetUltimoEstado().Estado.KeyValue;
            var resultadoPermiso = new PermisoEstadoRequerimientoRules(getUsuarioLogueado()).TienePermiso(keyValueEstado, permiso);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }
            if (!resultadoPermiso.Return)
            {
                resultado.AddErrorPublico("El requerimiento no se encuentra en un estado válido para realizar esta accion");
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        ///* Domicilio */

        //public bool SetearATI()
        //{
        //    var comando = new Consulta_Requerimiento();
        //    var ids = new List<int>();
        //    ids.Add(1359);
        //    var atixrqRules = new TerritorioIncumbenciaRules(getUsuarioLogueado());

        //    comando.IdsArea = ids;
        //    var desde = new DateTime(2018, 12, 1, 0, 0, 0);
        //    var hasta = new DateTime(2019, 12, 18, 0, 0, 0);
        //    comando.FechaDesde = desde;
        //    comando.FechaHasta = hasta;
        //    var result = GetByFilters(comando, null);

        //    var subareas = result.Return[0].AreaResponsable.Subareas;

        //    var B = dao.Transaction(() =>
        //    {
        //        foreach (Requerimiento rq in result.Return)
        //        {

        //            var lat = Double.Parse(rq.Domicilio.Latitud.Trim().Replace(',', '.'), CultureInfo.InvariantCulture);
        //            var lon = Double.Parse(rq.Domicilio.Longitud.Trim().Replace(',', '.'), CultureInfo.InvariantCulture);

        //            foreach (CerrojoArea a in subareas)
        //            {
        //                    var resultATIS = a.TerritorioIncumbencia.EstaEnMiTerritorio(lat, lon);
        //                    if (resultATIS)
        //                    {
        //                        rq.AreaResponsable = a;
        //                        break;
        //                    }
        //            }

        //            var resultUpdate = dao.Update(rq);
        //            if (!result.Ok)
        //            {
        //                Console.WriteLine("ERROR ID=" + rq.Id);
        //                Console.WriteLine(result.Error);
        //            }


        //            if (rq.AreaResponsable.Id == 1359)
        //            {
        //                Console.WriteLine("NO CAMBIO ID=" + rq.Id);
        //            }
        //        }

        //        return true;
        //    });

        //    return B;
        //}



    }

}
