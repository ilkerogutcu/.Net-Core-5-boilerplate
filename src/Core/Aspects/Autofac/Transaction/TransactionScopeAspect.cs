#region

using System;
using System.Transactions;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;

#endregion

namespace Core.Aspects.Autofac.Transaction
{
    /// <summary>
    ///     Transaction Scope Aspect
    /// </summary>
    public class TransactionScopeAspect : MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            using var transactionScope = new TransactionScope();
            try
            {
                invocation.Proceed();
                transactionScope.Complete();
            }
            catch (Exception e)
            {
                transactionScope.Dispose();
                throw;
            }
        }
    }
}