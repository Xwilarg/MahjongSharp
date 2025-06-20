using MahjongSharp.Game;
using MahjongSharp.Ruleset;
using MahjongSharp.Helper;
using System.Text;

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
    for (int i = 1; i < players.Count; i++)
    {
        Console.WriteLine();

        var ai = players[i];
        Console.WriteLine($"AI {i}");
        Console.WriteLine(string.Join("", Enumerable.Repeat("?", ai.Tiles.Count)));
    }

    Console.WriteLine();
    Console.WriteLine("Player");
    var textNotation = TileHelper.GetTextNotation(player.Tiles);
    Console.Write(textNotation);

    var newTile = wall.GetTile();
    Console.WriteLine($" {TileHelper.GetTextNotation([ newTile ])}");

    string hintText = "123456789ABCED";
    StringBuilder numPrev = new(); int c = 0;
    for (int i = 0; i < textNotation.Length; i++)
    {
        if (textNotation[i] >= '0' && textNotation[i] <= '9')
        {
            numPrev.Append(hintText[c]);
            c++;
        }
        else numPrev.Append(' ');
    }
    Console.WriteLine($"{numPrev} 0");
    Console.WriteLine("Enter an index to discard");
    Console.WriteLine("OR");
    Console.WriteLine("c: chii");
    Console.WriteLine("p: pon");
    Console.WriteLine("k: kan");
    Console.WriteLine("r: riichi");
    Console.WriteLine("t: tsumo");

    Console.ReadKey();
}