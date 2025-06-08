namespace MahjongSharp.Tile;

public abstract class AHonorTile<T> : ATile
    where T : Enum
{
    public AHonorTile(T type)
    {
        Type = type;
    }

    public T Type { private set; get; }
}