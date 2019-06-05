using Domain;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public abstract class BaseRepository<TAggregateRoot> : DbAccess, IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        /// <summary>
        /// 将指定的聚合根添加到仓储中。
        /// </summary>
        /// <param name="aggregateRoot">需要添加到仓储的聚合根实例。</param>
        public virtual TAggregateRoot Add(TAggregateRoot aggregateRoot)
        {
            var result = Db.Set<TAggregateRoot>().Insert(aggregateRoot);

            Db.SaveChanges();
            return result;
        }

        /// <summary>
        /// 批量添加聚合根。
        /// </summary>
        /// <param name="aggregateRoots">需要添加到仓储的聚合根实例。</param>
        public virtual List<TAggregateRoot> Add(List<TAggregateRoot> aggregateRoots)
        {
            var result = Db.Set<TAggregateRoot>().InsertRange(aggregateRoots);

            Db.SaveChanges();
            return result.ToList();
        }

        public virtual TAggregateRoot FullAdd(TAggregateRoot aggregateRoot)
        {
            throw new System.NotImplementedException();
        }

        public virtual List<TAggregateRoot> FullAdd(List<TAggregateRoot> aggregateRoots)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 根据聚合根的Id值，从仓储中读取聚合根。
        /// </summary>
        /// <param name="id">聚合根的Id值。</param>
        /// <returns>聚合根实例。</returns>
        public virtual TAggregateRoot GetById(long id)
        {
            if (id == 0)
            {
                return null;
            }
            var result = Db.Set<TAggregateRoot>().AsNoTracking().FirstOrDefault(p => p.Id == id);
            var aggregateRoot = result as ILazyLoad;
            if (aggregateRoot != null)
                aggregateRoot.LazyLoad();

            return result;
        }

        /// <summary>
        /// 根据聚合根的Id值，从仓储中读取聚合根。
        /// </summary>
        /// <param name="idList">聚合根的Id值。</param>
        /// <returns>聚合根实例。</returns>
        public virtual List<TAggregateRoot> GetListByIdList(List<long> idList)
        {
            idList = idList ?? new List<long>();
            var result = Db.Set<TAggregateRoot>().AsNoTracking().Where(p => idList.Contains(p.Id)).ToList();
            foreach (var obj in result)
            {
                var aggregateRoot = obj as ILazyLoad;
                if (aggregateRoot != null)
                    aggregateRoot.LazyLoad();
            }
            return result;
        }

        /// <summary>
        /// 将指定的聚合根从仓储中移除。
        /// </summary>
        /// <param name="aggregateRoot">需要从仓储中移除的聚合根。</param>
        public virtual int Delete(TAggregateRoot aggregateRoot)
        {
            Db.DeleteEntity(aggregateRoot);
            return Db.SaveChanges();
        }

        /// <summary>
        /// 将指定的聚合根从仓储中移除。
        /// </summary>
        /// <param name="aggregateRoots">需要从仓储中移除的聚合根。</param>
        public virtual int Delete(List<TAggregateRoot> aggregateRoots)
        {
            Db.DeleteEntity(aggregateRoots);
            return Db.SaveChanges();
        }

        public virtual int FullDelete(TAggregateRoot aggregateRoot)
        {
            throw new System.NotImplementedException();
        }

        public virtual int FullDelete(List<TAggregateRoot> aggregateRoots)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 更新指定的聚合根。
        /// </summary>
        /// <param name="aggregateRoot">需要更新的聚合根。</param>
        public virtual int Update(TAggregateRoot aggregateRoot)
        {
            Db.ModifiyEntity(aggregateRoot);
            return Db.SaveChanges();
        }

        /// <summary>
        /// 更新指定的聚合根。
        /// </summary>
        /// <param name="aggregateRoots">需要更新的聚合根。</param>
        public virtual int Update(List<TAggregateRoot> aggregateRoots)
        {
            Db.ModifiyEntity(aggregateRoots);
            return Db.SaveChanges();
        }

        public virtual int FullUpdate(TAggregateRoot aggregateRoot)
        {
            throw new System.NotImplementedException();
        }

        public virtual int FullUpdate(List<TAggregateRoot> aggregateRoots)
        {
            throw new System.NotImplementedException();
        }

        public virtual List<TAggregateRoot> GetAll()
        {
            var result = Db.Set<TAggregateRoot>().AsNoTracking().ToList();
            foreach (var obj in result)
            {
                var aggregateRoot = obj as ILazyLoad;
                if (aggregateRoot != null)
                    aggregateRoot.LazyLoad();
            }
            return result;
        }

        public virtual int Clear()
        {
            return Db.Set<TAggregateRoot>().Delete();
        }
    }
}
