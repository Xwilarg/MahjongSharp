namespace MahjongSharp.Tile;

public abstract record ATile
{
    public abstract bool IsDora(ATile tile);

    public abstract string GetUnicode();

    public abstract bool IsSimilarTo(ATile? b);
    public abstract int SimilarHashCode { get; }
}