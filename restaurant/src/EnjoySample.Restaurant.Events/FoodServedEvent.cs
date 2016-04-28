﻿using System;
using System.Collections.Generic;
using EnjoyCQRS.Events;

namespace EnjoySample.Restaurant.Events
{
    [Serializable]
    public class FoodServedEvent : DomainEvent
    {
        public IEnumerable<int> MenuNumbers { get; }

        public FoodServedEvent(Guid aggregateId, IEnumerable<int> menuNumbers) : base(aggregateId)
        {
            MenuNumbers = menuNumbers;
        }
    }
}