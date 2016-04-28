using EnjoyCQRS.Commands;
using EnjoyCQRS.EventStore;
using EnjoySample.Restaurant.Commands;
using EnjoySample.Restaurant.Domain;

namespace EnjoySample.Restaurant.CommandsHandlers
{
    public class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand>
    {
        private readonly IDomainRepository _domainRepository;

        public PlaceOrderCommandHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public void Execute(PlaceOrderCommand command)
        {
            var tab = _domainRepository.GetById<TabAggregate>(command.AggregateId);
            tab.PlaceOrder(command.OrderedItems);
        }
    }
}