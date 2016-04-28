using System;
using System.Collections.Generic;
using EnjoyCQRS.Events;
using EnjoyCQRS.TestFramework;
using EnjoySample.Restaurant.Commands;
using EnjoySample.Restaurant.CommandsHandlers;
using EnjoySample.Restaurant.Domain;
using EnjoySample.Restaurant.Domain.Exceptions;
using EnjoySample.Restaurant.Domain.ValueObjects;
using EnjoySample.Restaurant.Events;
using Xunit;

namespace EnjoySample.Restaurant.Tests.Commands
{
    public class MustPayEnoughToCloseTab : CommandTestFixture<CloseTabCommand, CloseTabCommandHandler, TabAggregate>
    {
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
            return new CloseTabCommand(_tabAggregate, _testDrink1.Price - 0.5M);
        }

        [Fact]
        public void Then_MustPayEnough_exception_should_be_throws()
        {
            Assert.Equal(typeof (MustPayEnough), CaughtException.GetType());
        }
    }
}