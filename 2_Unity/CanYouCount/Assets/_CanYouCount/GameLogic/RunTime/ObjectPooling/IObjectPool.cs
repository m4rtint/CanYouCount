namespace CanYouCount.ObjectPooling
{
	public interface IObjectPool<T> where T : IPoolable
	{
		T GetObjectFromPool();
		void ReturnObjectToPool(T poolable);
	}
}
