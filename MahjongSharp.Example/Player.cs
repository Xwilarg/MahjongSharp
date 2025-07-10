using MahjongSharp.Helper;
using MahjongSharp.Tile;
using System.Text;
using MahjongSharp.Game;

internal abstract class ATextPlayer : AGamePlayer
{
    internal ATextPlayer(IEnumerable<ATile> startingTiles) : base(startingTiles, sortAuto: true)
    { }

    protected void ShowDiscard()
    {
        Console.WriteLine("Discard");
        for (int i = 0; i < Discard.Count; i += 6)
        {
            Console.WriteLine(string.Join(" ", Discard.Skip(i).Take(6).Select(x => TileHelper.GetTextNotation([x]))));
        }
    }

    protected void ShowCalls()
    {
        Console.WriteLine("Calls");
        foreach (var c in Calls)
        {
            var text = TileHelper.GetTextNotation(c.Tiles);
            if (c.IsOpen)
            {
                Console.WriteLine($"{text.Take(c.PlayerSource.Value + 1)}'{text.Skip(c.PlayerSource.Value + 1)}");
            }
            else
            {
                Console.WriteLine(text);
            }
        }
    }

    /// <summary>
    /// How the player react to a call possibility
    /// </summary>
    /// <param name="call">All the calls we can do</param>
    /// <param name="tile">Tile that was last discarded, that we interact with</param>
    /// <returns>
    /// A tuple associating the call we do and which tiles we do it with
    /// If we don't do anything, first element of the tuple will be InteruptionCall.None
    /// </returns>
    public abstract (Naki call, IEnumerable<ATile> tiles) GetCallChoice(Naki call, ATile tile);
}

internal class AIPlayer : ATextPlayer
{
    internal AIPlayer(IEnumerable<ATile> startingTiles) : base(startingTiles)
    { }

    public override (Naki call, IEnumerable<ATile> tiles) GetCallChoice(Naki call, ATile tile)
    {
        return (Naki.None, []);
    }

    public override ATile GetDiscard(ATile newTile)
    {
        return newTile;
    }

    /// <inheritdoc/>
    public override void ShowStatus(ATile? newTile)
    {
        Console.WriteLine("========== AI     ===========");
        ShowDiscard();
        ShowCalls();
        Console.WriteLine("Hand");
        Console.Write(string.Join("", Enumerable.Repeat("?", Tiles.Count)));
        if (newTile != null)
        {
            Console.WriteLine(" ?");
        }
        else Console.WriteLine();
    }
}

internal class HumanPlayer : ATextPlayer
{
    internal HumanPlayer(IEnumerable<ATile> startingTiles) : base(startingTiles)
    { }

    public override (Naki call, IEnumerable<ATile> tiles) GetCallChoice(Naki call, ATile tile)
    {
        Console.WriteLine();
        Console.WriteLine($"Tile thrown: {TileHelper.GetTextNotation([tile])}");

        if (call.HasFlag(Naki.Chii)) Console.WriteLine("c: chii");
        if (call.HasFlag(Naki.Pon)) Console.WriteLine("p: pon");
        if (call.HasFlag(Naki.Kan)) Console.WriteLine("k: kan");
        Console.WriteLine("s: Skip");

        ConsoleKeyInfo k;
        while (true)
        {
            k = Console.ReadKey();

            if (k.Key == ConsoleKey.S) return (Naki.None, []);

            IEnumerable<ATile>[] tiles;
            if (call.HasFlag(Naki.Chii) && k.Key == ConsoleKey.C)
            {
                tiles = TileCall.GetChii(Tiles, tile).Select(x => x.Tiles).ToArray();
                return (Naki.Chii, tiles[ShowTileGroups(tiles)]);
            }
            if (call.HasFlag(Naki.Pon) && k.Key == ConsoleKey.P)
            {
                tiles = TileCall.GetPon(Tiles, tile).Select(x => x.Tiles).ToArray();
                return (Naki.Pon, tiles[ShowTileGroups(tiles)]);
            }
            if (call.HasFlag(Naki.Kan) && k.Key == ConsoleKey.K)
            {
                tiles = TileCall.GetKan(Tiles, tile).Select(x => x.Tiles).ToArray();
                return (Naki.Kan, tiles[ShowTileGroups(tiles)]);
            }
        }
    }

