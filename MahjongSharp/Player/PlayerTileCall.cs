using MahjongSharp.Helper;
using MahjongSharp.Tile;

namespace MahjongSharp.Player;

public record PlayerTileCall
{
    public bool IsOpen { set; get; }
    public InteruptionCall Type { set; get; }
    public int? PlayerSource { set; get; }

    public IEnumerable<ATile> Tiles { set; get; }
}