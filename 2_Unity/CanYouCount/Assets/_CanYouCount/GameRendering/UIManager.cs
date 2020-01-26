using System;
using UnityEngine;

namespace CanYouCount
{
	public class UIManager : MonoBehaviour
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
		private BaseScreen _currentScreen;

		public void Initialize(ApplicationManager appManager)
		{
			_appManager = appManager;
			_appManager.OnAppStateChanged += HandleAppStateChanged;

			InitializeAllScreens();
			HideAllScreens();
		}

		public void Cleanup()
		{
			_appManager.OnAppStateChanged -= HandleAppStateChanged;
		}

		public void UpdateUI(float deltaTime)
		{
			_currentScreen?.UpdateScreen(deltaTime);
		}

		private void HandleAppStateChanged(AppStates newState)
		{
			switch (newState)
			{
				case AppStates.MainMenu:
					ChangeCurrentScreen(_mainMenuScreen);
					break;

				case AppStates.Pregame:
					ChangeCurrentScreen(_inGameScreen);

					SetGameCenterTextVisibility(true);
					_mainTextRenderer.OnCountDownComplete += HandleCountdownComplete;
					_mainTextRenderer.StartCountDown();

					break;

				case AppStates.Ingame:
					ChangeCurrentScreen(_inGameScreen);
					break;

				case AppStates.GameOverAnimation:
					SetGameCenterTextVisibility(true);
					_mainTextRenderer.OnGameOverComplete += HandleGameOverComplete;
					_mainTextRenderer.StartGameOver();

					break;

				case AppStates.GameOver:
					ChangeCurrentScreen(_gameOverScreen);

					break;
				default:
					break;
			}
		}

		private void HandleGameOverComplete()
		{
			_mainTextRenderer.OnGameOverComplete -= HandleGameOverComplete;
			_appManager.ChangeState(AppStates.GameOver);
		}

		private void HandleCountdownComplete()
		{
			_mainTextRenderer.OnCountDownComplete -= HandleCountdownComplete;
			_appManager.ChangeState(AppStates.Ingame);
		}

		private void InitializeAllScreens()
		{
			_mainMenuScreen?.InitializeScreen(_appManager);
			_inGameScreen?.InitializeScreen(_appManager);
			_gameOverScreen?.InitializeScreen(_appManager);

			_mainTextRenderer.transform.localPosition = Vector3.zero;
		}

		private void HideAllScreens(bool isInstant = false)
		{
			_mainMenuScreen?.HideScreen(isInstant);
			_inGameScreen?.HideScreen(isInstant);
			_gameOverScreen?.HideScreen(isInstant);

			SetGameCenterTextVisibility(false);
		}

		private void SetGameCenterTextVisibility(bool visible)
		{
			_mainTextRenderer.gameObject.SetActive(visible);
		}

		private void ChangeCurrentScreen(BaseScreen newScreen)
		{
			if (_currentScreen == newScreen)
			{
				return;
			}

			// Hide old screen
			_currentScreen?.HideScreen();

			// Show new screen
			_currentScreen = newScreen;
			_currentScreen?.ShowScreen();
		}
	}
}
