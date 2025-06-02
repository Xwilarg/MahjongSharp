using MahjongSharp.Player;
using MahjongSharp.Ruleset;
using MahjongSharp.Tile;

namespace MahjongSharp;

public class Deck
{
    // All tiles we can still draw
    private IList<ATile> _tiles;
    // Tiles in the dead wall
    private IList<ATile> _deadWall;

    // Current players
    private IEnumerable<PlayerHand> _players;

    private Random _rand;

    public Deck(ARuleset ruleset, int playerCount, Random? rand = null)
    {
        _rand = rand ?? new Random();

        var tiles = ruleset.GetAllTiles().OrderBy(x => _rand.NextSingle());

        _players = Enumerable.Range(0, 4).Select(x => new PlayerHand(tiles.Skip(ruleset.HandSize * x).Take(ruleset.HandSize).ToList()));
        _deadWall = tiles.Skip(playerCount * ruleset.HandSize).Take(ruleset.DeadWallSize).ToList();
        _tiles = tiles.Skip(playerCount * ruleset.HandSize + ruleset.DeadWallSize).ToList();
    }
}