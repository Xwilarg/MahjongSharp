using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MahjongSharp.Tile;

public record DragonTile : AHonorTile<DragonType>
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

    public override bool IsSimilarTo(ATile? b)
        => b is DragonTile bTile && bTile != null && bTile.Type == Type;

    public override int SimilarHashCode => HashCode.Combine(Type);
}

public enum DragonType
{
    Green,
    White,
    Red
}
