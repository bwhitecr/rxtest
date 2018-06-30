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
        public static IObservable<TransactionDetail> MakeObservable(IObservable<char> input)
        {
            var stateObservable = input.Select(c =>
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
            });

            var timeObservable = Observable
                .Interval(TimeSpan.FromMilliseconds(50));

            var volume = 0M;

            return Observable.CombineLatest(stateObservable, timeObservable,
                (state, time) =>
                {
                    if (state == TransactionState.Filling)
                    {
                        volume += 0.05M;
                        return new TransactionDetail(state, volume);
                    }
                    else
                        return new TransactionDetail(state, volume);
                }).TakeWhile(d => d.State != TransactionState.Ended);
        }
    }
}