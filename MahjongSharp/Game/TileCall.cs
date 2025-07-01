using MahjongSharp.Tile;

namespace MahjongSharp.Game;

[Flags]
public enum InteruptionCall
{
    None = 0,
    Chii = 1,
    Pon = 2,
    Kan = 4
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
            (possibles.Any(x => x.Number == numTile.Number - 1) && possibles.Any(x => x.Number == numTile.Number - 2)) ||
            (possibles.Any(x => x.Number == numTile.Number + 1) && possibles.Any(x => x.Number == numTile.Number + 2)) ||
            (possibles.Any(x => x.Number == numTile.Number - 1) && possibles.Any(x => x.Number == numTile.Number + 1));
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