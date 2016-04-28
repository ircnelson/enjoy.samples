using System;
using EnjoyCQRS.Events;
using EnjoySample.Restaurant.Events;
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

        public void Execute(FoodOrderedEvent theEvent)
        {
            var orderItemModel = new OrderItemModel(Guid.NewGuid(), theEvent.AggregateId, theEvent.MenuNumber, theEvent.Price, theEvent.Status);

            _repository.Insert(orderItemModel);
        }
    }
}