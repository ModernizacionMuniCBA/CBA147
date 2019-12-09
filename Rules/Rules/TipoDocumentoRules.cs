using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;

namespace Rules.Rules
{
    public class TipoDocumentoRules : BaseRules<TipoDocumento>
    {

        private readonly TipoDocumentoDAO dao;

        public TipoDocumentoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = TipoDocumentoDAO.Instance;
        }
    }
}
