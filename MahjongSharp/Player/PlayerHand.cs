using MahjongSharp.Tile;

namespace MahjongSharp.Player;

public class PlayerHand
{
    private IList<ATile> _tiles;

    public PlayerHand(IList<ATile> startingTiles)
    {
        _tiles = startingTiles;
    }
}