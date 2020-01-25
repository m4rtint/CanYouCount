using NUnit.Framework;

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

		[Test]
		public void Test_GameConstructor_Initial()
		{
			// Arrange/Act
			const int NUM_VISIBLE = 5,
				TOTAL_NUM = 25;

			var random = new SeededRandomService();
			var game = new Game(random, NUM_VISIBLE, TOTAL_NUM);

			// Assert
			Assert.AreEqual(NUM_VISIBLE, game.VisibleTileCount);
			Assert.AreEqual(NUM_VISIBLE, game.VisibleTiles.Length);

			// Check to ensure the numbers visible are (1 to n)
			for (int i = 1; i <= NUM_VISIBLE; i++)
			{
				// Check if contains tile with number
				bool found = false;
				for (int j = 0; j < game.VisibleTileCount; j++)
				{
					if (i == game.VisibleTiles[j].MyNumber)
					{
						found = true;
						break;
					}
				}

				if (!found)
				{
					Assert.Fail($"Expected Game.VisibleTiles to contain the numbers [1 to {NUM_VISIBLE}], but didn't find [{i}]");
				}
			}
		}
	}
}