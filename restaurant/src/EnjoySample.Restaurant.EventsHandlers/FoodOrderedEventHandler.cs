using System;
using System.Threading.Tasks;
using EnjoyCQRS.Events;
using EnjoySample.Restaurant.Domain.Tab;
using EnjoySample.Restaurant.Read;
using EnjoySample.Restaurant.Read.Models;

namespace EnjoySample.Restaurant.EventsHandlers
{
    public class FoodOrderedEventHandler : IEventHandler<FoodOrderedEvent>
    {
        private readonly IReadRepository<OrderItemModel> _repository;

        public FoodOrderedEventHandler(IReadRepository<OrderItemModel> repository)
        {
            _repository = repository;
        }

        public Task ExecuteAsync(FoodOrderedEvent theEvent)
        {
            var orderItemModel = new OrderItemModel(Guid.NewGuid(), theEvent.AggregateId, theEvent.MenuNumber, theEvent.Price, theEvent.Status);

            _repository.Insert(orderItemModel);

            return Task.CompletedTask;
        }
    }
}