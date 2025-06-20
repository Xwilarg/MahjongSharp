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

    // Display player hand
    var textNotation = TileHelper.GetTextNotation(player.Tiles);
    Console.Write(textNotation);

    // ... along with the tile we just drew
    var newTile = wall.GetTile();
    Console.WriteLine($" {TileHelper.GetTextNotation([ newTile ])}");

    // Hint under the text notation that associate a number/capital letter under each tile
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

    var canChii = TileCall.CanChii(player.Tiles, newTile);
    var canPon = TileCall.CanPon(player.Tiles, newTile);
    var canKan = TileCall.CanKan(player.Tiles, newTile);
    if (canChii || canPon || canKan)
    {
        Console.WriteLine("OR"); // Tile calls
        if (canChii) Console.WriteLine("c: chii");
        if (canPon) Console.WriteLine("p: pon");
        if (canKan) Console.WriteLine("k: kan");
        //Console.WriteLine("r: riichi");
        //Console.WriteLine("t: tsumo");
    }

    Console.ReadKey();
}