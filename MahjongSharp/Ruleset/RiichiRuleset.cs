using MahjongSharp.Tile;

namespace MahjongSharp.Ruleset;

public class RiichiRuleset : ARuleset
{
    public override int DeadWallSize => 14;
    public override int HandSize => 14;

    public override IEnumerable<ATile> GetAllTiles()
    {
        for (int c = 0; c < 4; c++)
        {
            // Numbered tiles
            for (int i = 1; i <= 9; i++)
            {
                foreach (var type in Enum.GetValues(typeof(NumberedTileType)).Cast<NumberedTileType>())
                {
                    yield return new NumberedTile(i, type, i == 5 && c == 0);
                }
            }

                // Honor tiles
            foreach (var type in Enum.GetValues(typeof(HonorType)).Cast<HonorType>())
            {
                yield return new HonorTile(type, false);
            }
        }
    }
}