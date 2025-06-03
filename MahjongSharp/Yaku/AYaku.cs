using MahjongSharp.Tile;

namespace MahjongSharp.Yaku;

public abstract class AYaku
{
    public abstract bool DoesMatch(IEnumerable<ATile> tiles);

    public abstract bool ClosedOnly { get; }
}