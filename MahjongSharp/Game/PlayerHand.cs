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
        /*Tiles = Tiles.OrderBy(x =>
        {
            if (x is NumberedTile numTile)
            {

            }
        }).ToList();*/
    }
}