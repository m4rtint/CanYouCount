using System;

namespace CanYouCount.ObjectPooling
{
	public interface IObjectPool<T> : IDisposable where T : IPoolable
	{
		T GetObjectFromPool();
		void ReturnObjectToPool(T poolable);
	}
}
