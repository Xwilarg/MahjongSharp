using MahjongSharp.Game;
using MahjongSharp.Ruleset;

namespace MahjongSharp.Example;
public class TextGameClient : AGameClient
{
    public TextGameClient(ARuleset ruleset) : base(ruleset)
    {
        SetPlayers([
            new AIPlayer(DrawStartingTiles()),
            new AIPlayer(DrawStartingTiles()),
            new AIPlayer(DrawStartingTiles()),
            new HumanPlayer(DrawStartingTiles())
        ]);
    }
}
