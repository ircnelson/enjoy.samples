using System;
using System.Linq;
using EnjoyCQRS.TestFramework;
using EnjoySample.Restaurant.Commands;
using EnjoySample.Restaurant.Domain;
using EnjoySample.Restaurant.Domain.Tab;
using FluentAssertions;
using Xunit;

namespace EnjoySample.Restaurant.Tests.Commands
{
    public class CanOpenANewTab : CommandTestFixture<OpenTabCommand, OpenTabCommandHandler, TabAggregate>
    {
        protected override OpenTabCommand When()
        {
            return new OpenTabCommand(Guid.NewGuid(), 1, "Nelson");
        }

        [Fact]
        public void Then_opened_tab_event_will_be_published()
        {
            PublishedEvents.Last().Should().BeAssignableTo<TabOpenedEvent>();
        }

        [Fact]
        public void Then_the_published_event_will_contain_the_number_of_table()
        {
            PublishedEvents.Last().As<TabOpenedEvent>().TableNumber.Should().Be(1);
        }

        [Fact]
        public void Then_the_published_event_will_contain_the_waiter_name()
        {
            PublishedEvents.Last().As<TabOpenedEvent>().Waiter.Should().Be("Nelson");
        }
    }
}