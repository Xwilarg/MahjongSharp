namespace MahjongSharp.Tile;

public class HonorTile : ATile
{
    public HonorTile(HonorType type)
    {
        Type = type;
    }

    public override bool IsDora(ATile tile)
    {
        if (tile is not HonorTile honorTile) return false;

        return honorTile.Type switch
        {
            HonorType.GreenDragon => Type == HonorType.RedDragon,
            HonorType.WhiteDragon => Type == HonorType.GreenDragon,
            HonorType.RedDragon => Type == HonorType.WhiteDragon,
            HonorType.NorthWind => Type == HonorType.EastWind,
            HonorType.WestWind => Type == HonorType.NorthWind,
            HonorType.SouthWind => Type == HonorType.WestWind,
            HonorType.EastWind => Type == HonorType.SouthWind,
            _ => throw new NotImplementedException()
        };
    }

    public override string GetUnicode()
    {
        return Type switch
        {
            HonorType.GreenDragon => "🀅",
            HonorType.WhiteDragon => "🀆",
            HonorType.RedDragon => "🀄",
            HonorType.NorthWind => "🀃",
            HonorType.WestWind => "🀂",
            HonorType.SouthWind => "🀁",
            HonorType.EastWind => "🀀",
            _ => string.Empty
        };
    }

    public HonorType Type { private set; get; }

    public static bool operator ==(HonorTile? a, HonorTile? b)
    {
        if (a is null) return b is null;
        if (b is null) return false;
        return a.Type == b.Type;
    }

    public static bool operator !=(HonorTile? a, HonorTile? b)
        => !(a == b);

    public override bool Equals(object? o)
    {
        if (o is null) return false;
        if (o is not HonorTile tile) return false;
        return tile == this;
    }

    public override int GetHashCode()
    {
        return Type.GetHashCode();
    }
}

public enum HonorType
{
    GreenDragon,
    WhiteDragon,
    RedDragon,
    NorthWind,
    WestWind,
    SouthWind,
    EastWind
}