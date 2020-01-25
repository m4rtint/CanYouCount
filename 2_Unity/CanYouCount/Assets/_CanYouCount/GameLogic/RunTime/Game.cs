namespace CanYouCount
{
    public class Game
	{
		private int _visibleTileCount;
		private int _totalTileCount;

		private Tile _expectedTile;

		private Tile[] _visibleTiles;

        public int VisibleTileCount => _visibleTileCount;
        public Tile[] VisibleTiles => _visibleTiles;

        private IRandomService _randomValueGenerator;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:CanYouCount.Game"/> class.
        /// </summary>
        /// <param name="visible">Visible.</param>
        /// <param name="totalNumber">Total number.</param>
        public Game(IRandomService service, int visible, int totalNumber)
        {
            
        }

        private int NextNumber()
        {
            return _randomValueGenerator.RandInt(1, _totalTileCount);
        }



    }

    public class Tile
    {
        public int MyNumber;
    }
}