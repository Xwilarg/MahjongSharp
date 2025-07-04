using MahjongSharp.Player;
using MahjongSharp.Ruleset;
using MahjongSharp.Tile;

namespace MahjongSharp.Game;

public abstract class AGameClient
{
    private Wall _wall;
    private ARuleset _ruleset;
    private AGamePlayer[] _players;

    public AGameClient(ARuleset ruleset)
    {
        _ruleset = ruleset;
        _wall = new(ruleset);

        _players = [];
    }

    public void SetPlayers(AGamePlayer[] players)
    {
        _players = players;
    }

    public IEnumerable<ATile> DrawStartingTiles()
    {
        return _wall.GetTiles(_ruleset.HandSize);
    }
}
