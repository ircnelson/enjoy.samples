using EnjoyCQRS.Commands;
using EnjoyCQRS.EventStore;
using EnjoySample.Restaurant.Commands;
using EnjoySample.Restaurant.Domain;

namespace EnjoySample.Restaurant.CommandsHandlers
{
    public class OpenTabCommandHandler : ICommandHandler<OpenTabCommand>
    {
        private readonly IDomainRepository _domainRepository;

        public OpenTabCommandHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public void Execute(OpenTabCommand command)
        {
            var tab = TabAggregate.Create(command.TableNumber, command.Waiter);

            _domainRepository.Add(tab);
        }
    }
}