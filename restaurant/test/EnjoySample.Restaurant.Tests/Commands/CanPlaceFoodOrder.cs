using System;
using System.Collections.Generic;
using System.Linq;
using EnjoyCQRS.Events;
using EnjoyCQRS.TestFramework;
using EnjoySample.Restaurant.Commands;
using EnjoySample.Restaurant.CommandsHandlers;
using EnjoySample.Restaurant.Domain;
using EnjoySample.Restaurant.Domain.ValueObjects;
using EnjoySample.Restaurant.Events;
using FluentAssertions;
using Xunit;

namespace EnjoySample.Restaurant.Tests.Commands
{
    public class CanPlaceFoodOrder : CommandTestFixture<PlaceOrderCommand, PlaceOrderCommandHandler, TabAggregate>
    {
        private Guid _tabAggregate;
        private OrderedItem _testFood1;
        private OrderedItem _testFood2;

        protected override IEnumerable<IDomainEvent> Given()
        {
            _tabAggregate = Guid.NewGuid();

            _testFood1 = new OrderedItem
            {
                MenuNumber = 16,
                Description = "Beef Noodles",
                Price = 7.50M,
                IsDrink = false
            };

            _testFood2 = new OrderedItem
            {
                MenuNumber = 25,
                Description = "Vegetable Curry",
                Price = 6.00M,
                IsDrink = false
            };

            yield return PrepareDomainEvent.Set(new TabOpenedEvent(_tabAggregate, 1, "Nelson")).ToVersion(1);
        }

        protected override PlaceOrderCommand When()
        {
            return new PlaceOrderCommand(_tabAggregate, new List<OrderedItem> {_testFood1, _testFood2});
        }

        [Fact]
        public void Then_foods_ordered_event_will_be_published()
        {
            PublishedEvents.Last().Should().BeAssignableTo<FoodOrderedEvent>();
        }
    }
}