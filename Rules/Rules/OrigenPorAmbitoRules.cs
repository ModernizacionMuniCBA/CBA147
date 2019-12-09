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
    public class OrigenPorAmbitoRules : BaseRules<OrigenPorAmbito>
    {
        private readonly OrigenPorAmbitoDAO dao;

        public OrigenPorAmbitoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = OrigenPorAmbitoDAO.Instance;
        }


        public override Result<OrigenPorAmbito> ValidateDatosNecesarios(OrigenPorAmbito entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Origen
            if (entity.Origen == null)
            {
                result.AddErrorPublico("Debe ingresar el Origen");
                return result;
            }

            //Ambito
            if (entity.Ambito == null)
            {
                result.AddErrorPublico("Debe ingresar el Ambito");
            }


            return result;
        }

        public Result<List<Resultado_OrigenPorAmbito>> GetByFilters(Consulta_OrigenPorAmbito consulta)
        {
            return dao.GetByFilters(consulta);
        }

        public Result<Resultado_OrigenPorAmbito> Insertar(Comando_OrigenPorAmbito comando)
        {
            var resultado = new Result<Resultado_OrigenPorAmbito>();
            try
            {
                //Valido Ambito
                var consultaAmbito = new _CerrojoAmbitoRules(getUsuarioLogueado()).GetById(comando.AmbitoId);
                if (!consultaAmbito.Ok)
                {
                    resultado.Errores.Copy(consultaAmbito.Errores);
                    return resultado;
                }

                if (consultaAmbito.Return == null || consultaAmbito.Return.FechaBaja != null)
                {
                    resultado.AddErrorPublico("El ambito indicado no existe");
                    return resultado;
                }
                var ambito = consultaAmbito.Return;

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
                var consulta = new Consulta_OrigenPorAmbito();
                consulta.AmbitoId = comando.AmbitoId;

                var resultadoConsultaExistentes = GetByFilters(consulta);
                if (!resultadoConsultaExistentes.Ok)
                {
                    resultado.Errores.Copy(resultadoConsultaExistentes.Errores);
                    return resultado;
                }

                var existentes = resultadoConsultaExistentes.Return;

                var origenPorAmbito = new OrigenPorAmbito();

                //Si ya existe algun origen para el area indicada traigo ese registro para pisarle el valor de origen por el nuevo
                if (existentes != null && existentes.Count != 0)
                {
                    var resultadoConsultaExistente = GetById(existentes[0].Id);
                    if (!resultadoConsultaExistente.Ok)
                    {
                        resultado.Errores.Copy(resultadoConsultaExistente.Errores);
                        return resultado;
                    }

                    origenPorAmbito = resultadoConsultaExistente.Return;
                }

                if (origenPorAmbito == null)
                {
                    resultado.AddErrorPublico("Error procesando la solicitud");
                    return resultado;
                }

                //Seteo los datos
                origenPorAmbito.Origen = origen;
                origenPorAmbito.Ambito = ambito;
                origenPorAmbito.FechaBaja = null; //Si estaba dado de baja lo activo

                Result<OrigenPorAmbito> resultadoUpdate;
                if (origenPorAmbito.Id != 0)
                {
                    resultadoUpdate = Update(origenPorAmbito);
                }
                else
                {
                    resultadoUpdate = Insert(origenPorAmbito);
                }
                if (!resultadoUpdate.Ok)
                {
                    resultado.Errores.Copy(resultadoUpdate.Errores);
                    return resultado;
                }


                resultado.Return = new Resultado_OrigenPorAmbito(resultadoUpdate.Return);
                return resultado;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<Resultado_OrigenPorAmbito> DarDeBaja(int id)
        {
            var resultado = new Result<Resultado_OrigenPorAmbito>();
            var resultadoDelete = DeleteById(id);
            if (!resultadoDelete.Ok)
            {
                resultado.Errores.Copy(resultadoDelete.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_OrigenPorAmbito(resultadoDelete.Return);
            return resultado;
        }

        public Result<Resultado_OrigenPorAmbito> DarDeAlta(int id)
        {
            var resultado = new Result<Resultado_OrigenPorAmbito>();

            var resultadoConsulta = GetById(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Errores.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var origen = resultadoConsulta.Return;
            if (origen == null)
            {
                resultado.AddErrorPublico("El origen por ambito no existe");
                return resultado;
            }

            if (origen.FechaBaja == null)
            {
                resultado.AddErrorPublico("El origen por area no se encuentra dado de baja");
                return resultado;
            }

            origen.FechaBaja = null;
            var resultadoUpdate = Update(origen);
            if (!resultadoUpdate.Ok)
            {
                resultado.Errores.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_OrigenPorAmbito(resultadoUpdate.Return);
            return resultado;
        }
    }
}
