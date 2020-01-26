using UnityEngine;

namespace CanYouCount
{
	public class ApplicationManager : MonoBehaviour
	{
		[Header("Game Variables")]
		[SerializeField]
		private int _visibleTileCount = 25;

		[SerializeField]
		private int _totalTileCount = 50;

		private IRandomService _randomService;
		private Game _game;

		private void OnEnable()
		{
			// Create the game
			_randomService = new SeededRandomService();
			_game = new Game(_randomService, _visibleTileCount, _totalTileCount);

			// Create the renderers
			// TODO: Create renderers
		}
	}
}
