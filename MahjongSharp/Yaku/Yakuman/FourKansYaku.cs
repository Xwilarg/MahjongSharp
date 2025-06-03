using MahjongSharp.Tile;

namespace MahjongSharp.Yaku;

public class FourKansYaku : AYaku
{
    public override bool DoesMatch(IEnumerable<ATile> tiles)
    {
        var numTiles = tiles.Select(x => x as NumberedTile);
        if (numTiles.Any(x => x == null)) return false; // Contains a tile that isn't a numbered tile

        var first = numTiles.First();
        if (!numTiles.Skip(1).Any(x => x.Type != first.Type)) return false; // All tiles aren't the same suit

        return
            numTiles.Count(x => x.Number == 1) >= 3 &&
            numTiles.Count(x => x.Number == 9) >= 3 &&
            numTiles.Any(x => x.Number == 2) &&
            numTiles.Any(x => x.Number == 3) &&
            numTiles.Any(x => x.Number == 4) &&
            numTiles.Any(x => x.Number == 5) &&
            numTiles.Any(x => x.Number == 6) &&
            numTiles.Any(x => x.Number == 7) &&
            numTiles.Any(x => x.Number == 8);
    }

    public override bool ClosedOnly => false;
}