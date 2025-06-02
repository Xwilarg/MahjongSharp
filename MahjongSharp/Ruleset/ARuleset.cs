using MahjongSharp.Tile;

namespace MahjongSharp.Ruleset;

public abstract class ARuleset
{
    public abstract int DeadWallSize { get; }
    public abstract int HandSize { get; }

    public abstract IEnumerable<ATile> GetAllTiles();
}