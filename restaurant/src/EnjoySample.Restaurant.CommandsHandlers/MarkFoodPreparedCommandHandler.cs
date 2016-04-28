using EnjoyCQRS.Commands;
using EnjoyCQRS.EventStore;
using EnjoySample.Restaurant.Commands;
using EnjoySample.Restaurant.Domain;

namespace EnjoySample.Restaurant.CommandsHandlers
{
    public class MarkFoodPreparedCommandHandler : ICommandHandler<MarkFoodPreparedCommand>
    {
        private readonly IDomainRepository _domainRepository;

        public MarkFoodPreparedCommandHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public void Execute(MarkFoodPreparedCommand command)
        {
            var tab = _domainRepository.GetById<TabAggregate>(command.AggregateId);
            tab.MarkFoodPrepared(command.MenuNumbers);
        }
    }
}