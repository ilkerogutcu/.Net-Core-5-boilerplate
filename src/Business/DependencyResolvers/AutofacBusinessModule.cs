using System;
using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Settings;
using Core.Utilities.Interceptors;
using Core.Utilities.MessageBrokers.RabbitMq;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using FluentValidation;
using MassTransit;
using MediatR;
using Module = Autofac.Module;

namespace Business.DependencyResolvers
{
    /// <summary>
    ///     Register dependencies for business layer
    /// </summary>
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterType<EfProductRepository>().As<IProductRepository>().SingleInstance();
            builder.RegisterType<AuthenticationMailManager>().As<IAuthenticationMailService>().SingleInstance();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .AsClosedTypesOf(typeof(IRequestHandler<,>));
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .AsClosedTypesOf(typeof(IValidator<>));
            builder.RegisterType<RabbitMqProducer>().As<IRabbitMqProducer>().SingleInstance();
            builder.Register(context => BusConfigurator.Bus)
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}