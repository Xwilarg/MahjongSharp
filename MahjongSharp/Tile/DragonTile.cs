namespace MahjongSharp.Tile;

public class DragonTile : AHonorTile<DragonType>
{
    public DragonTile(DragonType type) : base(type)
    { }

    public override bool IsDora(ATile tile)
    {
        if (tile is not DragonTile honorTile) return false;

        return honorTile.Type switch
        {
            DragonType.Green => Type == DragonType.Red,
            DragonType.White => Type == DragonType.Green,
            DragonType.Red => Type == DragonType.White,
            _ => throw new NotImplementedException()
        };
    }

    public override string GetUnicode()
    {
        return Type switch
        {
            DragonType.Green => "🀅",
            DragonType.White => "🀆",
            DragonType.Red => "🀄",
            _ => string.Empty
        };
    }

    public static bool operator ==(DragonTile? a, DragonTile? b)
    {
        if (a is null) return b is null;
        if (b is null) return false;
        return a.Type == b.Type;
    }

    public static bool operator !=(DragonTile? a, DragonTile? b)
        => !(a == b);

    public override bool Equals(object? o)
    {
        if (o is null) return false;
        if (o is not DragonTile tile) return false;
        return tile == this;
    }

    public override int GetHashCode()
    {
        return Type.GetHashCode();
    }
}

public enum DragonType
{
    Green,
    White,
    Red
}
