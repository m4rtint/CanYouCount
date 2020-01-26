using System;
using System.Collections.Generic;
using UnityEditor;

namespace CanYouCount
{
	public class Game
	{

		private int _totalTileCount;

		private int _expectedValue;

		private Tile[] _visibleTiles;

		private Queue<Tile> _totalTiles;

        private float _timer;

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

        public float Timer => _timer;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CanYouCount.Game"/> class.
		/// </summary>
		/// <param name="visible">Visible.</param>
		/// <param name="totalNumber">Total number.</param>
		public Game(IRandomService service, int visible, int totalNumber)
		{

			CheckBoardArguments(totalNumber, visible);


			_randomValueGenerator = service;
			Tile[] allTiles = SetupTotalTiles(totalNumber, visible);
			_expectedValue = 1;
			_totalTiles = PlaceIntoQueue(allTiles);
			SetupVisibleTiles(visible);
		}

		/// <summary>
		/// On the tile tapped.
		/// </summary>
		/// <param name="tappedTile">Tapped tile.</param>
		public void OnTileTapped(Tile tappedTile)
		{
			if (tappedTile.TileValue == _expectedValue)
			{
				CorrectTileTapped(tappedTile);
				OnCorrectTileTapped?.Invoke(tappedTile, tappedTile);
			}
			else
			{
				OnWrongTileTapped?.Invoke(tappedTile);
			}
		}

        /// <summary>
        /// Updates the game.
        /// </summary>
        /// <param name="deltaTime">Delta time.</param>
        public void UpdateGame(float deltaTime)
        {
            _timer += deltaTime;
        }

        private void CheckBoardArguments(int totalSize, int visibleSize)
		{
			if (totalSize <= 0)
			{
				throw new System.Exception("Total Size too small < 0 - Get your shit together");
			}

			if (visibleSize <= 0)
			{
				throw new System.Exception("Visible Size too small < 0 - Get it together");
			}

			if (visibleSize > totalSize)
			{
				throw new System.Exception("Visible Size > Total Size - Come on now");
			}
		}

		private void CorrectTileTapped(Tile tappedTile)
		{
			_expectedValue++;

			for (int i = 0; i < _visibleTiles.Length; i++)
			{
				if (_visibleTiles[i].Equals(tappedTile))
				{
					_visibleTiles[i] = GetNextTileValue();
				}
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

		private Tile[] SetupTotalTiles(int totalNumber, int visible)
		{
			Tile[] allTiles = new Tile[totalNumber];
			for (int i = 0; i < totalNumber; i++)
			{
				allTiles[i] = new Tile(i + 1);
			}

			allTiles = RandomizeAllTiles(allTiles, visible);

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

		private Tile[] RandomizeAllTiles(Tile[] tiles, int visible)
		{
			int amount = AmountOfTimesToShuffle(tiles.Length, visible);
			int lowerLimit = 0;
			int upperLimit = 0;

			for (int i = 0; i < amount; i++)
			{
				lowerLimit = i * visible;
				upperLimit = Math.Min((i + 1) * visible, tiles.Length - 1);
				RandomizeTiles(tiles, lowerLimit, upperLimit);
			}

			return tiles;
		}

		private int AmountOfTimesToShuffle(int totalTiles, int visible)
		{
			int amount = totalTiles / visible;
			if (totalTiles % visible != 0)
			{
				amount++;
			}

			return amount;
		}

		private void RandomizeTiles(Tile[] tiles, int from, int to)
		{
			for (int i = to; i > from; i--)
			{
				int randomInt = _randomValueGenerator.RandInt(from, i);

				Tile randomTile = tiles[randomInt];
				tiles[randomInt] = tiles[from];
				tiles[from] = randomTile;
			}
		}

		private Tile GetNextTileValue()
		{
			if (_totalTiles.Count > 0)
			{
				return _totalTiles.Dequeue();
			}
			else
			{
				return new Tile();
			}
		}
	}
}