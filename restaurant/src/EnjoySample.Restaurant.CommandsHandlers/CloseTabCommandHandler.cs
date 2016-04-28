using EnjoyCQRS.Commands;
using EnjoyCQRS.EventStore;
using EnjoySample.Restaurant.Commands;
using EnjoySample.Restaurant.Domain;

namespace EnjoySample.Restaurant.CommandsHandlers
{
    public class CloseTabCommandHandler : ICommandHandler<CloseTabCommand>
    {
        private readonly IDomainRepository _domainRepository;

        public CloseTabCommandHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }
        public void Execute(CloseTabCommand command)
        {
            var tab = _domainRepository.GetById<TabAggregate>(command.AggregateId);
            tab.CloseTab(command.AmountPaid);
        }
    }
}