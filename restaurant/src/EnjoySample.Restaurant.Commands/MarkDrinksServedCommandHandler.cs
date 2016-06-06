using System.Threading.Tasks;
using EnjoyCQRS.Commands;
using EnjoyCQRS.EventSource.Storage;
using EnjoySample.Restaurant.Domain.Tab;

namespace EnjoySample.Restaurant.Commands
{
    public class MarkDrinksServedCommandHandler : ICommandHandler<MarkDrinksServedCommand>
    {
        private readonly IRepository _domainRepository;

        public MarkDrinksServedCommandHandler(IRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public async Task ExecuteAsync(MarkDrinksServedCommand command)
        {
            var tab = await _domainRepository.GetByIdAsync<TabAggregate>(command.AggregateId);
            tab.MarkDrinksServed(command.MenuNumbers);
        }
    }
}