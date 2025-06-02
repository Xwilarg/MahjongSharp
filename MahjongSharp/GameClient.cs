using MahjongSharp.Player;
using MahjongSharp.Ruleset;
using MahjongSharp.Tile;

namespace MahjongSharp;

public class GameClient
{
    // All tiles we can still draw
    private IList<ATile> _tiles;
    // Tiles in the dead wall
    private IList<ATile> _deadWall;

    // Current players
    private IEnumerable<PlayerHand> _players;

    private Random _rand;

    public GameClient(ARuleset ruleset, int playerCount, Random? rand = null)
    {
        var wall = new Wall(ruleset, rand);

        _players = Enumerable.Range(0, playerCount).Select(x => new PlayerHand(wall.GetTiles(ruleset.HandSize).ToList()));
    }
}