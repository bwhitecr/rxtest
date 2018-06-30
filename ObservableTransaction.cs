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
        private TransactionState currentState;
        private readonly Task generator;

        readonly ReplaySubject<string> subject;

        public ObservableTransaction(IObservable<char> inputObservable)
        {
            currentState = TransactionState.Idle;

            subject = new ReplaySubject<string>();
            subject.OnNext("Initialised.");

            generator = Task.Run(() =>
            {
                decimal currentVolume = 0M;

                while (true)
                {
                    var cs = currentState;
                    var s = cs.ToString();

                    if (cs == TransactionState.Filling)
                    {
                        currentVolume += 0.005M;
                        s = $"Volume: {currentVolume}L";
                    }
                    else if (cs == TransactionState.Ended)
                    {
                        break;
                    }
                    subject.OnNext(s);
                    Task.Delay(10).Wait();
                }

                subject.OnCompleted();
            });

            inputObservable.Subscribe((c) =>
                {

                });


        }

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

            var timeObservable = Observable.Interval(TimeSpan.FromMilliseconds(50)).Select(_ => currentState);

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

        public IObservable<string> DataStream { get => subject; }
    }
}