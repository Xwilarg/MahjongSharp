namespace MahjongSharp.Tile;

public record NumberedTile : ATile
{
    public NumberedTile(int number, NumberedTileType type, bool isAkaDora)
    {
        Number = number;
        Type = type;
        IsAkaDora = isAkaDora;
    }

    public override bool IsDora(ATile tile)
    {
        if (tile is not NumberedTile numTile || numTile.Type != Type) return false;

        if (numTile.Number == 9) return Number == 1;
        return numTile.Number == Number + 1;
    }

    public override string GetUnicode()
    {
        return Type switch
        {
            NumberedTileType.Bamboo =>
                Number switch
                {
                    1 => "🀐",
                    2 => "🀑",
                    3 => "🀒",
                    4 => "🀓",
                    5 => "🀔",
                    6 => "🀕",
                    7 => "🀖",
                    8 => "🀗",
                    9 => "🀘",
                    _ => string.Empty
                },
            NumberedTileType.Circle =>
                Number switch
                {
                    1 => "🀙",
                    2 => "🀚",
                    3 => "🀛",
                    4 => "🀜",
                    5 => "🀝",
                    6 => "🀞",
                    7 => "🀟",
                    8 => "🀠",
                    9 => "🀡",
                    _ => string.Empty
                },
            NumberedTileType.Kanji =>
                Number switch
                {
                    1 => "🀇",
                    2 => "🀈",
                    3 => "🀉",
                    4 => "🀊",
                    5 => "🀋",
                    6 => "🀌",
                    7 => "🀍",
                    8 => "🀎",
                    9 => "🀏",
                    _ => string.Empty
                },
            _ => string.Empty
        };
    }

    public int Number { private set; get; }
    public NumberedTileType Type { private set; get; }
    public bool IsAkaDora { private set; get; }

    public override bool IsSimilarTo(ATile? b)
        => b is NumberedTile bTile && bTile != null && bTile.Type == Type && bTile.Number == Number;

    public override int SimilarHashCode => HashCode.Combine(Type, Number);
}

public enum NumberedTileType
{
    Kanji,
    Bamboo,
    Circle
}