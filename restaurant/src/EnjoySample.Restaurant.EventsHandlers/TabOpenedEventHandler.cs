using System.Threading.Tasks;
using EnjoyCQRS.Events;
using EnjoySample.Restaurant.Domain.Tab;
using EnjoySample.Restaurant.Read;
using EnjoySample.Restaurant.Read.Models;

namespace EnjoySample.Restaurant.EventsHandlers
{
    public class TabOpenedEventHandler : IEventHandler<TabOpenedEvent>
    {
        private readonly IReadRepository<TabModel> _tabRepository;

        public TabOpenedEventHandler(IReadRepository<TabModel> tabRepository)
        {
            _tabRepository = tabRepository;
        }

        public Task ExecuteAsync(TabOpenedEvent theEvent)
        {
            var tab = new TabModel(theEvent.AggregateId, theEvent.TableNumber, theEvent.Waiter);
            _tabRepository.Insert(tab);

            return Task.CompletedTask;
        }
    }
}