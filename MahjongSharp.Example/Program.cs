using MahjongSharp.Game;
using MahjongSharp.Ruleset;

// For this example, we are using riichi mahjong
var ruleset = new RiichiRuleset();

// Create the wall
var wall = new Wall(ruleset);

// We register 4 players
List<PlayerHand> players = [];
for (int i = 0; i < 4; i++)
{
    var p = new PlayerHand(wall.GetTiles(ruleset.HandSize));
    players.Add(p);
    p.SortHand();
}

int currTurn = 0;
var player = players[0];

while (true)
{
    Console.Clear();

    // Show current game state
    Console.WriteLine("Player");
    Console.WriteLine(string.Join("", player.GetTextNotation()));

    for (int i = 1; i < players.Count; i++)
    {
        Console.WriteLine();

        var ai = players[i];
        Console.WriteLine($"AI {i}");
        Console.WriteLine(string.Join("", Enumerable.Repeat("?", ai.Tiles.Count)));
    }

    Console.ReadKey();
}