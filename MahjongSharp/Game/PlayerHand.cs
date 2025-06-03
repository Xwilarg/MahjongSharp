using MahjongSharp.Tile;

namespace MahjongSharp.Game;

public class PlayerHand
{
    private IList<ATile> _tiles;
    private IList<ATile[]> _kans;

    public PlayerHand(IList<ATile> startingTiles)
    {
        _tiles = startingTiles;
    }

    public void AddTile(ATile tile)
    {
        _tiles.Add(tile);
    }
}