using System.Linq;
using System.Threading.Tasks;
using EnjoyCQRS.Events;
using EnjoySample.Restaurant.Domain.Tab;
using EnjoySample.Restaurant.Read;
using EnjoySample.Restaurant.Read.Models;

namespace EnjoySample.Restaurant.EventsHandlers
{
    public class FoodPreparedEventHandler : IEventHandler<FoodPreparedEvent>
    {
        private readonly IReadRepository<TabModel> _tabRepository;

        public FoodPreparedEventHandler(IReadRepository<TabModel> tabRepository)
        {
            _tabRepository = tabRepository;
        }


        public Task ExecuteAsync(FoodPreparedEvent theEvent)
        {
            var tab = _tabRepository.GetById(theEvent.AggregateId);

            foreach (var menuNumber in theEvent.MenuNumbers)
            {
                var orderedItem = tab.OrderedItems.FirstOrDefault(e => e.MenuNumber == menuNumber);

                if (orderedItem != null)
                {
                    orderedItem.Status = "Prepared";

                    _tabRepository.Update(tab);
                }
            }

            return Task.CompletedTask;
        }
    }
}