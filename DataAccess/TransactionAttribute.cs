using ArxOne.MrAdvice.Advice;
using Common;
using DataAccess;
using Infrastructure;
using System;


namespace Achievement.DataAccess
{
    /// <summary>
    /// 事务标识
    /// </summary>
    [Serializable]
    public class TransactionAttribute : Attribute, IMethodAdvice
    {
        public DbPool DbPool
        {
            get
            {
                return IocContainer.Resolve<IDbPool>() as DbPool;
            }
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="args"></param>
        public void OnEntry(MethodAdviceContext args)
        {
            DbPool.GetDb().BeginTransaction(GetTranKey(args));
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="args"></param>
        public void OnSuccess(MethodAdviceContext args)
        {
            DbPool.GetDb().Commit(GetTranKey(args));
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="args"></param>
        public void OnException(MethodAdviceContext args)
        {
            DbPool.GetDb().Rollback(GetTranKey(args));
        }

        /// <summary>
        /// 结束事务
        /// </summary>
        /// <param name="args"></param>
        public void OnExit(MethodAdviceContext args)
        {
            DbPool.GetDb().EndTransaction(GetTranKey(args));
        }

        private string GetTranKey(MethodAdviceContext args)
        {
            return args.TargetMethod.GetHashCode().ToString();
        }

        public void Advise(MethodAdviceContext context)
        {
            OnEntry(context);
            try
            {
                context.Proceed();
                OnSuccess(context);
            }
            catch (Exception e)
            {
                OnException(context);
                throw;
            }
            finally
            {
                OnExit(context);
            }
        }
    }
}
