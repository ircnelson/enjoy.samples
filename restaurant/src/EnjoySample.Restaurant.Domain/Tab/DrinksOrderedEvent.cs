﻿using System;
using EnjoyCQRS.Events;

namespace EnjoySample.Restaurant.Domain.Tab
{
    [Serializable]
    public class DrinksOrderedEvent : DomainEvent
    {
        public string Description { get; }
        public int MenuNumber { get; }
        public decimal Price { get; }
        public string Status { get; }

        public DrinksOrderedEvent(Guid aggregateId, string description, int menuNumber, decimal price, string status) : base(aggregateId)
        {
            Description = description;
            MenuNumber = menuNumber;
            Price = price;
            Status = status;
        }
    }
}