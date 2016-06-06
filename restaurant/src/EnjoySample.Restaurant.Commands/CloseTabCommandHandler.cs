using System.Threading.Tasks;
using EnjoyCQRS.Commands;
using EnjoyCQRS.EventSource.Storage;
using EnjoySample.Restaurant.Domain.Tab;

namespace EnjoySample.Restaurant.Commands
{
    public class CloseTabCommandHandler : ICommandHandler<CloseTabCommand>
    {
        private readonly IRepository _domainRepository;

        public CloseTabCommandHandler(IRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }
        public async Task ExecuteAsync(CloseTabCommand command)
        {
            var tab = await _domainRepository.GetByIdAsync<TabAggregate>(command.AggregateId);
            tab.CloseTab(command.AmountPaid);
        }
    }
}