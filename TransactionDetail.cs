using System;

namespace rxtest
{
    public class TransactionDetail
    {
        public TransactionDetail(TransactionState state, decimal volume)
        {
            Timestamp = DateTimeOffset.Now;
            State = state;
            Volume = volume;
        }
        
        public DateTimeOffset Timestamp { get; }
        public TransactionState State { get; }
        public decimal Volume { get; }
    }
}