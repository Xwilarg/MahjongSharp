namespace MahjongSharp.Tile;

public class WindTile : AHonorTile<WindType>
{
    public WindTile(WindType type) : base(type)
    { }

    public override bool IsDora(ATile tile)
    {
        if (tile is not WindTile honorTile) return false;

        return honorTile.Type switch
        {
            WindType.North => Type == WindType.East,
            WindType.West => Type == WindType.North,
            WindType.South => Type == WindType.West,
            WindType.East => Type == WindType.South,
            _ => throw new NotImplementedException()
        };
    }

    public override string GetUnicode()
    {
        return Type switch
        {
            WindType.North => "🀃",
            WindType.West => "🀂",
            WindType.South => "🀁",
            WindType.East => "🀀",
            _ => string.Empty
        };
    }

    public static bool operator ==(WindTile? a, WindTile? b)
    {
        if (a is null) return b is null;
        if (b is null) return false;
        return a.Type == b.Type;
    }

    public static bool operator !=(WindTile? a, WindTile? b)
        => !(a == b);

    public override bool Equals(object? o)
    {
        if (o is null) return false;
        if (o is not WindTile tile) return false;
        return tile == this;
    }

    public override int GetHashCode()
    {
        return Type.GetHashCode();
    }
}

public enum WindType
{
    North,
    West,
    South,
    East
}
