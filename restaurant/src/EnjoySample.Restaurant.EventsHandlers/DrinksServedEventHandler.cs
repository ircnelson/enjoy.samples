﻿using System.Linq;
using System.Threading.Tasks;
using EnjoyCQRS.Events;
using EnjoySample.Restaurant.Domain.Tab;
using EnjoySample.Restaurant.Read;
using EnjoySample.Restaurant.Read.Models;

namespace EnjoySample.Restaurant.EventsHandlers
{
    public class DrinksServedEventHandler : IEventHandler<DrinksServedEvent>
    {
        private readonly IReadRepository<TabModel> _repository;

        public DrinksServedEventHandler(IReadRepository<TabModel> repository)
        {
            _repository = repository;
        }

        public Task ExecuteAsync(DrinksServedEvent theEvent)
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

            return Task.CompletedTask;
        }
    }
}