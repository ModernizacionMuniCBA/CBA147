using System;
using System.Linq;
using Model.Entities;
using Model;
using System.Collections.Generic;
using System.Text;
using Model.Resultados;
using Model.Utiles;

namespace DAO.DAO
{
    public class DomicilioDAO : BaseDAO<Domicilio>
    {
        private static DomicilioDAO instance;

        public static DomicilioDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DomicilioDAO();
                }
                return instance;
            }
        }

        public Result<bool> MigrarDomicilios()
        {
            var resultado = new Result<bool>();

            //using (var session = SessionManager.Instance.SessionFactory.OpenSession())
            //{
            //    session.SetBatchSize(1000);
            //    using (var trans = session.BeginTransaction())
            //    {
            //        try
            //        {
            //            var domicilios = session.QueryOver<Domicilio>().Where(x => x.FechaBaja == null && x.Xcatastro != null && x.Ycatastro != null).List().ToList();

            //            foreach (var d in domicilios)
            //            {
            //                var coord = GoogleMapsUtils.Convertir(d.Xcatastro, d.Ycatastro);
            //                if (coord != null)
            //                {
            //                    d.Direccion = d.DireccionGoogleMaps;
            //                    d.Latitud = ("" + coord[0]).Replace(".", ",");
            //                    d.Longitud = ("" + coord[1]).Replace(".", ",");
            //                    session.Update(d);
            //                }
            //            }

            //            resultado.Return = true;
            //            trans.Commit();
            //        }
            //        catch (Exception e)
            //        {
            //            trans.Rollback();
            //        }
            //    }
            //}
            return resultado;
        }

    }
}
