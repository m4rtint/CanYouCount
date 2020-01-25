using NUnit.Framework;

namespace CanYouCount
{
	public class Grid_Tests
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
		public void Test_()
		{
			var grid = new Grid(n, n);

			
		}
	}
}