    private int ShowTileGroups(IEnumerable<ATile>[] groups)
    {
        for (int i = 0; i < groups.Length; i++)
        {
            Console.WriteLine($"{i}: {TileHelper.GetTextNotation(groups[i])}");
        }

        char key;
        do
        {
            key = char.ToUpperInvariant(Console.ReadKey().KeyChar);
        } while (key < '0' || key >= '0' + groups.Length);

        return key - '0';
    }

    public override ATile GetDiscard(ATile newTile)
    {
        var textNotation = TileHelper.GetTextNotation(Tiles);

        // Hint under the text notation that associate a number/capital letter under each tile
        string hintText = "123456789QWERT";
        StringBuilder numPrev = new(); int c = 0;
        for (int i = 0; i < textNotation.Length; i++)
        {
            if (textNotation[i] >= '0' && textNotation[i] <= '9')
            {
                numPrev.Append(hintText[c]);
                c++;
            }
            else numPrev.Append(' ');
        }

        Console.WriteLine($"{numPrev} 0");
        Console.WriteLine("Enter an index to discard");

        List<char> acceptableInputs = ['0', .. hintText.Take(c)];

        // List of calls we can do
        var calls = GetPossibleClosedInteruptions(newTile);
        if (calls != Naki.None)
        {
            Console.WriteLine("OR"); // Tile calls
            if (calls.HasFlag(Naki.Kan) || calls.HasFlag(Naki.PonToKan)) { Console.WriteLine("K: kan"); acceptableInputs.Add('K'); }
            //Console.WriteLine("r: riichi");
            //Console.WriteLine("t: tsumo");
        }

        char key;
        do
        {
            key = char.ToUpperInvariant(Console.ReadKey().KeyChar);
        } while (!acceptableInputs.Contains(key));

        ATile discard;
        if (key == '0') discard = newTile;
        else if (key == 'K')
        {
            // We ask the player which kan he want to form
            // This can happen if we have multiple possible closed kan available
            // Or if additional to one/many closed kan we can also convert a pon to a kan

            List<IEnumerable<ATile>> tiles = [];
            int index = 0;

            if (calls.HasFlag(Naki.PonToKan)) // Pon that can be changed to a kan
            {
                tiles.Add(TileCall.GetKan(Calls, newTile).Tiles);

            }
            if (calls.HasFlag(Naki.Kan)) // Closed kan
            {
                tiles.AddRange(TileCall.GetKan(Tiles).Select(x => x.Tiles));
            }

            if (tiles.Count > 0)
            {
                index = ShowTileGroups(tiles.ToArray());
            }

            var res = tiles[index];

            if (index == 0 && calls.HasFlag(Naki.PonToKan))
            {
                UpdatePonToKan(newTile);
            }
            else
            {
                MakeCloseCall(Mentsu.Kan, res);
            }

            GameClient.UpdateCurrentPlayerStatus(newTile);
            return GetDiscard(newTile); // We still need to discard a tile
        }
        else discard = Tiles[hintText.IndexOf(key)];

        return discard;
    }

    /// <inheritdoc/>
    public override void ShowStatus(ATile? newTile)
    {
        Console.WriteLine("========== Player ==========");
        ShowDiscard();
        ShowCalls();
        Console.WriteLine("Hand");

        // Display player hand
        var textNotation = TileHelper.GetTextNotation(Tiles);
        Console.Write(textNotation);

        if (newTile == null)
        {
            Console.WriteLine();
        }
        else
        {
            // ... along with the tile we just drew
            Console.WriteLine($" {TileHelper.GetTextNotation([newTile])}");
        }
    }
}