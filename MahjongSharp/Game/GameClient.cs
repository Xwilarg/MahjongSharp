using MahjongSharp.Ruleset;
using MahjongSharp.Tile;

namespace MahjongSharp.Game;

public class GameClient
{
    // Current players
    private PlayerHand[] _players;
    private int _currentPlayer;

    public GameClient(ARuleset ruleset, int playerCount, Random? rand = null)
    {
        var wall = new Wall(ruleset, rand);

        _players = Enumerable.Range(0, playerCount).Select(x => new PlayerHand(wall.GetTiles(ruleset.HandSize).ToList())).ToArray();
    }
}