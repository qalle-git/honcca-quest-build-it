namespace HonccaBuildingGame.Classes.Tiles
{
    public struct Tile
    {
        public int TileX;
        public int TileY;

        public int TileIndex;
        public int TileLayer;

        public Type TileType;

        public enum Type
        {
            NONE = 0,
            COLLISION = 1,
            FALL_THROUGH = 2,
            JUMP_THROUGH = 4
        }
    }
}
