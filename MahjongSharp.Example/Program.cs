using MahjongSharp.Example;
using MahjongSharp.Helper;
using MahjongSharp.Ruleset;

// For this example, we are using riichi mahjong
var ruleset = new RiichiRuleset();

var client = new TextGameClient(ruleset);

while (true)
{
    client.PlayNextTurn();

    var possibleInteruptions = client.GetPossibleInteruptions();

    var ponInterupt = possibleInteruptions.FirstOrDefault(x => x.Value.HasFlag(InteruptionCall.Pon) || x.Value.HasFlag(InteruptionCall.Kan));

    await Task.Delay(500);
}