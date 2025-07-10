using MahjongSharp.Player;
using MahjongSharp.Tile;

namespace MahjongSharp.Helper;

/// <summary>
/// Mentsu are groups of tiles that can form a winning hand
/// https://riichi.wiki/Mentsu
/// </summary>
public enum Mentsu
{
    /// <summary>
    /// Sequence of tiles
    /// </summary>
    Chii = 1,
    /// <summary>
    /// 3 times the same tile
    /// </summary>
    Pon = 2,
    /// <summary>
    /// 4 times the same tile
    /// </summary>
    Kan = 4
}

/// <summary>
/// All possibles calls
/// </summary>
[Flags]
public enum Naki
{
    None = 0,
    /// <summary>
    /// Sequence of tiles
    /// </summary>
    Chii = 1,
    /// <summary>
    /// 3 times the same tile
    /// </summary>
    Pon = 2,
    /// <summary>
    /// 4 times the same tile
    /// </summary>
    Kan = 4,
    /// <summary>
    /// A pon that can be turned into a kan if we drew the same tile
    /// </summary>
    PonToKan = 8,
    /// <summary>
    /// North winds in 3P riichi
    /// Or flowers in MCR
    /// </summary>
    Flower = 16,
    /// <summary>
    /// Ron or tsumo, depending of the tile is from another player or not
    /// </summary>
    Agari = 32
}

public static class TileCall
{
    public static bool CanKan(IEnumerable<ATile> tiles, ATile with)
    {
        return tiles.Count(x => x.IsSimilarTo(with)) == 3;
    }

    public static IEnumerable<TileGroup> GetKan(IEnumerable<ATile> tiles, ATile with)
    {
        var possibles = tiles.Where(x => x.IsSimilarTo(with)).ToArray();
        if (possibles.Length == 3) yield return new() { Tiles = [with, .. possibles] };
    }

    public static bool CanKan(IEnumerable<ATile> tiles)
    {
        return tiles.GroupBy(x => x.SimilarHashCode).Any(x => x.Count() == 4);
    }

    public static IEnumerable<TileGroup> GetKan(IEnumerable<ATile> tiles)
    {
        foreach (var t in tiles.GroupBy(x => x.SimilarHashCode))
        {
            if (t.Count() == 4)
            {
                yield return new TileGroup() { Tiles = t };
            }
        }
    }

    public static bool CanKan(IEnumerable<PlayerTileCall> calls, ATile with)
    {
        return calls.Any(x => x.Type == Mentsu.Pon && x.Tiles.All(x => x.IsSimilarTo(with)));
    }

    public static PlayerTileCall? GetKan(IEnumerable<PlayerTileCall> calls, ATile with)
    {
        return calls.FirstOrDefault(x => x.Type == Mentsu.Pon && x.Tiles.All(x => x.IsSimilarTo(with)));
    }

    public static bool CanPon(IEnumerable<ATile> tiles, ATile with)
    {
        return tiles.Count(x => x.IsSimilarTo(with)) >= 2;
    }

    public static IEnumerable<TileGroup> GetPon(IEnumerable<ATile> tiles, ATile with)
    {
        var possibles = tiles.Where(x => x.IsSimilarTo(with)).ToArray();

        if (possibles.Length < 2) yield break; // Not enough matches

        for (int i = 0; i < possibles.Length; i++)
        {
            for (int y = i + 1; y < possibles.Length; y++)
            {
                yield return new() { Tiles = [with, possibles[i], possibles[y]] };
            }
        }
    }

    public static bool CanChii(IEnumerable<ATile> tiles, ATile with)
    {
        if (with is not NumberedTile numTile) return false;

        var possibles = tiles.Where(x => x is NumberedTile xNumTile && xNumTile.Type == numTile.Type).Select(x => (NumberedTile)x);
        return
            possibles.Any(x => x.Number == numTile.Number - 1) && possibles.Any(x => x.Number == numTile.Number - 2) ||
            possibles.Any(x => x.Number == numTile.Number + 1) && possibles.Any(x => x.Number == numTile.Number + 2) ||
            possibles.Any(x => x.Number == numTile.Number - 1) && possibles.Any(x => x.Number == numTile.Number + 1);
    }

    public static IEnumerable<TileGroup> GetChii(IEnumerable<ATile> tiles, ATile with)
    {
        if (with is not NumberedTile numTile) yield break;

        var possibles = tiles.Where(x => x is NumberedTile xNumTile && xNumTile.Type == numTile.Type).Select(x => (NumberedTile)x);
        foreach (var prev in possibles.Where(x => x.Number == numTile.Number - 1)) // Start at previous tile...
        {
            foreach (var prev2 in possibles.Where(x => x.Number == numTile.Number - 2)) // Add with tile - 2
            {
                yield return new TileGroup() { Tiles = [with, prev, prev2] };
            }
            foreach (var next in possibles.Where(x => x.Number == numTile.Number + 1)) // Add with tile + 1
            {
                yield return new TileGroup() { Tiles = [with, prev, next] };
            }
        }
        foreach (var next in possibles.Where(x => x.Number == numTile.Number + 1)) // Start at next tile...
        {
            foreach (var next2 in possibles.Where(x => x.Number == numTile.Number + 2)) // Add with tile + 2
            {
                yield return new TileGroup() { Tiles = [with, next, next2] };
            }
        }
    }
}

public record TileGroup
{
    public IEnumerable<ATile> Tiles { set; get; }
}