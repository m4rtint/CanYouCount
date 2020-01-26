using NUnit.Framework;
using System;

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

		private Game SetupGameForTesting()
		{
			const int NUM_VISIBLE = 5;
			const int TOTAL_NUM = 25;

			var random = new SeededRandomService();
			var game = new Game(random, NUM_VISIBLE, TOTAL_NUM);

			return game;
		}

		private static Tile GetVisibleTileByValue(Game game, int tileVale)
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
			Assert.AreEqual(EXPECTED_TOTAL_NUM, game.VisibleTiles.Length);

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
			Assert.AreEqual(originalValueOrder, updatedValueOrder);
		}
	}
}