using System;
using UnityEngine;

namespace CanYouCount
{
    public enum AppStates
    {
        MainMenu,
        Pregame,
        Ingame,
        GameOver,
    }

    public class ApplicationManager : MonoBehaviour
    {
        [Header("Object References")]
        [SerializeField]
        private GameRenderer _gameRenderer = null;
        [SerializeField]
        private CYC_UIManager _uiManager = null;
        [SerializeField]
        private UIManager _userInterfaceManager = null;

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

        public void GotoState(AppStates newState)
        {
            if (AppState == newState)
            {
                return;
            }

            AppState = newState;
            OnAppStateChanged?.Invoke(AppState);
        }

        private void OnEnable()
        {
            try
            {
                // Intialize Services
                _randomService = new SeededRandomService();

                // Initialize Renderer
                _gameRenderer.Initialize();

                // Initialize UI
                _uiManager.Initialize(this);

                GotoState(AppStates.MainMenu);
                StartNewGame();
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
                        _userInterfaceManager.StartCountDown();
                        break;
                    case AppStates.Ingame:
                        _game.UpdateGame(deltaTime);
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
                _userInterfaceManager.CleanUp();
            }
            catch (Exception ex)
            {
                PanicHelper.Panic(ex);
            }
        }

        private void StartNewGame()
        {
            if (_game != null)
            {
                CleanupGame();
            }

            _game = new Game(_randomService, _visibleTileCount, _totalTileCount, TimeSpan.FromSeconds(_maxGameTimeInSeconds));
            _game.OnGameOver += HandleGameOver;

            // Create the renderers
            _gameRenderer.SetGame(_game);

            // Initialize User Interface
            _userInterfaceManager.Initialize(_game);
            _game.OnGameOver += _userInterfaceManager.StartGameOver;

            // Change state to pregame
            GotoState(AppStates.Pregame);
        }

        private void CleanupGame()
        {
            if (_game == null)
            {
                return;
            }

            // Unsubscribe from events
            _game.OnGameOver -= HandleGameOver;
            _game.OnGameOver -= _userInterfaceManager.StartGameOver;

            _game = null;
        }

        private void HandleGameOver(GameOverInfo gameOverInfo)
        {
            Debug.Log($"GameOver [{(gameOverInfo.IsSuccess ? "SUCCESS" : "FAIL")}]: {gameOverInfo.Time} seconds");
            StartNewGame();
        }
    }
}
