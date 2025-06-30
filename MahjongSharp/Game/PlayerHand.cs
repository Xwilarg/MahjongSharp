using MahjongSharp.Tile;
using MahjongSharp.Helper;

namespace MahjongSharp.Game;

public class PlayerHand
{
    public IList<ATile> Tiles { private set; get; }
    public IList<ATile> Discard { private set; get; }

    private IList<ATile[]> _kans;

    public PlayerHand(IEnumerable<ATile> startingTiles)
    {
        Tiles = startingTiles.ToList();
        Discard = new List<ATile>();
    }

    public void AddTile(ATile tile)
    {
        Tiles.Add(tile);
    }

    public void DiscardTile(ATile tile)
    {
        Discard.Add(tile);
    }

    public void DiscardTileFromHand(ATile tile)
    {
        Tiles.Remove(tile);
        DiscardTile(tile);
    }

    public void SortHand()
    {
        Tiles = [.. TileHelper.SortTiles(Tiles)];
    }
}