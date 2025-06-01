namespace MahjongSharp.Tile;

public class NumberedTile : ATile
{
    public NumberedTile(int number, NumberedTileType type, bool isAkaDora) : base(isAkaDora)
    {
        Number = number;
        Type = type;
    }

    public override bool IsDora(ATile tile)
    {
        if (tile is not NumberedTile numTile || numTile.Type != Type) return false;

        if (numTile.Number == 9) return Number == 1;
        return numTile.Number == Number + 1;
    }

    public int Number { private set; get; }
    public NumberedTileType Type { private set; get; }
}

public enum NumberedTileType
{
    Kanji,
    Bamboo,
    Circle
}