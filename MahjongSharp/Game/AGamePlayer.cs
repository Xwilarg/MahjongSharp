using MahjongSharp.Helper;
using MahjongSharp.Player;
using MahjongSharp.Tile;

namespace MahjongSharp.Game;

public abstract class AGamePlayer : PlayerHand
{
    public AGamePlayer(IEnumerable<ATile> startingTiles, bool sortAuto = true) : base(startingTiles)
    {
        _sortAuto = sortAuto;
        if (sortAuto) SortHand();
    }

    public void AddTileToHandThenDiscard(ATile newTile, ATile discardTile)
    {
        if (newTile == discardTile) // Tile discarded is the tile we just drew
        {
            Discard.Push(newTile);
        }
        else
        {
            Tiles.Add(newTile);
            Discard.Push(discardTile);
        }
        if (_sortAuto) SortHand();
    }

    public ATile RemoveLastDiscard()
    {
        return Discard.Pop();
    }

    public void Interupt(Naki call, IEnumerable<ATile> tiles, ATile with)
    {
        GameClient.Interupt(this, call, tiles, with);
    }

    public ATile? LastDiscarded => Discard.Count == 0 ? null : Discard.Peek();

    /// <summary>
    /// Called when the hand need to be updated
    /// </summary>
    /// <param name="newTile">Tile we just drew, null if it's not our turn</param>
    public abstract void ShowStatus(ATile? newTile);
    public abstract ATile GetDiscard(ATile newTile);

    private bool _sortAuto;

    public AGameClient GameClient { set; protected get; }
}
