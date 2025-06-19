namespace MahjongSharp.Tile;

public record WindTile : AHonorTile<WindType>
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

    public override bool IsSimilarTo(ATile? b)
        => b is WindTile bTile && bTile != null && bTile.Type == Type;

    public override int SimilarHashCode => HashCode.Combine(Type);
}

public enum WindType
{
    North,
    West,
    South,
    East
}
