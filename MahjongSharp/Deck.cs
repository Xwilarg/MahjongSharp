using MahjongSharp.Tile;
using System.Collections.Generic;
using System.Linq;

namespace MahjongSharp;

public class DeckManager
{
    private readonly List<ATile> _tiles = new();

    public DeckManager()
    {
        for (int c = 0; c < 4; c++)
        {
            // Numbered tiles
            for (int i = 1; i <= 9; i++)
            {
                foreach (var type in Enum.GetValues(typeof(NumberedTileType)).Cast<NumberedTileType>())
                {
                    _tiles.Add(new NumberedTile(i, type, i == 5 && c == 0));
                }
            }

                // Honor tiles
            foreach (var type in Enum.GetValues(typeof(HonorType)).Cast<HonorType>())
            {
                _tiles.Add(new HonorTile(type, false));
            }
        }

        // Shuffle deck
        //_tiles = _tiles.OrderBy(_ => Random.value).ToList();
    }
}