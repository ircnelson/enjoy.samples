using System;
using System.Collections.Generic;
using EnjoyCQRS.Commands;
using EnjoySample.Restaurant.Domain.ValueObjects;

namespace EnjoySample.Restaurant.Commands
{
    [Serializable]
    public class PlaceOrderCommand : Command
    {
        public IEnumerable<OrderedItem> OrderedItems { get; }

        public PlaceOrderCommand(Guid aggregateId, IEnumerable<OrderedItem> orderedItems) : base(aggregateId)
        {
            OrderedItems = orderedItems;
        }
    }
}