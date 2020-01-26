using UnityEngine;

namespace CanYouCount
{
	public class
		UIManager : MonoBehaviour
	{
		[Header("UI Screens")]
		[SerializeField]
		private BaseScreen _mainMenuScreen = null;
		[SerializeField]
		private BaseScreen _inGameScreen = null;
		[SerializeField]
		private BaseScreen _gameOverScreen = null;

		[SerializeField]
		private GameCenterTextDisplay _mainTextRenderer = null;

		private ApplicationManager _appManager;

		public void Initialize(ApplicationManager appManager)
		{
			_appManager = appManager;
			_appManager.OnAppStateChanged += HandleAppStateChanged;

			HideAllScreens();
		}

		public void Cleanup()
		{
			_appManager.OnAppStateChanged -= HandleAppStateChanged;
		}

		private void HandleAppStateChanged(AppStates newState)
		{
			switch (newState)
			{
				case AppStates.MainMenu:
					break;

				case AppStates.Pregame:
					_inGameScreen.ShowScreen();

					_mainTextRenderer.OnCountDownComplete += HandleCountdownComplete;
					_mainTextRenderer.StartCountDown();

					break;

				case AppStates.Ingame:
					_inGameScreen.ShowScreen();

					break;

				case AppStates.GameOverAnimation:
					// Play game over animation

					// Change state

					// Hide ingame

					break;

				case AppStates.GameOver:
					_gameOverScreen.ShowScreen();

					break;
				default:
					break;
			}
		}

		private void HandleCountdownComplete()
		{
			_appManager.ChangeState(AppStates.Ingame);
			_mainTextRenderer.OnCountDownComplete -= HandleCountdownComplete;
		}

		private void HideAllScreens()
		{
			// Hide all screens
			_mainMenuScreen?.HideScreen();
			_inGameScreen?.HideScreen();
			_gameOverScreen?.HideScreen();
		}

		public void UpdateUI(float deltaTime)
		{

		}
	}
}
