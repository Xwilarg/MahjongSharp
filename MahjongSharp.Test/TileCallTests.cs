using MahjongSharp.Tile;

namespace MahjongSharp.Test;

public class TileCallTests
{
    private static ATile[] _hand1 =
    [
        new NumberedTile(1, NumberedTileType.Bamboo, false),
        new NumberedTile(1, NumberedTileType.Bamboo, false)
    ];

    public static object[] PonHands =
    {
        new object[] { _hand1, new NumberedTile(1, NumberedTileType.Bamboo, false), true }
    };

    [TestCaseSource(nameof(PonHands))]
    public void TestPon(ATile[] hand, ATile with, bool doesMatch)
    {
        var res = TileCall.GetPon(hand, with);
        Assert.That(res, doesMatch ? Is.Not.Empty : Is.Empty);
    }
}