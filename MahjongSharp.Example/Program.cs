using MahjongSharp.Game;
using MahjongSharp.Ruleset;
using MahjongSharp.Tile;

// For this example, we are using riichi mahjong
var ruleset = new RiichiRuleset();

// Create the wall
var wall = new Wall(ruleset);

// We register 4 players
List<PlayerHand> players = [];
for (int i = 0; i < 4; i++) players.Add(new PlayerHand(wall.GetTiles(ruleset.HandSize)));

int currTurn = 0;
var player = players[0];

while (true)
{
    Console.Clear();

    // Show current game state
    Console.WriteLine("Player");
    Console.WriteLine(string.Join("", player.Tiles.Select(x => x.GetUnicode())));

    Console.ReadKey();
}