using MahjongSharp.Tile;
using MahjongSharp.Helper;

namespace MahjongSharp.Player;

public class PlayerHand
{
    public IList<ATile> Tiles { private set; get; }
    public Stack<ATile> Discard { private set; get; }
    public IList<PlayerTileCall> Calls { private set; get; }

    public PlayerHand(IEnumerable<ATile> startingTiles)
    {
        Tiles = startingTiles.ToList();
        Discard = [];
        Calls = [];
    }

    public InteruptionCall GetPossibleInteruptions(ATile with)
    {
        InteruptionCall call = InteruptionCall.None;
        if (TileCall.CanChii(Tiles, with)) call |= InteruptionCall.Chii;
        if (TileCall.CanPon(Tiles, with)) call |= InteruptionCall.Pon;
        if (
            TileCall.CanKan(Tiles, with) // Hand interupt with given tile
            || Calls.Any(x => x.Type == InteruptionCall.Pon && TileCall.CanKan(x.Tiles, with)) // We have a pon that can be changed to a kan
        )
        {
            call |= InteruptionCall.Kan;
        }

        return call;
    }

    public InteruptionCall GetPossibleInteruptions()
    {
        if (TileCall.CanKan(Tiles)) return InteruptionCall.Kan;
        return InteruptionCall.None;
    }

    public void MakeCloseCall(InteruptionCall call, IEnumerable<ATile> tiles)
    {
        Calls.Add(new()
        {
            IsOpen = false,
            Type = call,
            PlayerSource = null,
            Tiles = tiles,
            IsKanAdded = false
        });
        foreach (var t in tiles) Tiles.Remove(t);
    }

    public void MakeOpenCall(InteruptionCall call, IEnumerable<ATile> tiles, ATile with, int from)
    {
        List<ATile> lTiles = tiles.ToList();
        if (from == lTiles.Count) lTiles.Add(with);
        else lTiles.Insert(from, with);

        Calls.Add(new()
        {
            IsOpen = true,
            Type = call,
            PlayerSource = from,
            Tiles = lTiles,
            IsKanAdded = false
        });
        foreach (var t in tiles) Tiles.Remove(t);
    }

    public void UpdatePonToKan(ATile with)
    {
        var call = Calls.First(x => x.Type == InteruptionCall.Pon && x.Tiles.All(x => x.IsSimilarTo(with)));
        call.IsKanAdded = true;
        List<ATile> tiles = call.Tiles.ToList();

    }

    /// <summary>
    /// Add a new tile to the player hand
    /// </summary>
    public void AddTile(ATile tile)
    {
        Tiles.Add(tile);
    }

    /// <summary>
    /// Add a new tile to the player discard
    /// </summary>
    public void DiscardTile(ATile tile)
    {
        Discard.Push(tile);
    }

    public ATile RemoveLastDiscard()
    {
        return Discard.Pop();
    }

    /// <summary>
    /// Move a tile from the player hand to the discard
    /// </summary>
    public void MoveToDiscard(ATile tile)
    {
        Tiles.Remove(tile);
        DiscardTile(tile);
    }

    /// <summary>
    /// Sort all tiles in hand
    /// </summary>
    public void SortHand()
    {
        Tiles = [.. TileHelper.SortTiles(Tiles)];
    }
}