using System;
using System.Collections.Generic;

namespace CanYouCount
{
	public struct GameOverInfo
	{
		public bool IsSuccess;
		public float Time;
	}

	public class Game
	{

		private int _totalTileCount;

		private int _expectedValue;

		private TimeSpan _maxGameTime;

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
		/// Occurs when the game is over (either all tiles are tapped, or time is up)
		/// </summary>
		public event Action<GameOverInfo> OnGameOver;

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
		/// True if the game is over
		/// </summary>
		public bool IsGameOver => _isGameOver;

		/// <summary>
		/// The value the game is expecting for the next <see cref="Tile"/>
		/// </summary>
		public int ExpectedValue => _expectedValue;

		public float Timer => _timer;
		private bool _isGameOver;

		private float _lastCorrectTapTime;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CanYouCount.Game"/> class.
		/// </summary>
		/// <param name="rngService">The random number service</param>
		/// <param name="visible">Visible.</param>
		/// <param name="totalNumber">Total number.</param>
		/// <param name="maxGameTime">The maximum time the game can run for, before failing</param>
		public Game(IRandomService rngService, int visible, int totalNumber, TimeSpan maxGameTime)
		{

			CheckBoardArguments(totalNumber, visible);
			if (maxGameTime.TotalSeconds <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(maxGameTime));
			}

			_timer = 0;
			_isGameOver = false;

			_maxGameTime = maxGameTime;
			_randomValueGenerator = rngService;
			Tile[] allTiles = SetupTotalTiles(totalNumber, visible);
			_expectedValue = 1;
			_totalTileCount = totalNumber;
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
				_expectedValue = Math.Min(_expectedValue + 1, _totalTileCount);

				CheckForGameOver();

				Tile swapTile = CorrectTileTapped(tappedTile);
				OnCorrectTileTapped?.Invoke(tappedTile, swapTile);
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
			if (_isGameOver)
			{
				return;
			}

			_timer += deltaTime;
			CheckForGameOver();
		}

		private bool AreAllVisibleTilesBlank()
		{
			for (int i = 0; i < _visibleTiles.Length; i++)
			{
				var visibleTile = _visibleTiles[i];
				if (visibleTile.TileValue.HasValue)
				{
					return false;
				}
			}

			return true;
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

		private Tile CorrectTileTapped(Tile tappedTile)
		{
			for (int i = 0; i < _visibleTiles.Length; i++)
			{
				if (_visibleTiles[i].Equals(tappedTile))
				{
					_visibleTiles[i] = GetNextTileValue();
					return _visibleTiles[i];
				}
			}

			return Tile.BlankTile;
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
			int timesToSwap = to - from;
			for (int i = 0; i < timesToSwap; i++)
			{
				int i1 = _randomValueGenerator.RandInt(from, to);
				int i2 = _randomValueGenerator.RandInt(from, to);

				Tile randomTile = tiles[i1];
				tiles[i1] = tiles[i2];
				tiles[i2] = randomTile;
			}

			//for (int i = to; i > from; i--)
			//{
			//	int randomInt = _randomValueGenerator.RandInt(from, i);

			//	Tile randomTile = tiles[randomInt];
			//	tiles[randomInt] = tiles[from];
			//	tiles[from] = randomTile;
			//}
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

		private void CheckForGameOver()
		{
			// Check for GameOver: Out of Time
			if (_timer >= _maxGameTime.TotalSeconds)
			{
				_isGameOver = true;
				OnGameOver?.Invoke(new GameOverInfo()
				{
					IsSuccess = false,
					Time = (float)_maxGameTime.TotalSeconds
				});
			}

			// Check for win
			if (_totalTiles.Count < 1 && AreAllVisibleTilesBlank())
			{
				// GameOver: Success
				_isGameOver = true;
				OnGameOver?.Invoke(new GameOverInfo()
				{
					IsSuccess = true,
					Time = _timer
				});
			}
		}
	}
}