using MahjongSharp.Tile;

namespace MahjongSharp.Yaku;

public class AllHonorsYaku : AYaku
{
    public override bool DoesMatch(IEnumerable<ATile> tiles)
    {
        return tiles.All(x => x is HonorTile);
    }
    
    public override bool ClosedOnly => false;
}