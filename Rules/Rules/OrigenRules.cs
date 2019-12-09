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
    public class OrigenRules : BaseRules<Origen>
    {
        private readonly OrigenDAO dao;

        public OrigenRules(UsuarioLogueado data)
            : base(data)
        {
            dao = OrigenDAO.Instance;
        }

        #region Validaciones 
        
        public override Result<int> BuscarCantidadDuplicados(Origen entity)
        {
            var result = new Result<int>();

            int? id = null;
            if (entity.Id != 0)
            {
                id = entity.Id;
            }
            var resultConsulta = dao.GetCantidadDuplicados(id, entity.Nombre, entity.KeyAlias, entity.KeySecret);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta);
                return result;
            }

            result.Return = resultConsulta.Return;
            return result;
        }

        public override string MensajeDuplicado(Origen entity)
        {
            return "Ya existe un origen con el nombre: " + entity.Nombre;
        }

        public override Result<Origen> ValidateDatosNecesarios(Origen entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Nombre
            if (string.IsNullOrEmpty(entity.Nombre))
            {
                result.AddErrorPublico("Debe ingresar el nombre");
                return result;
            }

            //Key
            if (string.IsNullOrEmpty(entity.KeyAlias))
            {
                result.AddErrorPublico("Debe ingresar el key Alias");
            }

            //KeySecret
            if (string.IsNullOrEmpty(entity.KeySecret))
            {
                result.AddErrorPublico("Debe ingresar el Key Secret");
            }

            return result;
        }

        #endregion
        
        public Result<List<Resultado_Origen>> GetResultadoByFilters(Consulta_Origen consulta)
        {
            return dao.GetResultadoByFilters(consulta);
        }

        public Result<List<Origen>> GetByFilters(Consulta_Origen consulta)
        {
            return dao.GetByFilters(consulta);
        }
        
        public Result<Resultado_Origen> Insertar(Comando_Origen comando)
        {
            var resultado = new Result<Resultado_Origen>();
            try
            {
                var resultadoKey = GenerarKeyAlias();
                if (!resultadoKey.Ok)
                {
                    resultado.Errores.Copy(resultadoKey.Errores);
                    return resultado;
                }


                var resultadoKeySecret = GenerarKeySecret();
                if (!resultadoKeySecret.Ok)
                {
                    resultado.Errores.Copy(resultadoKeySecret.Errores);
                    return resultado;
                }

                Origen origen = new Origen(comando, resultadoKey.Return, resultadoKeySecret.Return);
                var resultadoInsertar = base.Insert(origen);
                if (!resultadoInsertar.Ok)
                {
                    resultado.Errores.Copy(resultadoInsertar.Errores);
                    return resultado;
                }

                resultado.Return = new Resultado_Origen(resultadoInsertar.Return);
                return resultado;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<Resultado_Origen> Editar(Comando_Origen comando)
        {
            var resultado = new Result<Resultado_Origen>();
            try
            {
                if (!comando.Id.HasValue)
                {
                    resultado.AddErrorPublico("Debe indicar el origen a editar");
                    return resultado;
                }

                var resultadoConsulta = GetById(comando.Id.Value);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Errores.Copy(resultadoConsulta.Errores);
                    return resultado;
                }

                var origen = resultadoConsulta.Return;
                if (origen == null)
                {
                    resultado.AddErrorPublico("El origen no existe");
                    return resultado;
                }

                if (origen.FechaBaja != null)
                {
                    resultado.AddErrorPublico("El origen se encuentra dado de baja");
                    return resultado;
                }

                origen.Nombre = comando.Nombre;
                var resultadoUpdate = base.Update(origen);
                if (!resultadoUpdate.Ok)
                {
                    resultado.Errores.Copy(resultadoUpdate.Errores);
                    return resultado;
                }

                resultado.Return = new Resultado_Origen(resultadoUpdate.Return);
                return resultado;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }


        public Result<Resultado_Origen> DarDeBaja(int id)
        {
            var resultado = new Result<Resultado_Origen>();
            var resultadoDelete = DeleteById(id);
            if (!resultadoDelete.Ok)
            {
                resultado.Errores.Copy(resultadoDelete.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_Origen(resultadoDelete.Return);
            return resultado;
        }

        public Result<Resultado_Origen> DarDeAlta(int id)
        {
            var resultado = new Result<Resultado_Origen>();

            var resultadoConsulta = GetById(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Errores.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var origen = resultadoConsulta.Return;
            if (origen == null)
            {
                resultado.AddErrorPublico("El origen no existe");
                return resultado;
            }

            if (origen.FechaBaja == null)
            {
                resultado.AddErrorPublico("El origen no se encuentra dado de baja");
                return resultado;
            }

            origen.FechaBaja = null;
            var resultadoUpdate = Update(origen);
            if (!resultadoUpdate.Ok)
            {
                resultado.Errores.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_Origen(resultadoUpdate.Return);
            return resultado;
        }

        #region GenerarKey

        public Result<bool> ExisteKeyAlias(string key)
        {
            return dao.ExisteKeyAlias(key);
        }

        public Result<bool> ExisteKeySecret(string key)
        {
            return dao.ExisteKeySecret(key);
        }

        public Result<string> GenerarKeyAlias()
        {
            var resultado = new Result<string>();

            try
            {
                string key = null;
                bool yaexiste = true;

                do
                {
                    key = RandomString(200).ToUpper();

                    //Compruebo si ya existe el numero y sigo buscando de ser asi.
                    var resultadoYaExiste = ExisteKeyAlias(key);
                    if (!resultadoYaExiste.Ok)
                    {
                        resultado.Errores.Copy(resultadoYaExiste.Errores);
                        return resultado;

                    }
                    yaexiste = resultadoYaExiste.Return;

                } while (yaexiste);


                resultado.Return = key;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<string> GenerarKeySecret()
        {
            var resultado = new Result<string>();

            try
            {
                string key = null;
                bool yaexiste = true;

                do
                {
                    key = RandomString(200).ToUpper();

                    //Compruebo si ya existe el numero y sigo buscando de ser asi.
                    var resultadoYaExiste = ExisteKeySecret(key);
                    if (!resultadoYaExiste.Ok)
                    {
                        resultado.Errores.Copy(resultadoYaExiste.Errores);
                        return resultado;

                    }
                    yaexiste = resultadoYaExiste.Return;

                } while (yaexiste);


                resultado.Return = key;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        //funcion para generar random
        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ACDEFGHJKLMNPQRSTUWXZ123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion

        public Result<List<Resultado_Origen>> GetAmbitosDeUsuarioLogeado()
        {
            var resultado = new Result<List<Resultado_Origen>>();

            var usuarioLogeado = getUsuarioLogueado();
            if (usuarioLogeado == null)
            {
                resultado.AddErrorPublico("Debe iniciar sesion");
                return resultado;
            }


            var idsOrigen = new List<int>();

            // 1 - Busco por Usuario
            var resultadoPorUsuario = new OrigenPorUsuarioRules(usuarioLogeado).GetByFilters(new Consulta_OrigenPorUsuario()
            {
                UsuarioId = usuarioLogeado.Usuario.Id,
                DadosDeBaja = false
            });

            if (!resultadoPorUsuario.Ok)
            {
                resultado.Errores.Copy(resultadoPorUsuario.Errores);
                return resultado;
            }
            idsOrigen.AddRange(resultadoPorUsuario.Return.Select(x => x.OrigenId).ToList());

            if (usuarioLogeado.Ambito != null && usuarioLogeado.Ambito.KeyValue != 0)
            {
                // 2 - Si tengo ambito, busco segun ambito

                var resultadoPorAmbito = new OrigenPorAmbitoRules(usuarioLogeado).GetByFilters(new Consulta_OrigenPorAmbito()
                {
                    AmbitoId = usuarioLogeado.Ambito.Id,
                    DadosDeBaja = false
                });

                if (!resultadoPorAmbito.Ok)
                {
                    resultado.Errores.Copy(resultadoPorAmbito.Errores);
                    return resultado;
                }

                if (resultadoPorAmbito.Return == null || resultadoPorAmbito.Return.Count == 0 || resultadoPorAmbito.Return[0].FechaBaja != null)
                {
                    resultado.AddErrorPublico("Error procesando la solicitud");
                    resultado.AddErrorInterno("El ambito " + usuarioLogeado.Ambito.Nombre + " no tiene asociado ningun origen, o se encuentra dado de baja");
                    return resultado;
                }

                idsOrigen.AddRange(resultadoPorAmbito.Return.Select(x => x.OrigenId).ToList());
            }
            else
            {
                // 2 - Si no tengo ambito, busco segun areas

                if (usuarioLogeado.Areas == null || usuarioLogeado.Areas.Count == 0)
                {
                    resultado.AddErrorPublico("El usuario logeado no tiene areas");
                    return resultado;
                }

                var yaAgregueAreaDefault = false;
                foreach (var area in usuarioLogeado.Areas)
                {
                    var resultadoPorArea = new OrigenPorAreaRules(usuarioLogeado).GetByFilters(new Consulta_OrigenPorArea()
                    {
                        AreaId = area.Id,
                        DadosDeBaja = false
                    });

                    if (!resultadoPorArea.Ok)
                    {
                        resultado.Errores.Copy(resultadoPorArea.Errores);
                        return resultado;
                    }

                    if (resultadoPorArea.Return == null || resultadoPorArea.Return.Count == 0)
                    {
                        //Si no hay ningun origen para el area, debo agregar el area por default. (Solo una vez)

                        if (!yaAgregueAreaDefault)
                        {
                            yaAgregueAreaDefault = true;

                            var keyAlias = ConfigurationManager.AppSettings["ORIGEN_AREA_DEFAULT_KEY_ALIAS"] + "";
                            var keySecret = ConfigurationManager.AppSettings["ORIGEN_AREA_DEFAULT_KEY_SECRET"] + "";
                            if (keyAlias == null || keySecret == null)
                            {
                                resultado.AddErrorPublico("Error procesando la solicitud");
                                resultado.AddErrorInterno("No se encontraron los key alias y secret del area por default");
                                return resultado;
                            }

                            var resultadoOrigenAreaDefault = new OrigenRules(usuarioLogeado).GetByFilters(new Consulta_Origen()
                            {
                                KeyAlias = keyAlias,
                                KeySecret = keySecret
                            });

                            if (!resultadoOrigenAreaDefault.Ok)
                            {
                                resultado.Errores.Copy(resultadoOrigenAreaDefault.Errores);
                                return resultado;
                            }

                            if (resultadoOrigenAreaDefault.Return == null || resultadoOrigenAreaDefault.Return.Count == 0 || resultadoOrigenAreaDefault.Return[0].FechaBaja != null)
                            {
                                resultado.AddErrorPublico("Error procesando la solicitud");
                                resultado.AddErrorInterno("El origen por defaul para las areas no existe o esta dado de baja");
                                return resultado;
                            }

                            //Agrego el id al listado
                            idsOrigen.Add(resultadoOrigenAreaDefault.Return[0].Id);
                        }
                    }
                    else
                    {
                        //Si encontre algun origen asociado al area lo agrego al listado
                        idsOrigen.AddRange(resultadoPorArea.Return.Select(x => x.OrigenId).ToList());
                    }

                }
            }

            var origenes = new List<Resultado_Origen>();
            idsOrigen = idsOrigen.Distinct().ToList();

            foreach (var idOrigen in idsOrigen)
            {
                var resultadoConsulta = new OrigenRules(usuarioLogeado).GetById(idOrigen);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Errores.Copy(resultadoConsulta.Errores);
                    return resultado;
                }

                var origen = resultadoConsulta.Return;
                if (origen == null || origen.FechaBaja != null)
                {
                    resultado.AddErrorPublico("Error procesando la solicitud");
                    resultado.AddErrorPublico("Uno de los origenes del usuario logeado no existe o esta dado de baja");
                    return resultado;
                }

                origenes.Add(new Resultado_Origen(origen));
            }

            //Si o si tiene que haber origenes para el usuario
            if (origenes.Count() == 0)
            {
                resultado.AddErrorPublico("Error procesando la solicitud");
                resultado.AddErrorInterno("El usuario logeado no tiene ningun origen posible");
                return resultado;
            }

            resultado.Return = origenes;
            return resultado;
        }

    }
}
