using MahjongSharp.Ruleset;
using MahjongSharp.Tile;

namespace MahjongSharp.Game;

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

    /// <summary>
    /// Look at the next tile in the wall without removing it, return null if there is none
    /// </summary>
    public ATile? PeekNextTile()
    {
        return _wall.TryPeek(out var tile) ? tile : null;
    }

    /// <summary>
    /// Get the next tile in the wall, null if none
    /// </summary>
    /// <returns></returns>
    public ATile? GetTile()
    {
        return _wall.TryDequeue(out var tile) ? tile : null;
    }
}