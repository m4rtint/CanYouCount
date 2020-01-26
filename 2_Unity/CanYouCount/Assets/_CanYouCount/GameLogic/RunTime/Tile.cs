namespace CanYouCount
{
	[System.Diagnostics.DebuggerDisplay("Tile({_TileValue})")]
	public struct Tile
	{
		private int? _TileValue;

		public int? TileValue => _TileValue;

		public Tile(int number)
		{
			_TileValue = number;
		}
	}
}