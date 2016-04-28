using System.Linq;
using EnjoyCQRS.Events;
using EnjoySample.Restaurant.Events;
using EnjoySample.Restaurant.Read;
using EnjoySample.Restaurant.Read.Models;

namespace EnjoySample.Restaurant.EventsHandlers
{
    public class FoodServedEventHandler : IEventHandler<FoodServedEvent>
    {
        private readonly IReadRepository<TabModel> _repository;

        public FoodServedEventHandler(IReadRepository<TabModel> repository)
        {
            _repository = repository;
        }

        public void Execute(FoodServedEvent theEvent)
        {
            var tab = _repository.GetById(theEvent.AggregateId);

            foreach (var menuNumber in theEvent.MenuNumbers)
            {
                var order = tab.OrderedItems.FirstOrDefault(e => e.Status == "Ordered" && e.MenuNumber == menuNumber);

                if (order != null)
                {
                    order.Status = "Served";

                    tab.OrderedValue += order.Price;
                }
            }

            _repository.Update(tab);
        }
    }
}