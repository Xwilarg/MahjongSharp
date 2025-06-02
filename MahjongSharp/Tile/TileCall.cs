namespace MahjongSharp.Tile;

public static class TileCall
{
    public static IEnumerable<TileGroup> GetPon(IEnumerable<ATile> tiles, ATile with)
    {
        var possibles = tiles.Where(x => x == with).ToArray();

        if (possibles.Length < 2) return []; // Not enough matches

        List<TileGroup> groups = [];

        for (int i = 0; i < possibles.Length - 1; i++)
        {
            for (int y = i + 1; y < possibles.Length - 1; y++)
            {
                groups.Add(new() { Tiles = [with, possibles[i], possibles[y]] });
            }
        }

        return groups;
    }

    public static IEnumerable<TileGroup> GetPon(IEnumerable<ATile> tiles)
    {
    }
}

public record TileGroup
{
    public IEnumerable<ATile> Tiles { set; get; }
}