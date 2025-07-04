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
        for (int i = 0; i < _hand.Discard.Count; i += 6)
        {
            Console.WriteLine(string.Join(" ", _hand.Discard.Skip(i).Take(6).Select(x => TileHelper.GetTextNotation([x]))));
        }
    }

    internal void DiscardTileFromHand(ATile tile)
    {
        _hand.DiscardTileFromHand(tile);
        _hand.SortHand();
    }
}

internal class AIPlayer : ATextPlayer
{
    internal AIPlayer(IEnumerable<ATile> startingTiles) : base(startingTiles)
    { }

    public override ATile GetDiscard(ATile newTile)
    {
        return newTile;
    }

    /// <inheritdoc/>
    public override void ShowStatus(ATile? newTile)
    {
        Console.WriteLine("AI");
        ShowDiscard();
        Console.Write(string.Join("", Enumerable.Repeat("?", _hand.Tiles.Count)));
        if (newTile != null)
        {
            Console.WriteLine(" ?");
            _hand.AddTile(newTile);
        }
        else Console.WriteLine();
    }
}

internal class HumanPlayer : ATextPlayer
{
    internal HumanPlayer(IEnumerable<ATile> startingTiles) : base(startingTiles)
    { }

    internal bool GetPlayerInteruption(InteruptionCall call, ATile tile)
    {
        Console.WriteLine($"Tile thrown: {TileHelper.GetTextNotation([tile])}");

        if (call.HasFlag(InteruptionCall.Chii)) Console.WriteLine("c: chii");
        if (call.HasFlag(InteruptionCall.Pon)) Console.WriteLine("p: pon");
        if (call.HasFlag(InteruptionCall.Kan)) Console.WriteLine("k: kan");
        Console.WriteLine("s: Skip");

        while (true)
        {
            var k = Console.ReadKey();

            if (k.Key == ConsoleKey.S) return false;
        }
    }

    public override ATile GetDiscard(ATile newTile)
    {
        return newTile;
    }

    /// <inheritdoc/>
    internal override void ShowStatus(ATile? newTile)
    {
        Console.WriteLine("Player");
        ShowDiscard();

        // Display player hand
        var textNotation = TileHelper.GetTextNotation(_hand.Tiles);
        Console.Write(textNotation);

        if (newTile == null)
        {
            Console.WriteLine();
            return null;
        }

        // ... along with the tile we just drew
        Console.WriteLine($" {TileHelper.GetTextNotation([newTile])}");

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

        var calls = _hand.GetPossibleInteruptions(newTile);
        if (calls != InteruptionCall.None)
        {
            Console.WriteLine("OR"); // Tile calls
            if (calls.HasFlag(InteruptionCall.Chii)) { Console.WriteLine("C: chii"); acceptableInputs.Add('C'); }
            if (calls.HasFlag(InteruptionCall.Pon)) { Console.WriteLine("P: pon"); acceptableInputs.Add('P'); }
            if (calls.HasFlag(InteruptionCall.Kan)) { Console.WriteLine("K: kan"); acceptableInputs.Add('K'); }
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
        if (key == 'C')
        {
            return discard;
            var possibleChii = TileCall.GetChii(_hand.Tiles, newTile);
            //_hand.MakeOpenCall(InteruptionCall.Chii, _hand.Tiles, with, 
        }
        else discard = _hand.Tiles[hintText.IndexOf(key)];

        _hand.AddTile(newTile);

        return discard;
    }
}