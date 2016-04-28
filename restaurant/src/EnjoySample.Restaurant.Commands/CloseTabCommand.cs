using System;
using EnjoyCQRS.Commands;

namespace EnjoySample.Restaurant.Commands
{
    [Serializable]
    public class CloseTabCommand : Command
    {
        public decimal AmountPaid { get; }

        public CloseTabCommand(Guid aggregateId, decimal amountPaid) : base(aggregateId)
        {
            AmountPaid = amountPaid;
        }
    }
}