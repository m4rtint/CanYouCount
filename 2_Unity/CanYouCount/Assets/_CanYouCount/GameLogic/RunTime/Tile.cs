namespace CanYouCount
{ 
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