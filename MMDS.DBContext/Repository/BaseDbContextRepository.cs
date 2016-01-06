using MMDS.DBContext.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMDS.DBContext.Repository
{
    public abstract class BaseDbContextRepository<T, TEntity> : IBaseDynamicRepository<T>, IEntitiesKeys<T>, IDisposable
        where T : class
        where TEntity : DbContext, new()
    {
        private readonly TEntity _entities;

        protected TEntity Entities
        {
            get { return _entities; }
        }

        protected DbSet<T> Set
        {
            get { return _entities.Set<T>(); }
        }

        public IQueryable<T> Items
        {
            get { return Set.AsQueryable(); }
        }

        private string TableName { get; set; }

        public string GetTableName()
        {

            return TableName;
        }

        #region Constructors

        protected BaseDbContextRepository()
        {
            _entities = new TEntity();
        }

        protected BaseDbContextRepository(TEntity entities)
        {
            _entities = entities;
        }

        #endregion

        #region IBaseRepository<T> Members

        /// <summary>
        ///     Pobierz wszystkie
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return Items;
        }

        /// <summary>
        ///     Pobierz przez zapytanie
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<T> Query(Expression<Func<T, bool>> query)
        {
            return Items.Where(query);
        }

        /// <summary>
        ///     Pobierz wszystko przez zapytanie
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Query()
        {
            return Items;
        }

        /// <summary>
        ///     Dodaj
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            item = fillEmptyValues(item);

            Set.Add(item);
        }


        /// <summary>
        ///     Usuń
        /// </summary>
        /// <param name="item"></param>
        public void Delete(T item)
        {
            Set.Remove(item);
        }

        public void DeleteRange(List<T> itemList)
        {
            Set.RemoveRange(itemList);
        }

        /// <summary>
        ///     Zapisz do bazy
        /// </summary>
        public void Save()
        {
            try
            {
                _entities.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string x = ex.ToString();
            }
        }

        /// <summary>
        ///     Usuń obiekt
        /// </summary>
        public void Dispose()
        {
            if (_entities != null)
            {
                _entities.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Uzupełnia wartości null wartościami domyślnymi
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private T fillEmptyValues(T item)
        {
            Type type = item.GetType();

            foreach (PropertyInfo f in type.GetProperties())
            {
                if ((f.GetValue(item) == null) || (f.PropertyType.Name == "DateTime"))
                {
                    switch (f.PropertyType.Name)
                    {
                        case "Int32":
                            f.SetValue(item, 0);
                            break;

                        case "String":
                            f.SetValue(item, "");
                            break;

                        case "DateTime":
                            if (((DateTime)f.GetValue(item)).Year == 1)
                                f.SetValue(item, new DateTime(1753, 1, 1));
                            break;

                        case "Decimal":
                            f.SetValue(item, 0);
                            break;

                        case "Double":
                            f.SetValue(item, 0);
                            break;

                        case "Byte":
                            f.SetValue(item, new byte());
                            break;

                        case "Byte[]":
                            f.SetValue(item, new byte[0]);
                            break;
                    }
                }
            }

            return item;
        }

        #endregion

        #region IEntitiesKeys

        /// <summary>
        ///     Pobiera listę nazw pól "kluczowych"
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeyNames()
        {
            ObjectSet<T> objectSet = ((IObjectContextAdapter)_entities).ObjectContext.CreateObjectSet<T>();
            List<string> keyNames = objectSet.EntitySet.ElementType.KeyMembers.Select(k => k.Name).ToList();
            return keyNames;
        }

        /// <summary>
        ///     Pobiera wartości kluczy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public List<object> GetKeys(T item)
        {
            List<string> keyNames = GetKeyNames();
            Type type = typeof(T);

            var keys = new List<object>();
            foreach (string keyName in keyNames)
            {
                keys.Add(type.GetProperty(keyName).GetValue(item, null));
            }

            return keys;
        }

        /// <summary>
        ///     Pobiera obiekty po kluczach
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public IQueryable<T> GetByKeys(List<object> keys)
        {
            List<string> keyNames = GetKeyNames();
            IQueryable<T> returnItems = Items;

            foreach (string keyname in keyNames)
            {
                object value = keys[keyNames.IndexOf(keyname)];
                returnItems = returnItems.Where(c => c.GetType().GetProperty(keyname).GetValue(c, null).Equals(value));
            }

            return returnItems;
        }

        #endregion
    }
}
