using MahjongSharp.Example;
using MahjongSharp.Helper;
using MahjongSharp.Ruleset;
using MahjongSharp.Tile;

// For this example, we are using riichi mahjong
var ruleset = new RiichiRuleset();

var client = new TextGameClient(ruleset);

while (true)
{
    client.PlayNextTurn();

    var possibleInteruptions = client.GetPossibleInteruptions(out var tile);


    if (possibleInteruptions.Any(x => x.Key is HumanPlayer))
    {
        var p = possibleInteruptions.First(x => x.Key is HumanPlayer);
        var hp = (HumanPlayer)p.Key;

        (Naki call, IEnumerable<ATile> tiles) = hp.GetCallChoice(p.Value, tile);

        if (call != Naki.None)
        {
            hp.Interupt((Mentsu)call, tiles);
        }
    }

    await Task.Delay(500);
}