using EnjoyCQRS.Commands;
using EnjoyCQRS.EventStore;
using EnjoySample.Restaurant.Commands;
using EnjoySample.Restaurant.Domain;

namespace EnjoySample.Restaurant.CommandsHandlers
{
    public class MarkFoodServedCommandHandler : ICommandHandler<MarkFoodServedCommand>
    {
        private readonly IDomainRepository _domainRepository;

        public MarkFoodServedCommandHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public void Execute(MarkFoodServedCommand command)
        {
            var tab = _domainRepository.GetById<TabAggregate>(command.AggregateId);
            tab.MarkFoodServed(command.MenuNumbers);
        }
    }
}