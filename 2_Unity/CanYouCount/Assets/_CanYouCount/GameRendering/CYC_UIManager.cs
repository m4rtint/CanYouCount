using UnityEngine;

namespace CanYouCount
{
	public class CYC_UIManager : MonoBehaviour
	{
		[Header("UI Screens")]
		[SerializeField]
		private IScreen _mainMenuScreen = null;
		[SerializeField]
		private IScreen _pregameScreen = null;
		[SerializeField]
		private IScreen _inGameScreen = null;
		[SerializeField]
		private IScreen _gameOverScreen = null;

		private ApplicationManager _appManager;

		public void Initialize(ApplicationManager appManager)
		{
			_appManager = appManager;
			_appManager.OnAppStateChanged += HandleAppStateChanged;
		}

		public void Cleanup()
		{
			_appManager.OnAppStateChanged -= HandleAppStateChanged;
		}

		private void HandleAppStateChanged(AppStates obj)
		{
		}

		public void UpdateUI(float deltaTime)
		{

		}
	}
}
