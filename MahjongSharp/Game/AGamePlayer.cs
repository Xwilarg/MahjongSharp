using MahjongSharp.Helper;
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

    public void AddTileToHandThenDiscard(ATile newTile, ATile discardTile)
    {
        if (newTile == discardTile)
        {
            _hand.DiscardTile(newTile);
        }
        else
        {
            _hand.AddTile(newTile);
            _hand.DiscardTile(discardTile);
        }
        if (_sortAuto) _hand.SortHand();
    }

    public ATile RemoveLastDiscard()
    {
        return _hand.RemoveLastDiscard();
    }

    public InteruptionCall GetPossibleInteruptions(ATile tile, bool canChii)
    {
        var interupt = _hand.GetPossibleInteruptions(tile);
        if (!canChii) interupt &= ~InteruptionCall.Chii;

        return interupt;
    }

    public void Interupt(InteruptionCall call, IEnumerable<ATile> tiles, ATile with)
    {
        GameClient.Interupt(this, call, tiles, with);
    }

    internal void MakeOpenCall(InteruptionCall call, IEnumerable<ATile> tiles, ATile with, int from)
    {
        _hand.MakeOpenCall(call, tiles, with, from);
    }

    public ATile? LastDiscarded => _hand.Discard.Count == 0 ? null : _hand.Discard.Last();

    /// <summary>
    /// Called when the hand need to be updated
    /// </summary>
    /// <param name="newTile">Tile we just drew, null if it's not our turn</param>
    public abstract void ShowStatus(ATile? newTile);
    public abstract ATile GetDiscard(ATile? newTile);

    private bool _sortAuto;
    protected PlayerHand _hand;

    public AGameClient GameClient { set; protected get; }
}
