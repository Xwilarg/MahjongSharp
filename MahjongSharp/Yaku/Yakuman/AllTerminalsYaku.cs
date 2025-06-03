using MahjongSharp.Tile;

namespace MahjongSharp.Yaku;

public class AllTerminalsYaku : AYaku
{
    public override bool DoesMatch(IEnumerable<ATile> tiles)
    {
        return tiles.All(x => x is NumberedTile numTile && (numTile.Number == 1 || numTile.Number == 9));
    }
    
    public override bool ClosedOnly => false;
}