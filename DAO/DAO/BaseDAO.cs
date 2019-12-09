using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Collections;
using System.Data;
using Model;

namespace DAO.DAO
{
    public class BaseDAO<Entity> where Entity : BaseEntity
    {
        private static BaseDAO<Entity> instance;

        public static BaseDAO<Entity> Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BaseDAO<Entity>();
                }
                return instance;
            }
        }

        public ISession GetSession()
        {
            return SessionManager.Instance.GetSession();
        }

        public bool Transaction(Func<bool> action)
        {
            return SessionManager.Instance.Transaction(action);
        }

        public virtual Result<Entity> Insert(Entity entity)
        {
            var result = new Result<Entity>();

            Transaction(() =>
        {
            //Ejecto el insert
            try
            {
                GetSession().Save(entity);
                GetSession().Flush();
                result.Return = entity;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result.Ok;
        });
            return result;
        }

        public Result<bool> ValidarExiste(int id)
        {
            var result = new Result<bool>();

            var session = GetSession().QueryOver<Entity>();
            result.Return = session.Where(x => x.Id == id && x.FechaBaja == null).RowCount() != 0;
            return result;
        }

        public Result<List<Entity>> Insert(List<Entity> entities)
        {
            var result = new Result<List<Entity>>();

            Transaction(() =>
            {
                //Ejecuto el update
                foreach (var entity in entities)
                {
                    try
                    {
                        GetSession().Save(entity);
                    }
                    catch (Exception e)
                    {
                        result.AddErrorInterno(e);
                    }

                    if (result.Ok)
                    {
                        result.Return = entities;
                        GetSession().Flush();
                    }
                }

                return result.Ok;
            });

            return result;
        }

        public Result<Entity> Update(int id, Entity entity)
        {
            var result = new Result<Entity>();

            Transaction(() =>
            {
                //Ejecturo el update
                try
                {
                    entity.Id = id;
                    GetSession().Merge(entity);
                    GetSession().Flush();
                    result.Return = entity;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e);
                }

                return result.Ok;
            });

            return result;
        }

        public Result<Entity> Update(Entity entity)
        {
            return Update(entity.Id, entity);
        }

        public Result<List<Entity>> Update(List<Entity> entities)
        {
            var result = new Result<List<Entity>>();

            Transaction(() =>
            {
                //Ejecuto el update
                foreach (var entity in entities)
                {
                    try
                    {
                        GetSession().Merge(entity);
                    }
                    catch (Exception e)
                    {
                        result.AddErrorInterno(e);
                    }

                    if (result.Ok)
                    {
                        result.Return = entities;
                        GetSession().Flush();
                    }
                }

                return result.Ok;
            });

            return result;
        }

        public Result<Entity> Delete(Entity entity)
        {
            var result = new Result<Entity>();

            Transaction(() =>
            {

                //Ejecuto el delete
                try
                {
                    GetSession().Delete(entity);
                    GetSession().Flush();
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e);
                }

                return result.Ok;
            });

            return result;
        }

        public Result<List<Entity>> Delete(List<Entity> entities)
        {
            var result = new Result<List<Entity>>();

            Transaction(() =>
            {

                //Ejecuto el delete
                foreach (var entity in entities)
                {
                    try
                    {
                        GetSession().Delete(entity);
                    }
                    catch (Exception e)
                    {
                        result.AddErrorInterno(e);
                    }

                    if (result.Ok)
                    {
                        result.Return = entities;
                        GetSession().Flush();
                    }
                }

                return result.Ok;
            });

            return result;
        }

        public void Evict(Entity entity)
        {
            GetSession().Evict(entity);
        }

        public Result<Entity> GetById(int id)
        {
            var result = new Result<Entity>();

            try
            {
                result.Return = GetSession().Get<Entity>(id);
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public virtual Result<Entity> GetByIdObligatorio(int id)
        {
            var result = new Result<Entity>();

            try
            {
                var obj = GetSession().Get<Entity>(id);
                //GetSession().Refresh(obj);
                result.Return = obj;

                if (result.Return == null)
                {
                    result.AddErrorPublico("La entidad consultada no existe");
                    return result;
                }
                if (result.Return.FechaBaja != null)
                {
                    result.AddErrorPublico("La entidad consultada se encuentra dada de baja");
                    return result;
                }

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<List<Entity>> GetById(List<int> ids)
        {
            var result = new Result<List<Entity>>();

            try
            {
                var session = GetSession().QueryOver<Entity>();
                session.Where(x => x.Id.IsIn(ids));
                result.Return = new List<Entity>(session.List());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<List<Entity>> GetAll()
        {
            return GetAll(null);
        }

        public Result<List<Entity>> GetAll(bool? dadosDeBaja)
        {
            var result = new Result<List<Entity>>();

            try
            {
                var query = GetSession().QueryOver<Entity>();

                if (dadosDeBaja.HasValue)
                {
                    if (dadosDeBaja.Value)
                    {
                        query.Where(x => x.FechaBaja != null);
                    }
                    else
                    {
                        query.Where(x => x.FechaBaja == null);
                    }
                }
                result.Return = new List<Entity>(query.List());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }
        public bool EjecutarEnOtraSession(Func<bool> func)
        {
            return SessionManager.Instance.EjecutarEnOtraSession(func);
        }

        public Result<List<T>> ProcedimientoAlmacenado<T>(string name)
        {
            var resultado = new Result<List<T>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec " + name);

                if (!typeof(T).IsPrimitive && typeof(T) != typeof(Decimal) && typeof(T) != typeof(String) && typeof(T) != typeof(object) && typeof(T) != typeof(Hashtable))
                {
                    query.SetResultTransformer(Transformers.AliasToBean<T>());
                    resultado.Return = query.List<T>().ToList();
                }
                else
                {
                    var data = new List<T>();

                    if (typeof(T) == typeof(Hashtable))
                    {
                        query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToEntityMap);
                        data = query.List<T>().ToList();
                    }
                    else
                    {
                        foreach (var item in query.List<object>().ToList())
                        {
                            data.Add((T)item);
                        }
                    }

                    resultado.Return = data;
                }
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<DataTable> ProcedimientoAlmacenadoDataTable(string name)
        {
            var resultado = new Result<DataTable>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec " + name);
                var a = query.SetResultTransformer(new DataTableResultTransformer());
                var list = a.List<DataTable>();
                resultado.Return = (DataTable)list[0];
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }


        public class DataTableResultTransformer : IResultTransformer
        {
            private DataTable dataTable;

            public IList TransformList(IList collection)
            {
                var rows = collection.Cast<DataRow>().ToList();
                foreach (var row in rows)
                {
                    dataTable.Rows.Add(row);
                }
                return new List<DataTable> { dataTable };
            }

            public object TransformTuple(object[] tuple, string[] aliases)
            {
                //Create the table schema based on aliases if its not already done
                CreateDataTable(aliases);

                //Create and Fill DataRow
                return FillDataRow(tuple, aliases);
            }

            private DataRow FillDataRow(object[] tuple, string[] aliases)
            {
                DataRow dataRow = dataTable.NewRow();
                aliases.ToList().ForEach(alias =>
                {
                    dataRow[alias] = tuple[Array.FindIndex(aliases, colName => colName == alias)];
                });
                return dataRow;
            }

            private void CreateDataTable(IEnumerable<string> aliases)
            {
                if (dataTable == null)
                {
                    dataTable = new DataTable();
                    aliases.ToList().ForEach(alias => dataTable.Columns.Add(alias));
                }
            }
        }
    }
}
