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
    public class CanCloseTabWithTip : CommandTestFixture<CloseTabCommand, CloseTabCommandHandler, TabAggregate>
    {
        private const decimal TipValue = 0.5M;

        private Guid _tabAggregate;
        private OrderedItem _testDrink1;

        protected override IEnumerable<IDomainEvent> Given()
        {
            _tabAggregate = Guid.NewGuid();

            _testDrink1 = new OrderedItem
            {
                MenuNumber = 4,
                Description = "Sprite",
                Price = 1.50M,
                IsDrink = true
            };

            yield return PrepareDomainEvent.Set(new TabOpenedEvent(_tabAggregate, 1, "Nelson")).ToVersion(1);
            yield return PrepareDomainEvent.Set(new DrinksOrderedEvent(_tabAggregate, _testDrink1.Description, _testDrink1.MenuNumber, _testDrink1.Price, _testDrink1.Status.ToString())).ToVersion(2);
            yield return PrepareDomainEvent.Set(new DrinksServedEvent(_tabAggregate, new List<int> {_testDrink1.MenuNumber})).ToVersion(3);
        }

        protected override CloseTabCommand When()
        {
            return new CloseTabCommand(_tabAggregate, _testDrink1.Price + TipValue);
        }

        [Fact]
        public void Then_tab_closed_event_will_be_published()
        {
            PublishedEvents.Last().Should().BeAssignableTo<TabClosedEvent>();
        }

        [Fact]
        public void Then_tip_should_be_more_than_zero()
        {
            PublishedEvents.Last().As<TabClosedEvent>().TipValue.Should().Be(TipValue);
        }
    }
}