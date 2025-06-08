using MahjongSharp.Tile;

namespace MahjongSharp.Game;

public class PlayerHand
{
    public IList<ATile> Tiles { private set; get; }
    private IList<ATile[]> _kans;

    public PlayerHand(IEnumerable<ATile> startingTiles)
    {
        Tiles = startingTiles.ToList();
    }

    public void AddTile(ATile tile)
    {
        Tiles.Add(tile);
    }

    public void SortHand()
    {
        Tiles = Tiles.OrderBy(x =>
        {
            if (x is NumberedTile numTile)
            {
                return 000 + (10 * (int)numTile.Type) + numTile.Number;
            }
            else if (x is WindTile wind)
            {
                return 100 + (int)wind.Type;
            }
            else if (x is DragonTile dragon)
            {
                return 200 + (int)dragon.Type;
            }
            else return 999;
        }).ToList();
    }
}