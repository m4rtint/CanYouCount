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

		public void InitializeScreen(ApplicationManager appManager)
		{
			_applicationManager = appManager;
		}

		public virtual void HideScreen()
		{
			transform.LeanScale(Vector3.one * 2, _animationTime)
				  .setEase(LeanTweenType.easeOutBack)
				  .setOnComplete(() =>
				  {
					  gameObject.SetActive(false);
				  });
		}

		public virtual void ShowScreen()
		{
			gameObject.SetActive(true);
			transform.localScale = Vector3.zero;
			ScaleAnimation(Vector3.one);
		}

		public virtual void UpdateScreen(float deltaTime)
		{

		}

		private void ScaleAnimation(Vector3 scale)
		{
			transform.LeanScale(scale, _animationTime)
				  .setEase(LeanTweenType.easeOutBack);
		}
	}
}
