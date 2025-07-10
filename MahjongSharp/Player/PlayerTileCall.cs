using MahjongSharp.Helper;
using MahjongSharp.Tile;

namespace MahjongSharp.Player;

public record PlayerTileCall
{
    /// <summary>
    /// Is the call open (tile taken from another player)
    /// Or closed (all tiles are from the player')
    /// </summary>
    public bool IsOpen { set; get; }
    /// <summary>
    /// Which call was done
    /// </summary>
    public InteruptionCall Type { set; get; }
    /// <summary>
    /// If the call was open, which player tile is from another player
    /// This also indicate which player threw it (0 mean player before, 1 is player in front and 2/3 is player after)
    /// If tiles are shown, this also indicate which is tilted
    /// </summary>
    public int? PlayerSource { set; get; }
    /// <summary>
    /// Is the current call a pon that was turned into a kan
    /// When this happen, the n+1 tile is also tilted
    /// For example for PlayerSource = 1, tile at index 0 is straight, tiles at indexes 2 and 3 are on top of each other and 3 is straight
    /// </summary>
    public bool IsKanAdded { set; get; }

    public IEnumerable<ATile> Tiles { set; get; }
}