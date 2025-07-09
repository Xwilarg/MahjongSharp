using MahjongSharp.Game;
using MahjongSharp.Ruleset;
using MahjongSharp.Tile;

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

    protected override void UpdatePlayerStatus(int index, ATile? tile)
    {
        Console.Clear();
        // We are in the console so in our case we can't update the status of a single player
        // For that reason, we instead update all players
        for (int i = 0; i < _players.Length; i++)
        {
            _players[i].ShowStatus(index == i ? tile : null);
        }
    }
}
