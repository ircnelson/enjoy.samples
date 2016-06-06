using System.Threading.Tasks;
using EnjoyCQRS.Commands;
using EnjoyCQRS.EventSource.Storage;
using EnjoySample.Restaurant.Domain.Tab;

namespace EnjoySample.Restaurant.Commands
{
    public class MarkFoodPreparedCommandHandler : ICommandHandler<MarkFoodPreparedCommand>
    {
        private readonly IRepository _domainRepository;

        public MarkFoodPreparedCommandHandler(IRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public async Task ExecuteAsync(MarkFoodPreparedCommand command)
        {
            var tab = await _domainRepository.GetByIdAsync<TabAggregate>(command.AggregateId);
            tab.MarkFoodPrepared(command.MenuNumbers);
        }
    }
}