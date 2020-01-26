using TMPro;
using UnityEngine;

namespace CanYouCount
{
	public class TileRenderer : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _tileValueText = null;

		[SerializeField]
		private SpriteRenderer _tileBackground = null;

		public void SetTile(Tile tile)
		{
			if (!tile.TileValue.HasValue)
			{
				// Tile is Blank Tile
				_tileBackground.enabled = false;
				_tileValueText.enabled = false;
			}
			else
			{
				// Tile has value
				_tileBackground.enabled = true;
				_tileValueText.enabled = true;

				_tileValueText.text = tile.TileValue.Value.ToString();
			}
		}
	}
}
