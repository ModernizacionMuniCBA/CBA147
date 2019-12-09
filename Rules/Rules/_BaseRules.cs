using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model;
using Model.Entities;

namespace Rules.Rules
{
    public class BaseRules<Entity> where Entity : BaseEntity
    {
        private UsuarioLogueado data;

        protected UsuarioLogueado getUsuarioLogueado()
        {
            return data;
        }

        private readonly BaseDAO<Entity> dao;

        public BaseRules()
        {
            dao = BaseDAO<Entity>.Instance;
        }

        public BaseRules(UsuarioLogueado data)
            : this()
        {
            this.data = data;
        }



        public virtual Result<Entity> Insert(Entity entity)
        {
            var result = ValidateInsert(entity);
            if (!result.Ok)
            {
                return result;
            }
            return dao.Insert(entity);
        }

        public virtual Result<Entity> ValidateInsert(Entity entity)
        {
            if (data != null)
            {
                entity.Usuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(data.Usuario.Id).Return;
            }
            entity.FechaAlta = DateTime.Now;

            var result = new Result<Entity>();

            //---------------------------
            // Datos Necesarios
            //---------------------------

            var resultDatosNecesarios = ValidateDatosNecesarios(entity);
            if (!resultDatosNecesarios.Ok)
            {
                result.Copy(resultDatosNecesarios);
                return result;
            }

            //--------------------------
            // Duplicado
            //--------------------------

            var resultCantidadDuplicados = BuscarCantidadDuplicados(entity);
            if (!resultCantidadDuplicados.Ok)
            {
                result.Copy(resultCantidadDuplicados.Errores);
                return result;
            }

            if (resultCantidadDuplicados.Return != 0)
            {
                result.AddErrorPublico(MensajeDuplicado(entity));
            }

            if (!result.Ok)
            {
                return result;
            }

            CorregirDatos(entity);

            result.Return = entity;
            return result;
        }

        public virtual Result<Entity> Update(Entity entity)
        {
            var result = ValidateUpdate(entity);
            if (!result.Ok)
            {
                return result;
            }
            return dao.Update(entity);
        }

        public virtual Result<Entity> ValidateUpdate(Entity entity)
        {
            entity.FechaModificacion = DateTime.Now;
            if (data != null)
            {
                entity.Usuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(data.Usuario.Id).Return;
            }

            var result = new Result<Entity>();

            //---------------------------
            // Datos necesarios
            //---------------------------

            var resultDatosNecesarios = ValidateDatosNecesarios(entity);
            if (!resultDatosNecesarios.Ok)
            {
                result.Copy(resultDatosNecesarios);
                return result;
            }

            //Fecha de Alta
            if (entity.FechaModificacion == null)
            {
                result.AddErrorInterno("Falta la fecha de modificacion");
                return result;
            }

            //--------------------------
            // Duplicado
            //--------------------------

            var resultCantidadDuplicados = BuscarCantidadDuplicados(entity);
            if (!resultCantidadDuplicados.Ok)
            {
                result.Copy(resultCantidadDuplicados.Errores);
                return result;
            }

            if (resultCantidadDuplicados.Return != 0)
            {
                result.AddErrorPublico(MensajeDuplicado(entity));
            }

            if (!result.Ok)
            {
                return result;
            }


            CorregirDatos(entity);
            result.Return = entity;
            return result;
        }

        public virtual Result<Entity> Delete(Entity entity)
        {
            var result = ValidateDelete(entity);
            if (!result.Ok)
            {
                return result;
            }
            return dao.Update(entity);
        }

        public virtual Result<Entity> ValidateDelete(Entity entity)
        {
            if (data != null)
            {
                entity.Usuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(data.Usuario.Id).Return;
            }
            entity.FechaBaja = DateTime.Now;

            var result = new Result<Entity>();

            //---------------------------
            // Datos necesarios 
            //---------------------------

            var resultDatosNecesarios = ValidateDatosNecesarios(entity);
            if (!resultDatosNecesarios.Ok)
            {
                result.Copy(resultDatosNecesarios);
                return result;
            }

            //Id
            if (entity.Id <= 0)
            {
                result.AddErrorInterno("La entidad nunca dada de alta");
                return result;
            }



            if (!result.Ok)
            {
                return result;
            }
            result.Return = entity;
            return result;
        }

        public virtual Result<Entity> DeleteById(int id)
        {
            var resultQuery = GetById(id);
            if (!resultQuery.Ok)
            {
                return resultQuery;
            }

            var entity = resultQuery.Return;
            if (entity.FechaAlta == null)
            {
                entity.FechaAlta = DateTime.Now;
            }
            return Delete(entity);
        }

        public Result<Entity> GetById(int id)
        {
            return dao.GetById(id);
        }

        public Result<Entity> GetByIdObligatorio(int id)
        {
            return dao.GetByIdObligatorio(id);
        }

        public Result<List<Entity>> GetById(List<int> ids)
        {
            return dao.GetById(ids);
        }

        public Result<List<Entity>> GetAll()
        {
            return dao.GetAll();
        }

        public Result<List<Entity>> GetAll(bool? dadosDeBaja)
        {
            return dao.GetAll(dadosDeBaja);
        }

        /* Validaciones */

        public virtual Result<Entity> ValidateDatosNecesarios(Entity entity)
        {
            var result = new Result<Entity>();

            //Fecha Alta
            if (entity.FechaAlta == null)
            {
                result.AddErrorInterno("Falta la fecha de alta");
                return result;
            }

            //Usuario
            if (entity.Usuario == null)
            {
                result.AddErrorInterno("Falta el usuario");
                return result;
            }

            //Usuario
            if (entity.Observaciones != null)
            {

            }

            result.Return = entity;
            return result;
        }

        public virtual Result<int> BuscarCantidadDuplicados(Entity entity)
        {
            var duplicados = new Result<int>();
            duplicados.Return = 0;
            return duplicados;
        }

        public virtual string MensajeDuplicado(Entity entity)
        {
            return "Entidad duplicada";
        }

        protected virtual void CorregirDatos(Entity entity)
        {

        }


        public Result<List<T>> ProcedimientoAlmacenado<T>(string name)
        {
            return dao.ProcedimientoAlmacenado<T>(name);
        }
    }
}
