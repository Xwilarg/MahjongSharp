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
            Tiles.Remove(newTile);
            Discard.Push(discardTile);
        }
        if (_sortAuto) SortHand();
    }

    public ATile RemoveLastDiscard()
    {
        return Discard.Pop();
    }

    public void Interupt(Mentsu call, IEnumerable<ATile> tiles)
    {
        GameClient.Interupt(this, call, tiles);
    }

    public ATile? LastDiscarded => Discard.Count == 0 ? null : Discard.Peek();

    /// <summary>
    /// Called when the hand need to be updated
    /// </summary>
    /// <param name="newTile">Tile we just drew, null if it's not our turn</param>
    public abstract void ShowStatus(ATile? newTile);
    /// <summary>
    /// Ask the player to discard a tile
    /// </summary>
    /// <param name="newTile">Tile that was previously drew, if the player just did a call (chii, pon, kan), it can be null</param>
    /// <returns>Which tile we want to discard</returns>
    public abstract ATile GetDiscard(ATile? newTile);

    private bool _sortAuto;

    public AGameClient GameClient { set; protected get; }
}
