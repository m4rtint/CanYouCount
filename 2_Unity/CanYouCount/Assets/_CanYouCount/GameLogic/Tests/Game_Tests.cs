using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CanYouCount
{
	public class Game_Tests
	{
		// Create n (game) (n # visible, m total #)
		// We have n numbers loaded
		// The numbers are 1..n

		// Tile is activated
		// Nothing happens if not the expected tile
		// If expected tile
		// Remove that tile from visible tiles
		// Update expected tile to next tile
		// Expected tile gets replaced in visible tile array
		// If no more numbers, replace with blank
		// Event is broadcast for replacement/hide

		// Get value
		// 


		private const int TEST_GAME_NUM_VISIBLE = 5;
		private const int TEST_GAME_TOTAL_NUM = 25;

		[Test]
		public void Test_GameConstructor_Initial()
		{
			// Arrange/Act
			const int EXPECTED_NUM_VISIBLE = 5;
			const int EXPECTED_TOTAL_NUM = 25;

			var random = new SeededRandomService();
			var game = new Game(random, EXPECTED_NUM_VISIBLE, EXPECTED_TOTAL_NUM);

			// Assert
			Assert.AreEqual(EXPECTED_NUM_VISIBLE, game.VisibleTileCount,
				$"Expected visible tile count to be set to {EXPECTED_NUM_VISIBLE}");
			Assert.AreEqual(EXPECTED_NUM_VISIBLE, game.VisibleTiles.Length,
				$"Expected the number of visible tiles to be {EXPECTED_NUM_VISIBLE}");

			// Check to ensure the numbers visible are (1 to n)
			for (int i = 1; i <= EXPECTED_NUM_VISIBLE; i++)
			{
				// Try and get the desired tile
				Assert.DoesNotThrow(() =>
				{
					GetVisibleTileByValue(game, i);
				}, $"Expected to be able to find a visible tile with value [{i}], but could not");
			}
		}

		[Test]
		public void Test_GameConstructor_DifferentSizes()
		{
			var random = new SeededRandomService();

			// Create a bunch of games of different sizes
			for (int visibleSize = 0; visibleSize < 50; visibleSize++)
				for (int totalSize = 0; totalSize < 50; totalSize++)
				{
					if (totalSize <= 0)
					{
						Assert.Throws<Exception>(() => new Game(random, visibleSize, totalSize),
							"Should not be able to create a game with total size <= 0");
					}
					else if (visibleSize <= 0)
					{
						Assert.Throws<Exception>(() => new Game(random, visibleSize, totalSize),
							"Should not be able to create a game with more visible size <= 0");
					}
					else if (visibleSize > totalSize)
					{
						Assert.Throws<Exception>(() => new Game(random, visibleSize, totalSize),
							"Should not be able to create a game with more visible tiles than total tiles");
					}
					else
					{
						Assert.DoesNotThrow(() => new Game(random, visibleSize, totalSize),
							$"Should be able to create a Game with visible size [{visibleSize}] and total size [{totalSize}]");
					}
				}
		}

		[Test]
		public void Test_ActivateTile_FailsIfNotExpectedTile()
		{
			int wrongTileCount = 0;
			void HandleWrongTile(Tile wrongTile)
			{
				wrongTileCount++;
			}

			int swapTileCount = 0;
			void HandleSwapTile(Tile oldTile, Tile newTile)
			{
				swapTileCount++;
			}

			// Arrange
			var game = SetupGameForTesting();
			game.OnWrongTileTapped += HandleWrongTile;
			game.OnCorrectTileTapped += HandleSwapTile;

			// get a tile that isn't Tile 1
			var nonExpectedTile = FindTileByPredicate(game, tile => tile.TileValue != 1);

			// Act
			game.OnTileTapped(nonExpectedTile);

			// Assert
			Assert.AreEqual(1, wrongTileCount,
				$"Expected the {nameof(game.OnWrongTileTapped)} event to be called once, but was called {wrongTileCount} times");
			Assert.AreEqual(0, swapTileCount,
				$"Expected the {nameof(game.OnCorrectTileTapped)} event to not be called, but was called {swapTileCount} times");
		}

		[Test]
		public void Test_ActivateTile_SucceedsIfExpectedTile()
		{
			int wrongTileCount = 0;
			void HandleWrongTile(Tile wrongTile)
			{
				wrongTileCount++;
			}

			int swapTileCount = 0;
			void HandleSwapTile(Tile oldTile, Tile newTile)
			{
				swapTileCount++;
			}

            int swappedTileIndex = 1;

			// Arrange
			var game = SetupGameForTesting();
			game.OnWrongTileTapped += HandleWrongTile;
			game.OnCorrectTileTapped += HandleSwapTile;

			// get original tile values/ordering
			int expectedTileIndex = FindTileIndexByPredicate(game, tile => tile.TileValue == 1);
			Tile expectedTile = game.VisibleTiles[expectedTileIndex];
			var originalValueOrder = GetVisibleTileValues(game);

			// Act
			game.OnTileTapped(expectedTile);

			// Assert
			Assert.AreEqual(0, wrongTileCount,
				$"Expected the {nameof(game.OnWrongTileTapped)} event to not be called, but was called {wrongTileCount} times");
			Assert.AreEqual(swappedTileIndex, swapTileCount,
				$"Expected the {nameof(game.OnCorrectTileTapped)} event to be called onces, but was called {swapTileCount} times");


			var updatedValueOrder = GetVisibleTileValues(game);
			Assert.AreNotEqual(originalValueOrder, updatedValueOrder,
				$"Expected value arrays to differ by one value at index {expectedTileIndex}");

			// change value at index in the original array and compare again
			updatedValueOrder[expectedTileIndex] = swappedTileIndex;
			Assert.AreEqual(updatedValueOrder, originalValueOrder,
				$"Expected value arrays to not differ after updating value at index {expectedTileIndex}");
		}

		[Test]
		public void Test_Game_FullRun()
		{
			var game = SetupGameForTesting();

			for (int i = 1; i <= TEST_GAME_TOTAL_NUM; i++)
			{
				var tile = GetVisibleTileByValue(game, i);
				game.OnTileTapped(tile);
			}

			// TODO: Assert game is complete
		}

		[Test]
		public void Test_Game_ABlankTileAppeared()
		{
			var game = SetupGameForTesting();

			// Activate tiles until the next tile should be blank
			for (int i = 1; i <= (TEST_GAME_TOTAL_NUM - TEST_GAME_NUM_VISIBLE); i++)
			{
				var tile = GetVisibleTileByValue(game, i);
				game.OnTileTapped(tile);
			}

			// Next tile should be blank
			var nextTile = GetVisibleTileByValue(game, (TEST_GAME_TOTAL_NUM - TEST_GAME_NUM_VISIBLE) + 1);
			game.OnTileTapped(nextTile);

			var blankTile = GetVisibleTilesByValue(game, null);
			Assert.AreEqual(1, blankTile.Count,
				$"Expected 1 blank tile to appear after removing sufficient tiles");
		}

		#region Test Helpers

		private Game SetupGameForTesting()
		{
			var random = new SeededRandomService();
			var game = new Game(random, TEST_GAME_NUM_VISIBLE, TEST_GAME_TOTAL_NUM);

			return game;
		}

		private static Tile GetVisibleTileByValue(Game game, int? tileValue)
		{
			for (int i = 0; i < game.VisibleTiles.Length; i++)
			{
				if (game.VisibleTiles[i].TileValue == tileValue)
				{
					return game.VisibleTiles[i];
				}
			}

			throw new Exception($"Could not find visible tile by value: [{tileValue}]");
		}

		private static List<Tile> GetVisibleTilesByValue(Game game, int? tileVale)
		{
			var tiles = new List<Tile>();
			for (int i = 0; i < game.VisibleTiles.Length; i++)
			{
				if (game.VisibleTiles[i].TileValue == tileVale)
				{
					tiles.Add(game.VisibleTiles[i]);
				}
			}

			return tiles;
		}

		private static Tile FindTileByPredicate(Game game, Func<Tile, bool> predicate)
		{
			for (int i = 0; i < game.VisibleTiles.Length; i++)
			{
				if (predicate.Invoke(game.VisibleTiles[i]))
				{
					return game.VisibleTiles[i];
				}
			}

			throw new Exception("Could not find tile by predicate");
		}

		private static int?[] GetVisibleTileValues(Game game)
		{
			int?[] values = new int?[game.VisibleTileCount];
			for (int i = 0; i < game.VisibleTileCount; i++)
			{
				values[i] = game.VisibleTiles[i].TileValue;
			}
			return values;
		}

		private static int FindTileIndexByPredicate(Game game, Func<Tile, bool> predicate)
		{
			for (int i = 0; i < game.VisibleTiles.Length; i++)
			{
				if (predicate.Invoke(game.VisibleTiles[i]))
				{
					return i;
				}
			}

			return -1;
		}

		#endregion
	}
}