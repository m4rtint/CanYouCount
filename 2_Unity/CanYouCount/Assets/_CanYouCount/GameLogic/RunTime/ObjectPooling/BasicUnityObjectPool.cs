using System;
using System.Collections.Generic;
using UnityEngine;

namespace CanYouCount.ObjectPooling
{
	public class BasicUnityObjectPool<T> : IObjectPool<T> where T : Component, IPoolable
	{
		protected Transform _poolableContainer;

		protected Func<T> _allocationFunction;
		protected Queue<T> _poolables = new Queue<T>();

		public BasicUnityObjectPool(Func<T> allocationFunction, Transform parent = null)
		{
			_allocationFunction = allocationFunction ??
				throw new ArgumentNullException(nameof(allocationFunction));

			_poolableContainer = new GameObject($"{typeof(T).ToString()} Object Pool").transform;
			if (parent != null)
				_poolableContainer.parent = parent;
		}

		public void PreallocateObjects(int count)
		{
			for (int i = 0; i < count; i++)
				ReturnObjectToPool(_allocationFunction.Invoke());
		}

		public T GetObjectFromPool()
		{
			if (_poolables.Count < 1)
			{
				PreallocateObjects(1);
			}

			T poolable = _poolables.Dequeue();
			poolable.Activate();

			return poolable;
		}

		public T GetObjectFromPool(Transform parent)
		{
			T poolable = GetObjectFromPool();

			poolable.transform.SetParent(parent);
			poolable.transform.localPosition = Vector3.zero;

			return poolable;
		}

		public T GetObjectFromPool(Transform parent, Vector3 localPosition)
		{
			T poolable = GetObjectFromPool();

			poolable.transform.SetParent(parent);
			poolable.transform.localPosition = localPosition;

			return poolable;
		}

		public void ReturnObjectToPool(T poolable)
		{
			poolable.transform.SetParent(_poolableContainer);

			// Deactivate the poolable
			poolable.Deactivate();

			// Add it to the list
			_poolables.Enqueue(poolable);
		}
	}
}
