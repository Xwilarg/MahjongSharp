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

    public virtual Naki GetPossibleOpenInteruptions(ATile with)
    {
        Naki call = Naki.None;
        if (TileCall.CanChii(Tiles, with)) call |= Naki.Chii;
        if (TileCall.CanPon(Tiles, with)) call |= Naki.Pon;
        if (TileCall.CanKan(Tiles, with)) call |= Naki.Kan;

        return call;
    }

    public virtual Naki GetPossibleClosedInteruptions(ATile with)
    {
        Naki call = Naki.None;
        if (TileCall.CanKan(Tiles)) call |= Naki.Kan;
        if (Calls.Any(x => x.Type == Mentsu.Pon && TileCall.CanKan(x.Tiles, with))) // We have a pon that can be changed to a kan
            call |= Naki.PonToKan;

        return Naki.None;
    }

    public void MakeCloseCall(Mentsu call, IEnumerable<ATile> tiles)
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

    public void MakeOpenCall(Mentsu call, IEnumerable<ATile> tiles, ATile with, int from)
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
        var call = Calls.First(x => x.Type == Mentsu.Pon && x.Tiles.All(x => x.IsSimilarTo(with)));
        call.IsKanAdded = true;
        List<ATile> tiles = call.Tiles.ToList();

    }

    /// <summary>
    /// Move a tile from the player hand to the discard
    /// </summary>
    public void MoveToDiscard(ATile tile)
    {
        Tiles.Remove(tile);
        Discard.Push(tile);
    }

    /// <summary>
    /// Sort all tiles in hand
    /// </summary>
    public void SortHand()
    {
        Tiles = [.. TileHelper.SortTiles(Tiles)];
    }
}