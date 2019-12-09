using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;

namespace Rules.Rules
{
    public class TipoMotivoRules
    {
        public TipoMotivoRules(UsuarioLogueado data)
        {
           
        }

        public Resultado_Enums GetByKeyValue(Enums.TipoMotivo tipo)
        {
            return new Resultado_Enums((int)tipo, tipo.ToString());
        }

        public List<Resultado_Enums> GetAll()
        {
            var tipos = new List<Resultado_Enums>();
            tipos.Add(new Resultado_Enums((int)Enums.TipoMotivo.GENERAL, Enums.TipoMotivo.GENERAL.ToString()));
            tipos.Add(new Resultado_Enums((int)Enums.TipoMotivo.INTERNO, Enums.TipoMotivo.INTERNO.ToString()));
            tipos.Add(new Resultado_Enums((int)Enums.TipoMotivo.PRIVADO, Enums.TipoMotivo.PRIVADO.ToString())); 
            return tipos;
        }
    }
}
