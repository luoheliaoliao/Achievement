using Infrastructure.EntityFramework;
using Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;


namespace Common
{
    public abstract class BaseDB : DbContext, IDB
    {
        protected BaseDB() { }
        protected BaseDB(DbCompiledModel model) : base(model) { }
        protected BaseDB(string nameOrConnectionString) : base(nameOrConnectionString) { }
        protected BaseDB(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) { }
        protected BaseDB(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext, dbContextOwnsObjectContext) { }
        protected BaseDB(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model) { }
        protected BaseDB(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection) { }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Sku>().Property(p => p.Weight).HasPrecision(18, 3);
            //modelBuilder.Entity<ProductSnapshot>().Property(p => p.Weight).HasPrecision(18, 3);
            //modelBuilder.Entity<InventoryChangeRecord>().Property(p => p.Weight).HasPrecision(18, 3);
            //modelBuilder.Entity<Pos>().Property(p => p.LinTest).HasPrecision(18, 5);
            //modelBuilder.Conventions.Add(new DecimalPrecisionAttributeConvention());
            modelBuilder.Properties().Where(p => p.GetCustomAttributes(typeof(DecimalPrecisionAttribute), false).OfType<DecimalPrecisionAttribute>().Any())
                .Configure(m => m.HasPrecision(m.ClrPropertyInfo.GetCustomAttributes(false).OfType<DecimalPrecisionAttribute>().First().Precision,
                    m.ClrPropertyInfo.GetCustomAttributes(typeof(DecimalPrecisionAttribute), false).OfType<DecimalPrecisionAttribute>().First().Scale));
            modelBuilder.Properties().Configure(ConfigurationAction);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }

        private void ConfigurationAction(ConventionPrimitivePropertyConfiguration obj)
        {
            if (obj.ClrPropertyInfo.Name == "Id")
            {
                obj.IsKey();
                obj.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            }
            else if (obj.ClrPropertyInfo.PropertyType == typeof(string))
            {
                obj.IsRequired();
                //if (obj.ClrPropertyInfo.GetCustomAttributes(typeof(MaxLengthAttribute), false).Length == 0)
                //{
                //    obj.HasMaxLength(50);
                //}
            }
        }

        /// <summary>
        /// 直接修改entity的State，有可能发生异常，导致entity在EF的状态还在，需要在异常的情况下重置entity的State，避免被后续的SaveChange重新保存
        /// </summary>
        public void DeleteEntity<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                Entry<TEntity>(entity).State = EntityState.Deleted;
            }
            catch (Exception e)
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    entry.State = EntityState.Detached;
                }
                throw e;
            }
        }
        /// <summary>
        /// 直接修改entity的State，有可能发生异常，导致entity在EF的状态还在，需要在异常的情况下重置entity的State，避免被后续的SaveChange重新保存
        /// </summary>
        public void DeleteEntity<TEntity>(List<TEntity> entities) where TEntity : class
        {
            try
            {
                foreach (var entity in entities)
                {
                    Entry<TEntity>(entity).State = EntityState.Deleted;
                }
            }
            catch (Exception e)
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    entry.State = EntityState.Detached;
                }
                throw e;
            }
        }

        /// <summary>
        /// 直接修改entity的State，有可能发生异常，导致entity在EF的状态还在，需要在异常的情况下重置entity的State，避免被后续的SaveChange重新保存
        /// </summary>
        public void ModifiyEntity<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                Entry<TEntity>(entity).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    entry.State = EntityState.Detached;
                }
                throw;
            }
        }
        /// <summary>
        /// 直接修改entity的State，有可能发生异常，导致entity在EF的状态还在，需要在异常的情况下重置entity的State，避免被后续的SaveChange重新保存
        /// </summary>
        public void ModifiyEntity<TEntity>(List<TEntity> entities) where TEntity : class
        {
            try
            {
                foreach (var entity in entities)
                {
                    Entry<TEntity>(entity).State = EntityState.Modified;
                }
            }
            catch (Exception e)
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    entry.State = EntityState.Detached;
                }
                throw;
            }
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                if (dbEx.EntityValidationErrors.ToList().Count > 0)
                {
                    if (dbEx.EntityValidationErrors.ToList()[0].ValidationErrors.ToList().Count > 0)
                    {
                        throw new AlertException(dbEx.EntityValidationErrors.ToList()[0].ValidationErrors.ToList()[0].ErrorMessage);
                    }
                }
                throw dbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    entry.State = EntityState.Detached;
                }
            }
        }

        #region 事务

        protected DbTransaction Tran;
        /// <summary>
        /// 事务主键，为了支持嵌套性事务
        /// </summary>
        protected string TranKey;

        public void BeginTransaction(string tranKey = "")
        {
            if (Tran != null)
                return;
            Database.Connection.Open();
            Tran = Database.Connection.BeginTransaction(IsolationLevel.ReadCommitted);
            Database.UseTransaction(Tran);
            TranKey = tranKey;
        }

        public void EndTransaction(string tranKey = "")
        {
            if (TranKey != tranKey)
                return;

            Database.Connection.Close();
            Tran = null;
            TranKey = null;
        }

        public void Commit(string tranKey = "")
        {
            if (TranKey != tranKey)
                return;

            Tran.Commit();
        }

        public void Rollback(string tranKey = "")
        {
            if (TranKey != tranKey)
                return;

            Tran.Rollback();
        }

        #endregion

    }

}
