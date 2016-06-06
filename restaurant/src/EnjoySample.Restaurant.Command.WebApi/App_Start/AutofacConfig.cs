using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using Autofac.Integration.WebApi;
using EnjoyCQRS.Commands;
using EnjoyCQRS.Events;
using EnjoyCQRS.EventSource;
using EnjoyCQRS.EventSource.Storage;
using EnjoyCQRS.MessageBus;
using EnjoyCQRS.MessageBus.InProcess;
using EnjoySample.Restaurant.Commands;
using EnjoySample.Restaurant.EventsHandlers;
using LiteDB;

namespace EnjoySample.Restaurant.Command.WebApi
{
    public class AutofacConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Register(c => new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "\\Storage.db"));
            builder.RegisterType<EventStore.LiteDB.EventStore>().As<IEventStore>();

            builder.RegisterType<Session>().As<ISession>().InstancePerRequest();
            builder.RegisterType<Repository>().As<IRepository>().InstancePerDependency();
            builder.RegisterType<EventPublisher>().As<IEventPublisher>();

            builder.RegisterType<EventRouter>().As<IEventRouter>().InstancePerLifetimeScope();
            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(OpenTabCommandHandler).Assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>));

            builder.RegisterAssemblyTypes(typeof(TabOpenedEventHandler).Assembly)
                .AsClosedTypesOf(typeof(IEventHandler<>));
        }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;

        public UnitOfWork(ISession session)
        {
            _session = session;
        }

        public async Task CommitAsync()
        {
            await _session.SaveChangesAsync();
        }
    }

    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ILifetimeScope _scope;

        public CommandDispatcher(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            await InternalDispatchAsync((dynamic)command);
        }

        private async Task InternalDispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = _scope.Resolve<ICommandHandler<TCommand>>();

            await handler.ExecuteAsync(command);
        }
    }

    public class EventRouter : IEventRouter
    {
        private readonly ILifetimeScope _scope;

        public EventRouter(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public async Task RouteAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            var handlers = _scope.ResolveOptional<IEnumerable<IEventHandler<TEvent>>>();

            foreach (var handler in handlers)
            {
                await handler.ExecuteAsync(@event);
            }
        }
    }
}