using System.Threading.Tasks;
using EnjoyCQRS.Commands;
using EnjoyCQRS.EventSource.Storage;
using EnjoySample.Restaurant.Domain.Tab;

namespace EnjoySample.Restaurant.Commands
{
    public class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand>
    {
        private readonly IRepository _domainRepository;

        public PlaceOrderCommandHandler(IRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public async Task ExecuteAsync(PlaceOrderCommand command)
        {
            var tab = await _domainRepository.GetByIdAsync<TabAggregate>(command.AggregateId);
            tab.PlaceOrder(command.OrderedItems);
        }
    }
}