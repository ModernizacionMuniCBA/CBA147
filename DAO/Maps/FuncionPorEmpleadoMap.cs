using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class FuncionPorEmpleadoMap : BaseEntityMap<FuncionPorEmpleado>
    {
        public FuncionPorEmpleadoMap()
        {
            //Tabla
            Table("FuncionPorEmpleado");

            //Funcion
            References(x => x.Funcion, "IdFuncion").Not.Nullable();

            //Empleado
            References(x => x.Empleado, "IdEmpleado").Not.Nullable();

        }
    }
}
