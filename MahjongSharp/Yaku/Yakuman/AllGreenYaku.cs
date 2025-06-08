using MahjongSharp.Tile;

namespace MahjongSharp.Yaku;

public class AllGreenYaku : AYaku
{
    public override bool DoesMatch(IEnumerable<ATile> tiles)
    {
        // Tiles that are actually all green on the drawing itself
        return tiles.All(x =>
            {
                if (x is NumberedTile numTile)
                {
                    return numTile.Number == 2 || numTile.Number == 3 || numTile.Number == 4 || numTile.Number == 6 || numTile.Number == 8;
                }
                else if (x is DragonTile honor)
                {
                    return honor.Type == DragonType.Green;
                }
                return false;
            }
        );
    }
    
    public override bool ClosedOnly => false;
}