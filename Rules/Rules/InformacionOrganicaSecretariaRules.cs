using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using Model.Resultados;
using Model.Comandos;

namespace Rules.Rules
{
    public class InformacionOrganicaSecretariaRules : BaseRules<InformacionOrganicaSecretaria>
    {

        private readonly InformacionOrganicaSecretariaDAO dao;

        public InformacionOrganicaSecretariaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = InformacionOrganicaSecretariaDAO.Instance;
        }

        public override Result<int> BuscarCantidadDuplicados(InformacionOrganicaSecretaria entity)
        {
            int? id = null;
            if (entity.Id != 0)
            {
                id = entity.Id;
            }

            return dao.GetCantidadDuplicados(id, entity.Nombre);
        }

        public override string MensajeDuplicado(InformacionOrganicaSecretaria entity)
        {
            return "Ya existe una secretaria con el mismo nombre";
        }

        public Result<Resultado_InformacionOrganicaSecretaria> GetResultadoById(int id)
        {
            var resultado = new Result<Resultado_InformacionOrganicaSecretaria>();

            var resultadoConsulta = GetById(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_InformacionOrganicaSecretaria(resultadoConsulta.Return);
            return resultado;
        }

        public Result<Resultado_InformacionOrganicaSecretaria> GetResultadoByIdObligatorio(int id)
        {
            var resultado = new Result<Resultado_InformacionOrganicaSecretaria>();

            var resultadoConsulta = GetByIdObligatorio(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_InformacionOrganicaSecretaria(resultadoConsulta.Return);
            return resultado;
        }

        public Result<Resultado_InformacionOrganicaSecretaria> Insertar(Comando_InformacionOrganicaSecretaria comando)
        {
            var resultado = new Result<Resultado_InformacionOrganicaSecretaria>();

            try
            {
                var entity = new InformacionOrganicaSecretaria();
                entity.Nombre = comando.Nombre;

                var resultadoInsert = base.Insert(entity);
                if (!resultadoInsert.Ok)
                {
                    resultado.Copy(resultadoInsert.Errores);
                    return resultado;
                }

                resultado.Return = new Resultado_InformacionOrganicaSecretaria(resultadoInsert.Return);
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<Resultado_InformacionOrganicaSecretaria> Actualizar(Comando_InformacionOrganicaSecretaria comando)
        {
            var resultado = new Result<Resultado_InformacionOrganicaSecretaria>();

            try
            {
                var resultadoConsulta = GetByIdObligatorio(comando.Id.Value);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Copy(resultadoConsulta.Errores);
                    return resultado;
                }
                var entity = resultadoConsulta.Return;
                entity.Nombre = comando.Nombre;

                var resultadoUpdate = base.Update(entity);
                if (!resultadoUpdate.Ok)
                {
                    resultado.Copy(resultadoUpdate.Errores);
                    return resultado;
                }

                resultado.Return = new Resultado_InformacionOrganicaSecretaria(resultadoUpdate.Return);
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<Resultado_InformacionOrganicaSecretaria> DarDeBaja(int id)
        {
            var resultado = new Result<Resultado_InformacionOrganicaSecretaria>();
            var resultadoDelete = DeleteById(id);
            if (!resultadoDelete.Ok)
            {
                resultado.Errores.Copy(resultadoDelete.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_InformacionOrganicaSecretaria(resultadoDelete.Return);
            return resultado;
        }

        public Result<Resultado_InformacionOrganicaSecretaria> DarDeAlta(int id)
        {
            var resultado = new Result<Resultado_InformacionOrganicaSecretaria>();

            var resultadoConsulta = GetById(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Errores.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var entity = resultadoConsulta.Return;
            if (entity == null)
            {
                resultado.AddErrorPublico("La entidad no existe");
                return resultado;
            }

            if (entity.FechaBaja == null)
            {
                resultado.AddErrorPublico("La entidad no se encuentra dado de baja");
                return resultado;
            }

            entity.FechaBaja = null;
            var resultadoUpdate = Update(entity);
            if (!resultadoUpdate.Ok)
            {
                resultado.Errores.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_InformacionOrganicaSecretaria(resultadoUpdate.Return);
            return resultado;
        }

        public Result<List<Resultado_InformacionOrganicaSecretaria>> GetResultadoAll()
        {
            var resultado = new Result<List<Resultado_InformacionOrganicaSecretaria>>();

            var resultadoConsulta = GetAll(false);
            if (!resultadoConsulta.Ok)
            {
                resultado.Errores.Copy(resultadoConsulta.Errores);
                return resultado;
            }


            resultado.Return = Resultado_InformacionOrganicaSecretaria.ToList(resultadoConsulta.Return);
            return resultado;
        }

        public Result<bool> CambiarNombre(int id, string nombre)
        {
            var resultado = new Result<bool>();

            var resultadoConsulta = GetByIdObligatorio(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var entity = resultadoConsulta.Return;
            entity.Nombre = nombre;
           
            var resultadoUpdate = Update(entity);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        public Result<bool> QuitarDireccion(int id, int idDireccion)
        {
            var resultado = new Result<bool>();

            //Busco la secretaria
            var resultadoConsulta = GetByIdObligatorio(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var entity = resultadoConsulta.Return;

            //Busco la direccion
            var resultadoConsultaDireccion = new InformacionOrganicaDireccionRules(getUsuarioLogueado()).GetByIdObligatorio(idDireccion);
            if (!resultadoConsultaDireccion.Ok)
            {
                resultado.Copy(resultadoConsultaDireccion.Errores);
                return resultado;
            }

            var direccion = resultadoConsultaDireccion.Return;

            //Valido que la direccion sea de la secretaria
            if (direccion.Secretaria.Id != id)
            {
                resultado.AddErrorPublico("La dirección indicada no pertenece a la secretaría");
                return resultado;
            }

            //Doy de baja la direccion
            var resultadoUpdate = new InformacionOrganicaDireccionRules(getUsuarioLogueado()).DarDeBaja(idDireccion);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            return resultado;
        }

    }
}
