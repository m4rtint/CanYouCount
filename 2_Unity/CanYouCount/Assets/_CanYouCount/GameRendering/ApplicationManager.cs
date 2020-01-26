using System;
using UnityEngine;

namespace CanYouCount
{
	public class ApplicationManager : MonoBehaviour
	{
		[Header("Object References")]
		[SerializeField]
		private GameRenderer _gameRenderer = null;
		[SerializeField]
		private UIManager _userInterfaceManager = null;

		[Header("Game Variables")]
		[SerializeField]
		private int _visibleTileCount = 25;

		[SerializeField]
		private int _totalTileCount = 50;

		private IRandomService _randomService;
		public Game _game;

		private void OnEnable()
		{
			try
			{
				// Intialize Services
				_randomService = new SeededRandomService();

				// Initialize Renderers
				_gameRenderer.Initialize();

				StartNewGame();

				// Initialize User Interface
				_userInterfaceManager.Initialize(_game);
			}
			catch (Exception ex)
			{
				PanicHelper.Panic(ex);
			}
		}

		private void OnDisable()
		{
			_gameRenderer.Cleanup();
            _userInterfaceManager.CleanUp();
		}

		private void Update()
		{
			_game.UpdateGame(Time.deltaTime);
			_userInterfaceManager.UpdateUI();
		}

		private void StartNewGame()
		{
			_game = new Game(_randomService, _visibleTileCount, _totalTileCount);

			// Create the renderers
			_gameRenderer.SetGame(_game);
		}
	}
}
