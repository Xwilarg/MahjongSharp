using MahjongSharp.Tile;
using MahjongSharp.Helper;

namespace MahjongSharp.Player;

public class PlayerHand
{
    public IList<ATile> Tiles { private set; get; }
    public IList<ATile> Discard { private set; get; }
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
        if (TileCall.CanKan(Tiles, with) || TileCall.CanKan(Tiles)) call |= InteruptionCall.Kan;

        return call;
    }

    public void MakeCloseCall(InteruptionCall call, IEnumerable<ATile> tiles)
    {
        Calls.Add(new()
        {
            IsOpen = false,
            Type = call,
            PlayerSource = null,
            Tiles = tiles
        });
        foreach (var t in tiles) Tiles.Remove(t);
    }

    public void MakeOpenCall(InteruptionCall call, IEnumerable<ATile> tiles, ATile with, int from)
    {
        Calls.Add(new()
        {
            IsOpen = true,
            Type = call,
            PlayerSource = from,
            Tiles = [ ..tiles, with ]
        });
        foreach (var t in tiles) Tiles.Remove(t);
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
        Discard.Add(tile);
    }

    /// <summary>
    /// For a tile in player hand, remove it and move it to the discard instead
    /// </summary>
    public void DiscardTileFromHand(ATile tile)
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