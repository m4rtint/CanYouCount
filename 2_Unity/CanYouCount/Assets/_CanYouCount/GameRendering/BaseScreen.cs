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
            transform.localPosition = Vector3.zero;
		}

		public virtual void HideScreen(bool isInstant = false)
		{
			ScaleAnimation(false);
		}

		public virtual void ShowScreen(bool isInstant = false)
		{
			gameObject.SetActive(true);
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
