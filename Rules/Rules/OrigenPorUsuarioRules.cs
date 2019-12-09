using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Model.Consultas;
using Model.Comandos;

namespace Rules.Rules
{
    public class OrigenPorUsuarioRules : BaseRules<OrigenPorUsuario>
    {
        private readonly OrigenPorUsuarioDAO dao;

        public OrigenPorUsuarioRules(UsuarioLogueado data)
            : base(data)
        {
            dao = OrigenPorUsuarioDAO.Instance;
        }


        public override Result<OrigenPorUsuario> ValidateDatosNecesarios(OrigenPorUsuario entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Origen
            if (entity.Origen == null)
            {
                result.AddErrorPublico("Debe ingresar el Origen");
                return result;
            }

            //Usuario
            if (entity.Usuario == null)
            {
                result.AddErrorPublico("Debe ingresar el usuario");
            }


            return result;
        }

        public Result<List<Resultado_OrigenPorUsuario>> GetByFilters(Consulta_OrigenPorUsuario consulta)
        {
            return dao.GetByFilters(consulta);
        }

        public Result<Resultado_OrigenPorUsuario> Insertar(Comando_OrigenPorUsuario comando)
        {
            var resultado = new Result<Resultado_OrigenPorUsuario>();
            try
            {
                //Valido Usuario
                var consultaUsuario = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetById(comando.UsuarioId);
                if (!consultaUsuario.Ok)
                {
                    resultado.Errores.Copy(consultaUsuario.Errores);
                    return resultado;
                }

                if (consultaUsuario.Return == null || consultaUsuario.Return.FechaBaja != null)
                {
                    resultado.AddErrorPublico("El usuario indicado no existe");
                    return resultado;
                }
                var usuario = consultaUsuario.Return;

                //Valido Origen
                var consultaOrigen = new OrigenRules(getUsuarioLogueado()).GetById(comando.OrigenId);
                if (!consultaOrigen.Ok)
                {
                    resultado.Errores.Copy(consultaOrigen.Errores);
                    return resultado;

                }

                if (consultaOrigen.Return == null || consultaOrigen.Return.FechaBaja != null)
                {
                    resultado.AddErrorPublico("El origen indicado no existe");
                    return resultado;
                }
                var origen = consultaOrigen.Return;


                //Busco si ya existe algun origen para el area indicada
                var consulta = new Consulta_OrigenPorUsuario();
                consulta.UsuarioId = comando.UsuarioId;

                var resultadoConsultaExistentes = GetByFilters(consulta);
                if (!resultadoConsultaExistentes.Ok)
                {
                    resultado.Errores.Copy(resultadoConsultaExistentes.Errores);
                    return resultado;
                }

                var existentes = resultadoConsultaExistentes.Return;

                var origenPorUsuario = new OrigenPorUsuario();

                //Si ya existe algun origen para el area indicada traigo ese registro para pisarle el valor de origen por el nuevo
                if (existentes != null && existentes.Count != 0)
                {
                    var resultadoConsultaExistente = GetById(existentes[0].Id);
                    if (!resultadoConsultaExistente.Ok)
                    {
                        resultado.Errores.Copy(resultadoConsultaExistente.Errores);
                        return resultado;
                    }

                    origenPorUsuario = resultadoConsultaExistente.Return;
                }

                if (origenPorUsuario == null)
                {
                    resultado.AddErrorPublico("Error procesando la solicitud");
                    return resultado;
                }

                //Seteo los datos
                origenPorUsuario.Origen = origen;
                origenPorUsuario.UsuarioOrigen =usuario;
                origenPorUsuario.FechaBaja = null; //Si estaba dado de baja lo activo

                Result<OrigenPorUsuario> resultadoUpdate;
                if (origenPorUsuario.Id != 0)
                {
                    resultadoUpdate = Update(origenPorUsuario);
                }
                else
                {
                    resultadoUpdate = Insert(origenPorUsuario);
                }
                if (!resultadoUpdate.Ok)
                {
                    resultado.Errores.Copy(resultadoUpdate.Errores);
                    return resultado;
                }


                resultado.Return = new Resultado_OrigenPorUsuario(resultadoUpdate.Return);
                return resultado;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<Resultado_OrigenPorUsuario> DarDeBaja(int id)
        {
            var resultado = new Result<Resultado_OrigenPorUsuario>();
            var resultadoDelete = DeleteById(id);
            if (!resultadoDelete.Ok)
            {
                resultado.Errores.Copy(resultadoDelete.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_OrigenPorUsuario(resultadoDelete.Return);
            return resultado;
        }

        public Result<Resultado_OrigenPorUsuario> DarDeAlta(int id)
        {
            var resultado = new Result<Resultado_OrigenPorUsuario>();

            var resultadoConsulta = GetById(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Errores.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var origen = resultadoConsulta.Return;
            if (origen == null)
            {
                resultado.AddErrorPublico("El origen por usuario no existe");
                return resultado;
            }

            if (origen.FechaBaja == null)
            {
                resultado.AddErrorPublico("El origen por usuario no se encuentra dado de baja");
                return resultado;
            }

            origen.FechaBaja = null;
            var resultadoUpdate = Update(origen);
            if (!resultadoUpdate.Ok)
            {
                resultado.Errores.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_OrigenPorUsuario(resultadoUpdate.Return);
            return resultado;
        }
    }
}
