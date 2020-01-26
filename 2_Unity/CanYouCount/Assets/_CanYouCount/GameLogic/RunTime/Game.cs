using System;
using System.Collections.Generic;

namespace CanYouCount
{
    public class Game
	{

        private int _totalTileCount;

		private Tile _expectedTile;

		private Tile[] _visibleTiles;

        private Queue<Tile> _totalTiles;

        private IRandomService _randomValueGenerator;

        /// <summary>
        /// Occurs when on wrong tile tapped.
        /// </summary>
        public event Action<Tile> OnWrongTileTapped;

        /// <summary>
        /// Occurs when on correct tile tapped.
        /// </summary>
        public event Action<Tile, Tile> OnCorrectTileTapped;

        /// <summary>
        /// Gets the visible tile count.
        /// </summary>
        /// <value>The visible tile count.</value>
        public int VisibleTileCount => _visibleTiles.Length;

        /// <summary>
        /// Gets the visible tiles.
        /// </summary>
        /// <value>The visible tiles.</value>
        public Tile[] VisibleTiles => _visibleTiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CanYouCount.Game"/> class.
        /// </summary>
        /// <param name="visible">Visible.</param>
        /// <param name="totalNumber">Total number.</param>
        public Game(IRandomService service, int visible, int totalNumber)
        {
            _randomValueGenerator = service;
            Tile[] allTiles = SetupTotalTiles(totalNumber);
            _totalTiles = PlaceIntoQueue(allTiles);
            SetupVisibleTiles(visible);
        }

        /// <summary>
        /// On the tile tapped.
        /// </summary>
        /// <param name="tappedTile">Tapped tile.</param>
        public void OnTileTapped(Tile tappedTile)
        {
            if (tappedTile.TileValue == _expectedTile.TileValue)
            {
                OnCorrectTileTapped?.Invoke(tappedTile, tappedTile);
            }
            else
            {
                OnWrongTileTapped?.Invoke(tappedTile);
            }
        }

        private Queue<Tile> PlaceIntoQueue(Tile[] tiles)
        {
            _totalTiles = new Queue<Tile>();

            for (int i = 0; i < tiles.Length; i++)
            {
                _totalTiles.Enqueue(tiles[i]);
            }

            return _totalTiles;
        }

        private Tile[] SetupTotalTiles(int totalNumber) 
        {
            Tile[] allTiles = new Tile[totalNumber];
            for (int i = 0; i < totalNumber; i++)
            {
                allTiles[i] = new Tile(i + 1);
            }

            allTiles = RandomizeAllTiles(allTiles);

            return allTiles;
        }

        private void SetupVisibleTiles(int numberOfVisibleTiles)
        {
            _visibleTiles = new Tile[numberOfVisibleTiles];
            for (int i = 0; i < numberOfVisibleTiles; i++)
            {
                _visibleTiles[i] = _totalTiles.Dequeue();
            }
        }

        private Tile[] RandomizeAllTiles(Tile[] tiles)
        {
            int length = tiles.Length - 1;
            for (int i = length; i > 0; i--)
            {
                int randomInt = _randomValueGenerator.RandInt(0, i);
                Tile randomTile = tiles[randomInt];
                tiles[randomInt] = tiles[i];
                tiles[i] = randomTile;
            }

            return tiles;
        }
    }
}