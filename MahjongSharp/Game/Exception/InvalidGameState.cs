namespace MahjongSharp.Game.Exception;

public class InvalidGameState : System.Exception
{
    public InvalidGameState(string message) : base(message)
    { }
}