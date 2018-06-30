namespace rxtest
{
    public enum TransactionState
    {
        Idle,
        Calling,
        Acknowledged,
        Filling,
        Stopped,
        Ended,
        Finalised
    }
}