using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public List<Player> players;
    
    private Grid _gameGrid;
    private int _activePlayer;
    private bool _gameOver;

    public static Action OnGridFilled;
    public static Action OnPlayerTurn;
    public static Action OnAIPickColumn;
    public static Action OnDisableButton;
    public static Action<List<Slot>> OnWinDetected;
    public static Action<Player> OnWinnerDetected;

    private void Start()
    {
        AssignEvents();
    }

    public void SelectFirstPlayer()
    {
        _activePlayer = 0;
        if(players[_activePlayer].automaticInput)
            OnAIPickColumn?.Invoke();
        else
            OnPlayerTurn?.Invoke();
    }

    private void AssignEvents()
    {
        GridSlots.OnGridInstantiated += ReceivesSlotGrid;
        ColumnButtons.OnColumnButtonPressed += ColumnSelected;
        OnWinDetected += GameOver;
    }

    private void ReceivesSlotGrid(Grid grid)
    {
        _gameGrid = grid;
    }

    private void ColumnSelected(int columnIndex)
    {
        if (_gameOver) return;

        if (_gameGrid.IsFull())
        {
            // Whole grid is filled, call draw UI and game over 
            OnGridFilled?.Invoke();
            return;
        }

        if (_gameGrid.ColumnIsFull(columnIndex)) return;

        // Disable buttons to avoid input errors
        OnDisableButton?.Invoke();
        OnAIPickColumn?.Invoke();

        for (var i = 0; i < _gameGrid.Rows; i++)
        {
            var slot = _gameGrid.Slots[i, columnIndex];
            if (slot.SlotContent != SlotContent.Void) continue;

            slot.SetSlotContent(players[_activePlayer], () =>
            {
                // Waits for animation to be over
                if(!GetWinState(_gameGrid))
                    TogglePlayer();
            });
            break;
        }
        
        
    }

    private bool GetWinState(Grid grid)
    {
        var win = grid.WinDetected();

        if (!win) return false;
        OnWinDetected?.Invoke(_gameGrid.WinnerSlots);
        OnWinnerDetected?.Invoke(players[_activePlayer]);

        return true;
    }

    private void TogglePlayer()
    {
        if (_gameOver) return;

        _activePlayer++;
        if (_activePlayer >= players.Count)
            _activePlayer = 0;

        if (players[_activePlayer].automaticInput)
        {
            StartCoroutine(PickColumn());
        }
        else
            OnPlayerTurn?.Invoke();    
    }

    private IEnumerator PickColumn()
    {
        // Add delay to not feel too automatic
        yield return new WaitForSeconds(1f);
        
        
        var possibleColumns = _gameGrid.GetOptimalColumns(players[_activePlayer].content);
        if (possibleColumns.Count <= 0)
            possibleColumns = _gameGrid.GetAvailableColumns();
        
        var index = Random.Range(0, possibleColumns.Count);
        
        ColumnSelected(possibleColumns.ToList()[index]);
        
    }

    private void GameOver(List<Slot> slots)
    {
        _gameOver = true;
        OnWinnerDetected?.Invoke(players[_activePlayer]);
    }

    public void Clear()
    {
        
        _gameOver = false;
        DOVirtual.DelayedCall(0.5f, SelectFirstPlayer);
    }
        
}
