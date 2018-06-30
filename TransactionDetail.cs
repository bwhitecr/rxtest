using System;

namespace rxtest
{
    public class TransactionDetail
    {
        public TransactionDetail(
            TransactionState state, 
            string grade,
            decimal unitPrice,
            decimal volume)
        {
            Timestamp = DateTimeOffset.Now;
            State = state;
            Volume = volume;
            Grade = grade;
            UnitPrice = unitPrice;
        }
        
        public DateTimeOffset Timestamp { get; }
        public TransactionState State { get; }
        public string Grade { get; }
        public decimal UnitPrice { get; }
        public decimal Volume { get; }
        public decimal TotalPrice { get => UnitPrice * Volume; }

        public override string ToString() 
            => $"{Timestamp} - {State} - {Volume}L @ {UnitPrice}/L = {TotalPrice:c}";
    }
}