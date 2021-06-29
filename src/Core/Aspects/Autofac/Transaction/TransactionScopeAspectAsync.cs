#region

using System;
using System.Transactions;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Core.Aspects.Autofac.Transaction
{
    public class TransactionScopeAspectAsync : MethodInterception
    {
        private readonly Type _dbContextType;

        public void InterceptDbContext(IInvocation invocation)
        {
            var db = ServiceTool.ServiceProvider.GetService(_dbContextType) as DbContext;
            using var transactionScope = db.Database.BeginTransaction();
            try
            {
                invocation.Proceed();
                transactionScope.Commit();
            }
            finally
            {
                transactionScope.Rollback();
            }
        }

        public override void Intercept(IInvocation invocation)
        {
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            invocation.Proceed();
            tx.Complete();
        }
    }
}