using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CanYouCount
{
	public abstract class BaseScreen : MonoBehaviour, IScreen
	{
		[SerializeField]
		private float _animationTime = 0.5f;

		protected ApplicationManager _applicationManager;

		public virtual void InitializeScreen(ApplicationManager appManager)
		{
			_applicationManager = appManager;
            transform.position = Vector3.zero;
		}

		public virtual void HideScreen()
		{
			ScaleAnimation(false);
		}

		public virtual void ShowScreen()
		{
			ScaleAnimation(true);
		}

		public virtual void UpdateScreen(float deltaTime)
		{

		}

		private void ScaleAnimation(bool open)
		{
			Vector3 scale = open ? Vector3.one : Vector3.zero;
			transform.LeanScale(scale, _animationTime)
				  .setEase(LeanTweenType.easeOutBack)
				  .setOnComplete(() =>
				  {
					  gameObject.SetActive(open);
				  });
		}
	}
}
