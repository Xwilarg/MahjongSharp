namespace MahjongSharp.Tile;

public abstract class ATile
{
    protected ATile(bool isAkaDora)
    {
        _isAkaDora = isAkaDora;
    }

    protected bool _isAkaDora;

    public abstract bool IsDora(ATile tile);
}