using System;

namespace MahjongSharp.Tile;

public class HonorTile : ATile
{
    public HonorTile(HonorType type, bool isAkaDora) : base(isAkaDora)
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

    public HonorType Type { private set; get; }
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