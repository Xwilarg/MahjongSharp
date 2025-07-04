using MahjongSharp.Example;
using MahjongSharp.Game;
using MahjongSharp.Ruleset;

// For this example, we are using riichi mahjong
var ruleset = new RiichiRuleset();

var client = new TextGameClient(ruleset);

while (true)
{
    Console.Clear();

    for (int turn = 0; turn < players.Length; turn++) // Turn around the table
    {
        Console.Clear();

        var upcomingTile = wall.GetTile();
        for (int p = 0; p < players.Length; p++) // A player draw a tile, we show the status of everyone
        {
            var currPlayer = players[p];

            var discard = currPlayer.ShowStatus(turn == p ? upcomingTile : null);

            if (discard != null) currPlayer.DiscardTileFromHand(discard);

            Console.WriteLine();
        }

        await Task.Delay(500);
    }
}