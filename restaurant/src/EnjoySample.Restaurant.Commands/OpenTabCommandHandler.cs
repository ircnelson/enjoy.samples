using System.Threading.Tasks;
using EnjoyCQRS.Commands;
using EnjoyCQRS.EventSource.Storage;
using EnjoySample.Restaurant.Domain.Tab;

namespace EnjoySample.Restaurant.Commands
{
    public class OpenTabCommandHandler : ICommandHandler<OpenTabCommand>
    {
        private readonly IRepository _domainRepository;

        public OpenTabCommandHandler(IRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public async Task ExecuteAsync(OpenTabCommand command)
        {
            var tab = TabAggregate.Create(command.TableNumber, command.Waiter);

            await _domainRepository.AddAsync(tab);
        }
    }
}