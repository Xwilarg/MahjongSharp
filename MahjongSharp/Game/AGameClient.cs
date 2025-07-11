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
        _currentTurn++;
        if (_currentTurn == _players.Length) _currentTurn = 0;

        if (_players.Length == 0) throw new InvalidGameState("There is no player registered, please call SetPlayers first");

        var tile = _wall.GetTile();

        UpdatePlayerStatus(_currentTurn, tile);

        DiscardCurrentPlayer();
    }

    internal void Interupt(AGamePlayer player, Mentsu call, IEnumerable<ATile> tiles)
    {
        var discard = _players[_currentTurn].RemoveLastDiscard();
        UpdatePlayerStatus(_currentTurn, null);

        var turnIndex = Array.IndexOf(_players, player);
        int relativeIndex;
        if ((turnIndex == 0 && _currentTurn == _players.Length - 1) || turnIndex == _currentTurn - 1) relativeIndex = 0; // Player before
        else if ((turnIndex == _players.Length - 1 && _currentTurn == 0) || turnIndex == _currentTurn - 1) relativeIndex = _players.Length - 2; // Player after
        else turnIndex = 1; // Player in front

        player.MakeOpenCall(call, tiles, discard, turnIndex);

        _currentTurn = turnIndex;

        DiscardCurrentPlayer();
    }

    private void DiscardCurrentPlayer()
    {
        var currPlayer = _players[_currentTurn];
        var discardTile = currPlayer.GetDiscard(null);
        currPlayer.Discard.Push(discardTile);

        UpdatePlayerStatus(_currentTurn, null);
    }

    public Dictionary<AGamePlayer, Naki> GetPossibleInteruptions(out ATile lastDiscard)
    {
        if (_players.Length == 0) throw new InvalidGameState("There is no player registered, please call SetPlayers first");

        lastDiscard = _players[_currentTurn].LastDiscarded;
        if (lastDiscard == null) throw new InvalidGameState("There is no available discard on the last player");

        Dictionary<AGamePlayer, Naki> interuptions = [];
        for (int i = 0; i < _players.Length; i++)
        {
            if (i == _currentTurn) continue; // Player can't interupt himself

            // Get possibles calls
            // We can only chii if the tile is thrown by the player before us
            var interupt = _players[i].GetPossibleClosedInteruptions(lastDiscard);

            // We can only chii if the tile is from the player before us
            var canChii = i == _currentTurn - 1 || (i == 0 && _currentTurn == _players.Length - 1);
            if (!canChii) interupt &= ~Naki.Chii;


            if (interupt != Naki.None)
            {
                interuptions.Add(_players[i], interupt);
            }
        }
        return interuptions;
    }
}
