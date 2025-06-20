using MahjongSharp.Tile;
using MahjongSharp.Helper;

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
        Tiles = [.. TileHelper.SortTiles(Tiles) ];
    }
}