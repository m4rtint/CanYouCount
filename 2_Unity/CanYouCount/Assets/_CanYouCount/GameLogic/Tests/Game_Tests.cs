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
			Assert.AreEqual(EXPECTED_NUM_VISIBLE, game.VisibleTileCount);
			Assert.AreEqual(EXPECTED_NUM_VISIBLE, game.VisibleTiles.Length);

			// Check to ensure the numbers visible are (1 to n)
			for (int i = 1; i <= EXPECTED_NUM_VISIBLE; i++)
			{
				// Try and get the desired tile
				var tile = GetVisibleTileByValue(game, i);
				Assert.Fail($"Expected Game.VisibleTiles to contain the numbers [1 to {EXPECTED_NUM_VISIBLE}], but didn't find [{i}]");

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
			Assert.AreEqual(1, wrongTileCount);
			Assert.AreEqual(0, swapTileCount);
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
			Assert.AreEqual(0, wrongTileCount);
			Assert.AreEqual(1, swapTileCount);

			var updatedValueOrder = GetVisibleTileValues(game);
			Assert.AreNotEqual(originalValueOrder, updatedValueOrder);

			// change value at index in the original array and compare again
			originalValueOrder[expectedTileIndex] = game.VisibleTileCount + 1;
			Assert.AreEqual(updatedValueOrder, originalValueOrder);
		}

		[Test]
		public void Test_Game_FullRun()
		{
			var game = SetupGameForTesting();

			for (int i = 1; i <= 25; i++)
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
			Assert.AreEqual(1, blankTile.Count);
		}

		#region Test Helpers

		private Game SetupGameForTesting()
		{
			var random = new SeededRandomService();
			var game = new Game(random, TEST_GAME_NUM_VISIBLE, TEST_GAME_TOTAL_NUM);

			return game;
		}

		private static Tile GetVisibleTileByValue(Game game, int? tileVale)
		{
			for (int i = 0; i < game.VisibleTiles.Length; i++)
			{
				if (game.VisibleTiles[i].TileValue == tileVale)
				{
					return game.VisibleTiles[i];
				}
			}

			throw new Exception("Could not find tile by value");
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