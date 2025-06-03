namespace MahjongSharp.Tile;

public static class TileCall
{
    public static IEnumerable<TileGroup> CanKan(IEnumerable<ATile> tiles, ATile with)
    {
        var possibles = tiles.Where(x => x == with).ToArray();
        if (possibles.Length == 3) yield return new() { Tiles = [with, .. possibles] };
    }

    public static IEnumerable<TileGroup> CanKan(IEnumerable<ATile> tiles)
    {
        foreach (var t in tiles.GroupBy(x => x))
        {
            if (t.Count() == 4)
            {
                yield return new TileGroup() { Tiles = t };
            }
        }
    }

    public static IEnumerable<TileGroup> GetPon(IEnumerable<ATile> tiles, ATile with)
    {
        var possibles = tiles.Where(x => x == with).ToArray();

        Console.WriteLine($"{possibles.Length}");
        if (possibles.Length < 2) yield break; // Not enough matches

        for (int i = 0; i < possibles.Length - 1; i++)
        {
            for (int y = i + 1; y < possibles.Length - 1; y++)
            {
                yield return new() { Tiles = [with, possibles[i], possibles[y]] };
            }
        }
    }

    public static IEnumerable<TileGroup> GetChii(IEnumerable<ATile> tiles, ATile with)
    {
        if (with is not NumberedTile numTile) yield break;

        var possibles = tiles.Where(x => x is NumberedTile xNumTile && xNumTile.Type == numTile.Type).Select(x => (NumberedTile)x);
        foreach (var prev in possibles.Where(x => x.Number == numTile.Number - 1)) // Start at previous tile...
        {
            foreach (var prev2 in possibles.Where(x => x.Number == numTile.Number - 2)) // Add with tile - 2
            {
                yield return new TileGroup() { Tiles = [ with, prev, prev2 ] };
            }
            foreach (var next in possibles.Where(x => x.Number == numTile.Number + 1)) // Add with tile + 1
            {
                yield return new TileGroup() { Tiles = [ with, prev, next ] };
            }
        }
        foreach (var next in possibles.Where(x => x.Number == numTile.Number + 1)) // Start at next tile...
        {
            foreach (var next2 in possibles.Where(x => x.Number == numTile.Number + 2)) // Add with tile + 2
            {
                yield return new TileGroup() { Tiles = [ with, next, next2 ] };
            }
        }
    }
}

public record TileGroup
{
    public IEnumerable<ATile> Tiles { set; get; }
}