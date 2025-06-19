namespace MahjongSharp.Tile;

public abstract record AHonorTile<T> : ATile
    where T : Enum
{
    public AHonorTile(T type)
    {
        Type = type;
    }

    public T Type { private set; get; }
}