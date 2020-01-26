using TMPro;
using UnityEngine;

namespace CanYouCount
{
	public class InGameScreen : BaseScreen
	{
		[Header("Text To Update")]
		[SerializeField]
		private TMP_Text _timerText = null;
		[SerializeField]
		private TMP_Text _nextTileText = null;

		private Game _game = null;
		private GameCenterTextDisplay _overworldScreenRenderer = null;

		/// <summary>
		/// Initialize the specified game.
		/// </summary>
		/// <param name="game">Game.</param>
		public void Initialize(Game game)
		{
			CleanUp();
			_game = game;
			_game.OnCorrectTileTapped += OnCorrectTileTapped;
		}

		/// <summary>
		/// Updates the user interface.
		/// </summary>
		public void UpdateUI()
		{
			_timerText.text = string.Format(GameUIContent.TwoDecimalPoint, _game?.Timer);
		}

		/// <summary>
		/// Cleans up.
		/// </summary>
		public void CleanUp()
		{
			if (_overworldScreenRenderer == null)
			{
				return;
			}

			SetNextUI(0);
			SetTimeUI(0);
			Destroy(_overworldScreenRenderer);
		}

        /// <summary>
        /// Starts the count down.
        /// </summary>
        public void StartCountDown()
        {
            _overworldScreenRenderer.StartCountDown();
        }

        /// <summary>
        /// Starts the game over.
        /// </summary>
        public void StartGameOver(GameOverInfo info)
        {
            _overworldScreenRenderer.StartGameOver();
        }

		private void SetTimeUI(float time)
		{
			_timerText.text = string.Format(GameUIContent.TwoDecimalPoint, time);
		}

		private void SetNextUI(int value)
		{
			_nextTileText.text = value.ToString();
		}

		private void OnCorrectTileTapped(Tile originalTile, Tile arg2)
		{
			SetNextUI(originalTile.TileValue ?? 0 + 1);
		}
	}
}
