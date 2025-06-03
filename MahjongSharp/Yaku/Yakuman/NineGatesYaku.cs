using MahjongSharp.Tile;

namespace MahjongSharp.Yaku;

public class NineGatesYaku : AYaku
{
    public override bool DoesMatch(IEnumerable<ATile> tiles)
    {
        return tiles.GroupBy(x => x).Count(x => TileCall.GetKan(x).Any()) == 4; // 4 kans mean everything is kan
    }

    public override bool ClosedOnly => true;
}