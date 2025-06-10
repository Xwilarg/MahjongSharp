using System.Text;
using MahjongSharp.Tile;

namespace MahjongSharp.Game;

public class PlayerHand
{
    public IList<ATile> Tiles { private set; get; }
    private IList<ATile[]> _kans;

    public PlayerHand(IEnumerable<ATile> startingTiles)
    {
        Tiles = startingTiles.ToList();
    }

    public void AddTile(ATile tile)
    {
        Tiles.Add(tile);
    }

    public string GetTextNotation()
    {
        if (!Tiles.Any()) return string.Empty;

        StringBuilder str = new();
        char? last = null;
        foreach (var t in Tiles)
        {
            char curr;
            string toAdd;
            if (t is NumberedTile numTile)
            {
                curr = numTile.Type switch
                {
                    NumberedTileType.Bamboo => 's',
                    NumberedTileType.Circle => 'p',
                    NumberedTileType.Kanji => 'm',
                    _ => '?'
                };
                toAdd = numTile.Number.ToString();
            }
            else if (t is WindTile wind)
            {
                curr = 'z';
                toAdd = wind.Type switch
                {
                    WindType.East => "1",
                    WindType.South => "2",
                    WindType.West => "3",
                    WindType.North => "4",
                    _ => "?"
                };
            }
            else if (t is DragonTile dragon)
            {
                curr = 'z';
                toAdd = dragon.Type switch
                {
                    DragonType.White => "5",
                    DragonType.Green => "6",
                    DragonType.Red => "7",
                    _ => "?"
                };
            }
            else
            {
                toAdd = string.Empty;
                curr = '?';
            }
            if (last == null)
            {
                last = curr;
            }
            else if (curr != last)
            {
                str.Append(last);
                last = curr;
            }
            str.Append(toAdd);
        }
        str.Append(last);
        return str.ToString();
    }

    public void SortHand()
    {
        Tiles = Tiles.OrderBy(x =>
        {
            if (x is NumberedTile numTile)
            {
                return 000 + (10 * (int)numTile.Type) + numTile.Number;
            }
            else if (x is WindTile wind)
            {
                return 100 + (int)wind.Type;
            }
            else if (x is DragonTile dragon)
            {
                return 200 + (int)dragon.Type;
            }
            else return 999;
        }).ToList();
    }
}