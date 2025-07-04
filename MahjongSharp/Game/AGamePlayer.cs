using MahjongSharp.Player;
using MahjongSharp.Tile;

namespace MahjongSharp.Game;

public abstract class AGamePlayer
{
    public AGamePlayer(IEnumerable<ATile> startingTiles, bool sortAuto = true)
    {
        _hand = new PlayerHand(startingTiles);
        _sortAuto = sortAuto;
        if (sortAuto) _hand.SortHand();
    }

    /// <summary>
    /// Called when the hand need to be updated
    /// </summary>
    /// <param name="newTile">Tile we just drew, null if it's not our turn</param>
    public abstract void ShowStatus(ATile? newTile);

    private bool _sortAuto;
    protected PlayerHand _hand;
}
