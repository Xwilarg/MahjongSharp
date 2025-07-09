using MahjongSharp.Helper;
using MahjongSharp.Player;
using MahjongSharp.Game.Exception;
using MahjongSharp.Ruleset;
using MahjongSharp.Tile;

namespace MahjongSharp.Game;

public abstract class AGameClient
{
    protected Wall _wall;
    protected ARuleset _ruleset;
    protected AGamePlayer[] _players;

    private int _currentTurn;

    public AGameClient(ARuleset ruleset)
    {
        _ruleset = ruleset;
        _wall = new(ruleset);

        _players = [];
    }

    /// <summary>
    /// Set all players that will participate to the game
    /// </summary>
    /// <remarks>This should be done right after the call to the constructor</remarks>
    public void SetPlayers(AGamePlayer[] players)
    {
        _players = players;

        foreach (var p in _players) p.GameClient = this;
        foreach (var p in _players) p.ShowStatus(null);
    }

    /// <returns>A set of tiles as according to the current ruleset</returns>
    public IEnumerable<ATile> DrawStartingTiles()
    {
        return _wall.GetTiles(_ruleset.HandSize);
    }

    /// <summary>
    /// Called when a player status need to be updated
    /// This mean that the state of the hand of the player changed
    /// </summary>
    /// <param name="index">Which player need to be updated</param>
    /// <param name="tile">Tile that is being drawn</param>
    protected virtual void UpdatePlayerStatus(int index, ATile? tile)
    {
        _players[index].ShowStatus(tile);
    }

    public void UpdateCurrentPlayerStatus(ATile? tile)
        => _players[_currentTurn].ShowStatus(tile);

    public void PlayNextTurn()
    {
        if (_players.Length == 0) throw new InvalidGameState("There is no player registered, please call SetPlayers first");

        var tile = _wall.GetTile();

        UpdatePlayerStatus(_currentTurn, tile);
        var currPlayer = _players[_currentTurn];
        var discardTile = currPlayer.GetDiscard(tile);
        currPlayer.AddTileToHandThenDiscard(tile, discardTile);

        UpdatePlayerStatus(_currentTurn, null);

        _currentTurn++;
        if (_currentTurn == _players.Length) _currentTurn = 0;
    }

    public void Interupt(AGamePlayer player, InteruptionCall call)
    {
        _players[_currentTurn].AddTileToHandThenDiscard
    }

    public Dictionary<AGamePlayer, InteruptionCall> GetPossibleInteruptions()
    {
        if (_players.Length == 0) throw new InvalidGameState("There is no player registered, please call SetPlayers first");

        var lastDiscard = _players[_currentTurn].LastDiscarded;
        if (lastDiscard == null) throw new InvalidGameState("There is no available discard on the last player");

        Dictionary<AGamePlayer, InteruptionCall> interuptions = [];
        for (int i = 0; i < _players.Length; i++)
        {
            if (i == _currentTurn) continue; // Player can't interupt himself

            // Get possibles calls
            // We can only chii if the tile is thrown by the player before us
            var interupt = _players[i].GetPossibleInteruptions(lastDiscard, i == _currentTurn - 1 || (i == 0 && _currentTurn == _players.Length - 1));

            if (interupt != InteruptionCall.None)
            {
                interuptions.Add(_players[i], interupt);
            }
        }
        return interuptions;
    }
}
