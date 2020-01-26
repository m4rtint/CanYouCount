using UnityEngine;

namespace CanYouCount.ObjectPooling
{
	public class BasicUnityPoolable : MonoBehaviour, IPoolable
	{
		public void Activate()
		{
			gameObject.SetActive(true);
		}

		public void Deactivate()
		{
			gameObject.SetActive(false);
		}
	}
}
