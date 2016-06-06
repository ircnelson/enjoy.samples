﻿using System;
using System.Collections.Generic;
using EnjoyCQRS.Events;
using EnjoyCQRS.TestFramework;
using EnjoySample.Restaurant.Commands;
using EnjoySample.Restaurant.Domain;
using EnjoySample.Restaurant.Domain.Exceptions;
using EnjoySample.Restaurant.Domain.Tab;
using EnjoySample.Restaurant.Domain.ValueObjects;
using Xunit;

namespace EnjoySample.Restaurant.Tests.Commands
{
    public class CanNotServePreparedFoodTwice :
        CommandTestFixture<MarkFoodServedCommand, MarkFoodServedCommandHandler, TabAggregate>
    {
        private Guid _tabAggregate;
        private OrderedItem _testFood1;

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

            yield return PrepareDomainEvent.Set(new TabOpenedEvent(_tabAggregate, 1, "Nelson")).ToVersion(1);
            yield return PrepareDomainEvent.Set(new FoodOrderedEvent(_tabAggregate, _testFood1.Description, _testFood1.MenuNumber, _testFood1.Price, _testFood1.Status.ToString())).ToVersion(2);
            yield return PrepareDomainEvent.Set(new FoodPreparedEvent(_tabAggregate, new List<int> {_testFood1.MenuNumber})).ToVersion(3);
            yield return PrepareDomainEvent.Set(new FoodServedEvent(_tabAggregate, new List<int> {_testFood1.MenuNumber})).ToVersion(4)
                ;
        }

        protected override MarkFoodServedCommand When()
        {
            return new MarkFoodServedCommand(_tabAggregate, new List<int> {_testFood1.MenuNumber});
        }

        [Fact]
        public void Then_food_not_prepared_exception_should_be_throws()
        {
            Assert.Equal(typeof (FoodNotPrepared), CaughtException.GetType());
        }
    }
}