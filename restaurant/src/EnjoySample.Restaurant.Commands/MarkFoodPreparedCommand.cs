using System;
using System.Collections.Generic;
using EnjoyCQRS.Commands;

namespace EnjoySample.Restaurant.Commands
{
    [Serializable]
    public class MarkFoodPreparedCommand : Command
    {
        public IEnumerable<int> MenuNumbers { get; }

        public MarkFoodPreparedCommand(Guid aggregateId, IEnumerable<int> menuNumbers) : base(aggregateId)
        {
            MenuNumbers = menuNumbers;
        }
    }
}