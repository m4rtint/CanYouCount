using CanYouCount.ObjectPooling;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CanYouCount
{
	public class TileRenderer : BasicUnityPoolable, IPointerDownHandler
	{
		[SerializeField]
		private TMP_Text _tileValueText = null;

		[SerializeField]
		private SpriteRenderer _tileBorder = null;
		[SerializeField]
		private SpriteRenderer _tileBackground = null;

		private Tile _tile;
		private Game _game;

		public void SetTile(Game game, Tile tile)
		{
			_tile = tile;
			_game = game;

			if (!tile.TileValue.HasValue)
			{
				// Tile is Blank Tile
				SetEnabled(false);
			}
			else
			{
				// Tile has value
				SetEnabled(true);

				_tileValueText.text = tile.TileValue.Value.ToString();
			}
		}

		private void SetEnabled(bool state)
		{
			_tileBackground.enabled = state;
			_tileBorder.enabled = state;
			_tileValueText.enabled = state;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			_tileBackground.color = Color.green;
			_game.OnTileTapped(_tile);
		}
	}
}
