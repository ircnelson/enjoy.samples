using System.Threading.Tasks;
using EnjoyCQRS.Commands;
using EnjoyCQRS.EventSource.Storage;
using EnjoySample.Restaurant.Domain.Tab;

namespace EnjoySample.Restaurant.Commands
{
    public class MarkFoodServedCommandHandler : ICommandHandler<MarkFoodServedCommand>
    {
        private readonly IRepository _domainRepository;

        public MarkFoodServedCommandHandler(IRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public async Task ExecuteAsync(MarkFoodServedCommand command)
        {
            var tab = await _domainRepository.GetByIdAsync<TabAggregate>(command.AggregateId);
            tab.MarkFoodServed(command.MenuNumbers);
        }
    }
}