namespace CanYouCount.ObjectPooling
{
	public interface IPoolable
	{
		void Activate();
		void Deactivate();
	}
}
