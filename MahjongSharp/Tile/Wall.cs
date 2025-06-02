using MahjongSharp.Ruleset;

namespace MahjongSharp.Tile;

public class Wall
{
    private Queue<ATile> _wall;
    private List<ATile> _deadWall;

    public Wall(ARuleset ruleset, Random? rand = null)
    {
        rand ??= new Random();
        var tiles = ruleset.GetAllTiles().OrderBy(x => rand.NextSingle());
        _deadWall = tiles.Take(ruleset.DeadWallSize).ToList();
        _wall = new Queue<ATile>(tiles.Skip(ruleset.DeadWallSize));
    }

    public IEnumerable<ATile> GetTiles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (!_wall.TryDequeue(out var tile))
            {
                yield break;
            }
            yield return tile;
        }
    }

    public ATile? GetTile()
    {
        if (_wall.TryDequeue(out var tile))
        {
            return tile;
        }
        return null;
    }
}