using Model;
using Model.Entities;
using System;
using System.Linq;

namespace DAO.DAO
{
    public class TerritorioIncumbenciaDAO : BaseDAO<TerritorioIncumbencia>
    {
        private static TerritorioIncumbenciaDAO instance;

        public static TerritorioIncumbenciaDAO Instance
        {
            get {
                if (instance == null)
                    instance = new TerritorioIncumbenciaDAO();
                return instance;
            }         
        }
    }
}
