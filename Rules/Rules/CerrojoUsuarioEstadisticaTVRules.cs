using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DAO.DAO;
using Model;
using Model.Entities;
using Encriptacion;
using Model.Resultados;
using Model.Consultas;

namespace Rules.Rules
{

    public class CerrojoUsuarioEstadisticaTVRules: BaseRules<_VecinoVirtualUsuario>
    {

        private readonly CerrojoUsuarioEstadisticaTVDAO dao;

        public CerrojoUsuarioEstadisticaTVRules(UsuarioLogueado data)
            : base(data)
        {
            dao = CerrojoUsuarioEstadisticaTVDAO.Instance;
        }

        public Result<bool> TengoPermiso()
        {
            return dao.TengoPermiso(getUsuarioLogueado().Usuario);
        }

    }


}
