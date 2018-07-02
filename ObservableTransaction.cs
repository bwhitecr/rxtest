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
            IObservable<TransactionState> input,
            string grade,
            decimal unitPrice)
        {
            var timeObservable = Observable
                .Interval(TimeSpan.FromMilliseconds(50));

            var volume = 0M;

            return Observable.CombineLatest(input, timeObservable,
                (state, _) =>
                {
                    if (state == TransactionState.Filling)
                    {
                        volume += 0.05M;
                    }

                    return new { state, volume };
                })
                .DistinctUntilChanged()
                .TakeWhile(d => d.state != TransactionState.Ended)
                .Select(d => new TransactionDetail(d.state, grade, unitPrice, d.volume));
        }

    }
}