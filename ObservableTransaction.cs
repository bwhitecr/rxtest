using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace rxtest
{
    public class ObservableTransaction
    {
        public static IObservable<TransactionDetail> MakeTransactionObservable(
            IObservable<char> input,
            string grade,
            decimal unitPrice)
        {
            var timeObservable = Observable
                .Interval(TimeSpan.FromMilliseconds(50));

            var volume = 0M;

            return Observable.CombineLatest(input, timeObservable,
                (c, time) =>
                {
                    var state = CharacterToTransactionState(c);
                    if (state == TransactionState.Filling)
                    {
                        volume += 0.05M;
                        return new TransactionDetail(state, grade, unitPrice, volume);
                    }
                    else
                        return new TransactionDetail(state, grade, unitPrice, volume);
                }).TakeWhile(d => d.State != TransactionState.Ended);
        }

        private static TransactionState CharacterToTransactionState(char c)
        {
            switch (c)
            {
                case '1':
                    return TransactionState.Idle;
                case '2':
                    return TransactionState.Calling;
                case '3':
                    return TransactionState.Acknowledged;
                case '4':
                    return TransactionState.Filling;
                case '5':
                    return TransactionState.Finalised;
                default:
                    return TransactionState.Ended;
            }
        }
    }
}