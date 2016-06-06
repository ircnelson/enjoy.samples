using System;
using System.Threading.Tasks;
using EnjoyCQRS.Events;
using EnjoySample.Restaurant.Domain.Tab;
using EnjoySample.Restaurant.Read;
using EnjoySample.Restaurant.Read.Models;

namespace EnjoySample.Restaurant.EventsHandlers
{
    public class DrinksOrderedEventHandler : IEventHandler<DrinksOrderedEvent>
    {
        private readonly IReadRepository<TabModel> _repository;

        public DrinksOrderedEventHandler(IReadRepository<TabModel> repository)
        {
            _repository = repository;
        }

        public Task ExecuteAsync(DrinksOrderedEvent theEvent)
        {
            var tab = _repository.GetById(theEvent.AggregateId);

            var newOrder = new OrderItemModel(Guid.NewGuid(), theEvent.AggregateId, theEvent.MenuNumber, theEvent.Price, theEvent.Status);
            tab.OrderedItems.Add(newOrder);

            _repository.Update(tab);

            return Task.CompletedTask;
        }
    }
}