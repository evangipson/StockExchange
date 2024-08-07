namespace StockExchange.Domain.Models.Events
{
	public interface IUnsubscriber<T>
	{
		public class Unsubscriber : IDisposable
		{
			private readonly List<IObserver<T>> _observers;
			private readonly IObserver<T> _observer;

			public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
			{
				_observers = observers;
				_observer = observer;
			}

			public void Dispose()
			{
				if (!(_observer == null)) _observers.Remove(_observer);
			}
		}
	}
}
