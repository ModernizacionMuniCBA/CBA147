using System;
using System.Linq;
using Model;
using Rules.Rules;
using Rules;

namespace Intranet_Servicios2.MisRules
{
    public class _WSRules_Base<Entity> where Entity : BaseEntity
    {
        private readonly BaseRules<Entity> rules;

        private UsuarioLogueado data;

        protected UsuarioLogueado getUsuarioLogueado()
        {
            return data;
        }

        protected void setUsuarioLogueado(UsuarioLogueado data)
        {
            this.data = data;
        }

        private _WSRules_Base()
        {
            rules = new BaseRules<Entity>(data);
        }

        public _WSRules_Base(UsuarioLogueado data)
            : this()
        {
            this.data = data;
        }
    }
}