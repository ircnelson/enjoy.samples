using System;
using System.Collections.Generic;
using EnjoyCQRS.Commands;

namespace EnjoySample.Restaurant.Commands
{
    [Serializable]
    public class MarkFoodServedCommand : Command
    {
        public IEnumerable<int> MenuNumbers { get; }

        public MarkFoodServedCommand(Guid aggregateId, IEnumerable<int> menuNumbers) : base(aggregateId)
        {
            MenuNumbers = menuNumbers;
        }
    }
}