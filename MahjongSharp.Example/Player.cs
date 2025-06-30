using MahjongSharp.Game;
using MahjongSharp.Helper;
using MahjongSharp.Tile;
using System.Text;

internal abstract class APlayer
{
    internal APlayer(IEnumerable<ATile> startingTiles)
    {
        _hand = new PlayerHand(startingTiles);
        _hand.SortHand();
    }

    protected PlayerHand _hand;

    /// <summary>
    /// Show the hand in the console
    /// </summary>
    /// <param name="newTile">Tile we just drew, null if it's not our turn</param>
    /// <returns>Tile to be discarded, null if not applicable</returns>
    internal abstract ATile? ShowStatus(ATile? newTile);

    internal InteruptionCall GetPossibleInteruptions(ATile tile)
    {
        InteruptionCall call = InteruptionCall.None;
        if (TileCall.CanChii(_hand.Tiles, tile)) call |= InteruptionCall.Chii;
        if (TileCall.CanPon(_hand.Tiles, tile)) call |= InteruptionCall.Pon;
        if (TileCall.CanKan(_hand.Tiles, tile)) call |= InteruptionCall.Kan;

        return call;
    }

    internal void DiscardTileFromHand(ATile tile)
    {
        _hand.DiscardTileFromHand(tile);
        _hand.SortHand();
    }
}

internal class AIPlayer : APlayer
{
    internal AIPlayer(IEnumerable<ATile> startingTiles) : base(startingTiles)
    { }

    /// <inheritdoc/>
    internal override ATile? ShowStatus(ATile? newTile)
    {
        Console.WriteLine("AI");
        Console.Write(string.Join("", Enumerable.Repeat("?", _hand.Tiles.Count)));
        if (newTile != null)
        {
            Console.WriteLine(" ?");
            _hand.AddTile(newTile);
            return _hand.Tiles[0];
        }
        Console.WriteLine();
        return null;
    }
}

internal class HumanPlayer : APlayer
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

    /// <inheritdoc/>
    internal override ATile? ShowStatus(ATile? newTile)
    {
        Console.WriteLine("Player");

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
        string hintText = "123456789ABCED";
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
        c--;

        Console.WriteLine($"{numPrev} 0");
        Console.WriteLine("Enter an index to discard");

        var canChii = TileCall.CanChii(_hand.Tiles, newTile);
        var canPon = TileCall.CanPon(_hand.Tiles, newTile);
        var canKan = TileCall.CanKan(_hand.Tiles, newTile);
        if (canChii || canPon || canKan)
        {
            Console.WriteLine("OR"); // Tile calls
            if (canChii) Console.WriteLine("c: chii");
            if (canPon) Console.WriteLine("p: pon");
            if (canKan) Console.WriteLine("k: kan");
            //Console.WriteLine("r: riichi");
            //Console.WriteLine("t: tsumo");
        }

        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey();
        } while (key.KeyChar < '0' && key.KeyChar > hintText[c]);

        ATile discard;
        if (key.KeyChar == '0') discard = newTile;
        else discard = _hand.Tiles[key.KeyChar - '1'];

        _hand.AddTile(newTile);

        return discard;
    }
}

[Flags]
public enum InteruptionCall
{
    None = 0,
    Chii = 1,
    Pon = 2,
    Kan = 4
}