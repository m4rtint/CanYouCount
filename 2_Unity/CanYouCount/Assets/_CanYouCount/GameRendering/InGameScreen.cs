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

		public override void ShowScreen(bool isInstant = false)
		{
			base.ShowScreen(isInstant);

			_game = _applicationManager.Game;

			// Subscribe to game events
			if (_game != null)
			{
				_game.OnCorrectTileTapped += OnCorrectTileTapped;
			}
		}

		public override void UpdateScreen(float deltaTime)
		{
			SetTimeUI(_game?.Timer ?? 0);
		}

		public override void HideScreen(bool isInstant = false)
		{
			// Unsubscribe from game events
			if (_game != null)
			{
				_game.OnCorrectTileTapped += OnCorrectTileTapped;
			}

			SetNextUI(1);
			SetTimeUI(0);
			base.HideScreen(true);
		}

		private void SetTimeUI(float time)
		{
			_timerText.text = string.Format(GameUIContent.TwoDecimalPoint, time);
		}

		private void SetNextUI(int value)
		{
			_nextTileText.text = value.ToString();
		}

		private void OnCorrectTileTapped(Tile tappedTile, Tile swappedTile)
		{
			SetNextUI(_game?.ExpectedValue ?? 0);
		}
	}
}
