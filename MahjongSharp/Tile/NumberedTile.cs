namespace MahjongSharp.Tile;

public class NumberedTile : ATile
{
    public NumberedTile(int number, NumberedTileType type, bool isAkaDora)
    {
        Number = number;
        Type = type;
        _isAkaDora = isAkaDora;
    }

    public override bool IsDora(ATile tile)
    {
        if (tile is not NumberedTile numTile || numTile.Type != Type) return false;

        if (numTile.Number == 9) return Number == 1;
        return numTile.Number == Number + 1;
    }

    public int Number { private set; get; }
    public NumberedTileType Type { private set; get; }
    private bool _isAkaDora;

    public static bool operator ==(NumberedTile a, NumberedTile b)
        => a.Type == b.Type && a.Number == b.Number;

    public static bool operator !=(NumberedTile a, NumberedTile b)
        => !(a == b);

    public override bool Equals(object? o)
    {
        if (o is null) return false;
        if (o is not NumberedTile tile) return false;
        return tile == this;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Number);
    }
}

public enum NumberedTileType
{
    Kanji,
    Bamboo,
    Circle
}