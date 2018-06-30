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
        public static IObservable<string> MakeObservable(IObservable<char> input)
        {
            TransactionState currentState = TransactionState.Idle;

            var stateObservable = input.Select(c =>
            {
                switch (c)
                {
                    case '1':
                        currentState = TransactionState.Idle;
                        break;
                    case '2':
                        currentState =  TransactionState.Calling;
                        break;
                    case '3':
                        currentState =  TransactionState.Acknowledged;
                        break;
                    case '4':
                        currentState = TransactionState.Filling;
                        break;
                    case '5':
                        currentState = TransactionState.Ended;
                        break;
                }
                return currentState;
            });

            var timeObservable = Observable
                .Interval(TimeSpan.FromMilliseconds(50))
                .Select(_ => currentState);

            var volume = 0M;

            return Observable.Merge(stateObservable, timeObservable)
                .TakeWhile(state => state != TransactionState.Ended)
                .Select(state =>
                {
                    if (state == TransactionState.Filling)
                    {
                        volume += 0.05M;
                        return $"Filling - {volume} litres";
                    }
                    else
                        return state.ToString();
                });
        }
    }
}