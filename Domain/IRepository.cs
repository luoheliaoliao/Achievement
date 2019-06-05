using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        /// <summary>
        /// 将指定的聚合根添加到仓储中。
        /// </summary>
        /// <param name="aggregateRoot">需要添加到仓储的聚合根实例。</param>
        TAggregateRoot Add(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 批量添加聚合根。
        /// </summary>
        /// <param name="aggregateRoots">需要添加到仓储的聚合根实例。</param>
        List<TAggregateRoot> Add(List<TAggregateRoot> aggregateRoots);

        /// <summary>
        /// 将指定的聚合根添加到仓储中。
        /// </summary>
        /// <param name="aggregateRoot">需要添加到仓储的聚合根实例。</param>
        TAggregateRoot FullAdd(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 批量添加聚合根。
        /// </summary>
        /// <param name="aggregateRoots">需要添加到仓储的聚合根实例。</param>
        List<TAggregateRoot> FullAdd(List<TAggregateRoot> aggregateRoots);

        /// <summary>
        /// 根据聚合根的ID值，从仓储中读取聚合根。
        /// </summary>
        /// <param name="id">聚合根的ID值。</param>
        /// <returns>聚合根实例。</returns>
        TAggregateRoot GetById(long id);

        /// <summary>
        /// 根据聚合根的ID值，从仓储中读取聚合根。
        /// </summary>
        /// <param name="idList">聚合根的Id值。</param>
        /// <returns>聚合根实例。</returns>
        List<TAggregateRoot> GetListByIdList(List<long> idList);

        ///// <summary>
        ///// 将指定的聚合根从仓储中移除。
        ///// </summary>
        ///// <param name="id">需要从仓储中移除的聚合根。</param>
        //int Delete(long id);

        /// <summary>
        /// 将指定的聚合根从仓储中移除。
        /// </summary>
        /// <param name="aggregateRoot">需要从仓储中移除的聚合根。</param>
        int Delete(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 将指定的聚合根从仓储中移除。
        /// </summary>
        /// <param name="aggregateRoots">需要从仓储中移除的聚合根。</param>
        int Delete(List<TAggregateRoot> aggregateRoots);

        /// <summary>
        /// 将指定的聚合根从仓储中移除。
        /// </summary>
        /// <param name="aggregateRoot">需要从仓储中移除的聚合根。</param>
        int FullDelete(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 将指定的聚合根从仓储中移除。
        /// </summary>
        /// <param name="aggregateRoots">需要从仓储中移除的聚合根。</param>
        int FullDelete(List<TAggregateRoot> aggregateRoots);

        /// <summary>
        /// 更新指定的聚合根。
        /// </summary>
        /// <param name="aggregateRoot">需要更新的聚合根。</param>
        int Update(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 更新指定的聚合根。
        /// </summary>
        /// <param name="aggregateRoots">需要更新的聚合根。</param>
        int Update(List<TAggregateRoot> aggregateRoots);

        /// <summary>
        /// 更新指定的聚合根。
        /// </summary>
        /// <param name="aggregateRoot">需要更新的聚合根。</param>
        int FullUpdate(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 更新指定的聚合根。
        /// </summary>
        /// <param name="aggregateRoots">需要更新的聚合根。</param>
        int FullUpdate(List<TAggregateRoot> aggregateRoots);

        /// <summary>
        /// 获取全部的聚合根，该方法容易造成数据过度查询，请谨慎使用！
        /// </summary>
        List<TAggregateRoot> GetAll();

        /// <summary>
        /// 移除全部的聚合根。
        /// </summary>
        int Clear();

    }
}
