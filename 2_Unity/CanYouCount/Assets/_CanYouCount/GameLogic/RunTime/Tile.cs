namespace CanYouCount
{
	[System.Diagnostics.DebuggerDisplay("Tile({_TileValue})")]
	public struct Tile
	{
		public static Tile BlankTile
			=> new Tile() { _TileValue = null };

		private int? _TileValue;

		public int? TileValue => _TileValue;

		public Tile(int number)
		{
			_TileValue = number;
		}
	}
}