using EnjoyCQRS.Events;
using EnjoySample.Restaurant.Events;
using EnjoySample.Restaurant.Read;
using EnjoySample.Restaurant.Read.Models;

namespace EnjoySample.Restaurant.EventsHandlers
{
    public class TabClosedEventHandler : IEventHandler<TabClosedEvent>
    {
        private readonly IReadRepository<TabModel> _tabRepository;

        public TabClosedEventHandler(IReadRepository<TabModel> tabRepository)
        {
            _tabRepository = tabRepository;
        }

        public void Execute(TabClosedEvent theEvent)
        {
            var tabModel = _tabRepository.GetById(theEvent.AggregateId);

            if (tabModel != null)
            {
                tabModel.AmountPaid = theEvent.AmountPaid;
                tabModel.IsOpen = false;
                tabModel.OrderedValue = theEvent.OrdersValue;
                tabModel.TipValue = theEvent.TipValue;
            }

            _tabRepository.Update(tabModel);
        }
    }
}