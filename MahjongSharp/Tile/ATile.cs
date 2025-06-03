namespace MahjongSharp.Tile;

public abstract class ATile
{
    public abstract bool IsDora(ATile tile);

    public static bool operator ==(ATile a, ATile b)
        => a.Equals(b);

    public static bool operator !=(ATile a, ATile b)
        => !(a == b);
}