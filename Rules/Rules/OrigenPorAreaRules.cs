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
    public class OrigenPorAreaRules : BaseRules<OrigenPorArea>
    {
        private readonly OrigenPorAreaDAO dao;

        public OrigenPorAreaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = OrigenPorAreaDAO.Instance;
        }


        public override Result<OrigenPorArea> ValidateDatosNecesarios(OrigenPorArea entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Origen
            if (entity.Origen == null)
            {
                result.AddErrorPublico("Debe ingresar el Origen");
                return result;
            }

            //Area
            if (entity.Area == null)
            {
                result.AddErrorPublico("Debe ingresar el Area");
            }


            return result;
        }

        public Result<List<Resultado_OrigenPorArea>> GetByFilters(Consulta_OrigenPorArea consulta)
        {
            return dao.GetByFilters(consulta);
        }

        public Result<Resultado_OrigenPorArea> Insertar(Comando_OrigenPorArea comando)
        {
            var resultado = new Result<Resultado_OrigenPorArea>();
            try
            {
                //Valido Area
                var consultaArea = new _CerrojoAreaRules(getUsuarioLogueado()).GetById(comando.AreaId);
                if (!consultaArea.Ok)
                {
                    resultado.Errores.Copy(consultaArea.Errores);
                    return resultado;
                }

                if (consultaArea.Return == null || consultaArea.Return.FechaBaja != null)
                {
                    resultado.AddErrorPublico("El area indicada no existe");
                    return resultado;
                }
                var area = consultaArea.Return;

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
                var consulta = new Consulta_OrigenPorArea();
                consulta.AreaId = comando.AreaId;

                var resultadoConsultaExistentes = GetByFilters(consulta);
                if (!resultadoConsultaExistentes.Ok)
                {
                    resultado.Errores.Copy(resultadoConsultaExistentes.Errores);
                    return resultado;
                }

                var existentes = resultadoConsultaExistentes.Return;

                var origenPorArea = new OrigenPorArea();

                //Si ya existe algun origen para el area indicada traigo ese registro para pisarle el valor de origen por el nuevo
                if (existentes != null && existentes.Count != 0)
                {
                    var resultadoConsultaExistente = GetById(existentes[0].Id);
                    if (!resultadoConsultaExistente.Ok)
                    {
                        resultado.Errores.Copy(resultadoConsultaExistente.Errores);
                        return resultado;
                    }

                    origenPorArea = resultadoConsultaExistente.Return;
                }

                if (origenPorArea == null)
                {
                    resultado.AddErrorPublico("Error procesando la solicitud");
                    return resultado;
                }

                //Seteo los datos
                origenPorArea.Area = area;
                origenPorArea.Origen = origen;
                origenPorArea.FechaBaja = null; //Si estaba dado de baja lo activo

                Result<OrigenPorArea> resultadoUpdate;
                if (origenPorArea.Id != 0)
                {
                    resultadoUpdate = Update(origenPorArea);
                }
                else
                {
                    resultadoUpdate = Insert(origenPorArea);
                }
                if (!resultadoUpdate.Ok)
                {
                    resultado.Errores.Copy(resultadoUpdate.Errores);
                    return resultado;
                }


                resultado.Return = new Resultado_OrigenPorArea(resultadoUpdate.Return);
                return resultado;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<Resultado_OrigenPorArea> DarDeBaja(int id)
        {
            var resultado = new Result<Resultado_OrigenPorArea>();
            var resultadoDelete = DeleteById(id);
            if (!resultadoDelete.Ok)
            {
                resultado.Errores.Copy(resultadoDelete.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_OrigenPorArea(resultadoDelete.Return);
            return resultado;
        }

        public Result<Resultado_OrigenPorArea> DarDeAlta(int id)
        {
            var resultado = new Result<Resultado_OrigenPorArea>();

            var resultadoConsulta = GetById(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Errores.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var origen = resultadoConsulta.Return;
            if (origen == null)
            {
                resultado.AddErrorPublico("El origen por Area no existe");
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

            resultado.Return = new Resultado_OrigenPorArea(resultadoUpdate.Return);
            return resultado;
        }
    }
}
