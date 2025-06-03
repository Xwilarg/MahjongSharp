namespace MahjongSharp.Tile;

public abstract class ATile
{
    public abstract bool IsDora(ATile tile);

    public abstract string GetUnicode();

    public static bool operator ==(ATile? a, ATile? b)
    {
        if (a is null) return b is null;
        if (b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(ATile? a, ATile? b)
        => !(a == b);
}