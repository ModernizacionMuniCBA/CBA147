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
    public class InformacionOrganicaDireccionRules : BaseRules<InformacionOrganicaDireccion>
    {

        private readonly InformacionOrganicaDireccionDAO dao;

        public InformacionOrganicaDireccionRules(UsuarioLogueado data)
            : base(data)
        {
            dao = InformacionOrganicaDireccionDAO.Instance;
        }

        public override Result<int> BuscarCantidadDuplicados(InformacionOrganicaDireccion entity)
        {
            int? id = null;
            if(entity.Id!=0)id = entity.Id;
            return dao.GetCantidadDuplicados(id, entity.Secretaria.Id, entity.Nombre);
        }

        public override string MensajeDuplicado(InformacionOrganicaDireccion entity)
        {
            return "Ya existe una direccion con ese nombre";
        }

        public Result<Resultado_InformacionOrganicaDireccion> Insertar(Comando_InformacionOrganicaDireccion comando)
        {
            var resultado = new Result<Resultado_InformacionOrganicaDireccion>();

            try
            {
                var entity = new InformacionOrganicaDireccion();
                entity.Secretaria = new InformacionOrganicaSecretariaRules(getUsuarioLogueado()).GetByIdObligatorio(comando.IdSecretaria).Return;
                entity.Nombre = comando.Nombre;
                entity.Responsable = comando.Responsable;
                entity.Telefono = comando.Telefono;
                entity.Email = comando.Email;
                entity.Domicilio = comando.Domicilio;
                entity.UsuarioCreador = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetByIdObligatorio(getUsuarioLogueado().Usuario.Id).Return;

                var resultadoInsert = base.Insert(entity);
                if (!resultadoInsert.Ok)
                {
                    resultado.Copy(resultadoInsert.Errores);
                    return resultado;
                }

                resultado.Return = new Resultado_InformacionOrganicaDireccion(resultadoInsert.Return);
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<Resultado_InformacionOrganicaDireccion> Actualizar(Comando_InformacionOrganicaDireccion comando)
        {
            var resultado = new Result<Resultado_InformacionOrganicaDireccion>();

            try
            {
                var resultadoConsulta = GetByIdObligatorio(comando.Id.Value);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Copy(resultadoConsulta.Errores);
                    return resultado;
                }
                var entity = resultadoConsulta.Return;
                entity.Secretaria = new InformacionOrganicaSecretariaRules(getUsuarioLogueado()).GetByIdObligatorio(comando.IdSecretaria).Return;
                entity.Nombre = comando.Nombre;
                entity.Responsable = comando.Responsable;
                entity.Telefono = comando.Telefono;
                entity.Email = comando.Email;
                entity.Domicilio = comando.Domicilio;

                var resultadoUpdate = base.Update(entity);
                if (!resultadoUpdate.Ok)
                {
                    resultado.Copy(resultadoUpdate.Errores);
                    return resultado;
                }

                resultado.Return = new Resultado_InformacionOrganicaDireccion(resultadoUpdate.Return);
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<InformacionOrganicaDireccion>> GetByIdSecretaria(int id)
        {
            return dao.GetByIdSecretaria(id);
        }

        public Result<List<Resultado_InformacionOrganicaDireccion>> GetResultadoByIdSecretaria(int id)
        {
            var resultado = new Result<List<Resultado_InformacionOrganicaDireccion>>();
            var resultadoConsulta = dao.GetByIdSecretaria(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            resultado.Return = Resultado_InformacionOrganicaDireccion.ToList(resultadoConsulta.Return);
            return resultado;

        }

        public Result<Resultado_InformacionOrganicaDireccion> DarDeBaja(int id)
        {
            var resultado = new Result<Resultado_InformacionOrganicaDireccion>();
            var resultadoDelete = DeleteById(id);
            if (!resultadoDelete.Ok)
            {
                resultado.Errores.Copy(resultadoDelete.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_InformacionOrganicaDireccion(resultadoDelete.Return);
            return resultado;
        }

        public Result<Resultado_InformacionOrganicaDireccion> DarDeAlta(int id)
        {
            var resultado = new Result<Resultado_InformacionOrganicaDireccion>();

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

            resultado.Return = new Resultado_InformacionOrganicaDireccion(resultadoUpdate.Return);
            return resultado;
        }

        public Result<Resultado_InformacionOrganicaDireccion> GetResultadoById(int id)
        {
            var resultado = new Result<Resultado_InformacionOrganicaDireccion>();

            var resultadoConsulta = GetById(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Errores.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return != null)
            {
                resultado.Return = new Resultado_InformacionOrganicaDireccion(resultadoConsulta.Return);
            }

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

        public Result<bool> CambiarResponsable(int id, string responsable)
        {
            var resultado = new Result<bool>();

            var resultadoConsulta = GetByIdObligatorio(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var entity = resultadoConsulta.Return;
            entity.Responsable = responsable;


            var resultadoUpdate = Update(entity);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        public Result<bool> CambiarTelefono(int id, string telefono)
        {
            var resultado = new Result<bool>();

            var resultadoConsulta = GetByIdObligatorio(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var entity = resultadoConsulta.Return;
            entity.Telefono = telefono;


            var resultadoUpdate = Update(entity);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        public Result<bool> CambiarDomicilio(int id, string domicilio)
        {
            var resultado = new Result<bool>();

            var resultadoConsulta = GetByIdObligatorio(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var entity = resultadoConsulta.Return;
            entity.Domicilio = domicilio;


            var resultadoUpdate = Update(entity);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        public Result<bool> CambiarEmail(int id, string email)
        {
            var resultado = new Result<bool>();

            var resultadoConsulta = GetByIdObligatorio(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var entity = resultadoConsulta.Return;
            entity.Email = email;

            var resultadoUpdate = Update(entity);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

    }
}
