using MahjongSharp.Tile;

namespace MahjongSharp.Test;

public class TileCallTests
{
    private static ATile[] _hand1 =
    [
        new NumberedTile(1, NumberedTileType.Bamboo, false),
        new NumberedTile(1, NumberedTileType.Bamboo, false),
        new NumberedTile(4, NumberedTileType.Circle, false),
        new NumberedTile(5, NumberedTileType.Circle, false),
        new NumberedTile(5, NumberedTileType.Circle, false),
        new NumberedTile(5, NumberedTileType.Circle, false)
    ];

    public static object[] PonHands =
    {
        new object[] { _hand1, new NumberedTile(1, NumberedTileType.Bamboo, false), true },
        new object[] { _hand1, new NumberedTile(1, NumberedTileType.Kanji, false), false },
        new object[] { _hand1, new NumberedTile(5, NumberedTileType.Circle, false), true }
    };
    public static object[] KanHands =
    {
        new object[] { _hand1, new NumberedTile(1, NumberedTileType.Bamboo, false), false },
        new object[] { _hand1, new NumberedTile(1, NumberedTileType.Kanji, false), false },
        new object[] { _hand1, new NumberedTile(5, NumberedTileType.Circle, false), true }
    };

    [TestCaseSource(nameof(PonHands))]
    public void TestPonWith(ATile[] hand, ATile with, bool doesMatch)
    {
        var res = TileCall.GetPon(hand, with);
        Assert.That(res, doesMatch ? Is.Not.Empty : Is.Empty);
    }

    [TestCaseSource(nameof(KanHands))]
    public void TestKanWith(ATile[] hand, ATile with, bool doesMatch)
    {
        var res = TileCall.GetKan(hand, with);
        Assert.That(res, doesMatch ? Is.Not.Empty : Is.Empty);
    }

    [TestCaseSource(nameof(KanHands))]
    public void TestKan(ATile[] hand, ATile with, bool doesMatch)
    {
        var res = TileCall.GetKan([with, ..hand]);
        Assert.That(res, doesMatch ? Is.Not.Empty : Is.Empty);
    }
}