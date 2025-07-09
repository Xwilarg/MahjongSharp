using MahjongSharp.Example;
using MahjongSharp.Game;
using MahjongSharp.Ruleset;

// For this example, we are using riichi mahjong
var ruleset = new RiichiRuleset();

var client = new TextGameClient(ruleset);

while (true)
{
    client.PlayNextTurn();

    var possibleInteruptions = client.GetPossibleInteruptions();

    await Task.Delay(500);
}