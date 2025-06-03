using MahjongSharp.Tile;

namespace MahjongSharp.Yaku;

public class BigWindsYaku : AYaku
{
    public override bool DoesMatch(IEnumerable<ATile> tiles)
    {
        // All tiles are triplets of winds, meaning 2 tiles are others things
        return tiles.Count(x => x is not HonorTile honor || (honor.Type != HonorType.EastWind && honor.Type != HonorType.NorthWind && honor.Type != HonorType.WestWind && honor.Type != HonorType.SouthWind)) == 2;
    }
    
    public override bool ClosedOnly => false;
}