﻿using System;
using UnityEngine;

namespace CanYouCount
{
	public enum AppStates
	{
		Initial,
		MainMenu,
		Pregame,
		Ingame,
		GameOverAnimation,
		GameOver,
	}

	public class ApplicationManager : MonoBehaviour
	{
		[Header("Object References")]
		[SerializeField]
		private GameRenderer _gameRenderer = null;
		[SerializeField]
		private UIManager _uiManager = null;
		[SerializeField]
		private AudioManager _audioManager = null;

		[Header("Game Variables")]
		[SerializeField]
		private int _visibleTileCount = 25;

		[SerializeField]
		private int _totalTileCount = 50;

		[SerializeField]
		private float _maxGameTimeInSeconds = 300;

		public AppStates AppState { get; private set; }
		public event Action<AppStates> OnAppStateChanged;

		private IRandomService _randomService;
		private Game _game;

		public Game Game => _game;
		public AudioManager AudioManager => _audioManager;

		/// <summary>
		/// Changes the application's state to the provided state
		/// </summary>
		/// <param name="newState">The newly provided state</param>
		public void ChangeState(AppStates newState)
		{
			if (AppState == newState)
			{
				return;
			}

			Debug.Log($"AppState Change: {AppState} => {newState}", this);

			AppState = newState;
			OnAppStateChanged?.Invoke(AppState);
		}

		/// <summary>
		/// Starts a new game and transitions to <see cref="AppStates.Pregame"/>
		/// </summary>
		public void StartNewGame()
		{
			if (_game != null)
			{
				CleanupGame();
			}

			_game = new Game(_randomService, _visibleTileCount, _totalTileCount, TimeSpan.FromSeconds(_maxGameTimeInSeconds));
			_game.OnGameOver += HandleGameOver;

			// Create the renderers
			_gameRenderer.SetGame(_game);

			// Change state to pregame
			ChangeState(AppStates.Pregame);
		}

		private void OnEnable()
		{
			try
			{
				// Intialize Services
				_randomService = new SeededRandomService();

				// Initialize Renderer
				_gameRenderer.Initialize(this);

				// Initialize UI
				_uiManager.Initialize(this);

				ChangeState(AppStates.MainMenu);
			}
			catch (Exception ex)
			{
				PanicHelper.Panic(ex);
			}
		}

		private void Update()
		{
			var deltaTime = Time.deltaTime;
			try
			{
				switch (AppState)
				{
					case AppStates.MainMenu:
						break;
					case AppStates.Pregame:
						break;
					case AppStates.Ingame:
						_game.UpdateGame(deltaTime);
						break;
					case AppStates.GameOverAnimation:
						break;
					case AppStates.GameOver:
						break;
					default:
						break;
				}

				_uiManager.UpdateUI(deltaTime);
			}
			catch (Exception ex)
			{
				PanicHelper.Panic(ex);
			}
		}

		private void OnDisable()
		{
			try
			{
				_gameRenderer.Cleanup();
				_uiManager.Cleanup();
			}
			catch (Exception ex)
			{
				PanicHelper.Panic(ex);
			}
		}

		private void CleanupGame()
		{
			if (_game == null)
			{
				return;
			}

			// Unsubscribe from events
			_game.OnGameOver -= HandleGameOver;

			_game = null;
		}

		private void HandleGameOver(GameOverInfo gameOverInfo)
		{
			if (gameOverInfo.IsSuccess)
			{
				AudioManager.PlayWinState();
			}
			else
			{
				AudioManager.PlayLoseState();
			}

			Debug.Log($"GameOver [{(gameOverInfo.IsSuccess ? "SUCCESS" : "FAIL")}]: {gameOverInfo.Time} seconds");
			ChangeState(AppStates.GameOverAnimation);
		}
	}
}
