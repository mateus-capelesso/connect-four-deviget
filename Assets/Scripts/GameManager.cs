using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public List<Player> players;
    
    private Slot[,] _gameGrid;
    private int _activePlayer;

    public static Action OnGridFilled; 

    private void Start()
    {
        _activePlayer = 0;
        AssignEvents();
    }

    private void AssignEvents()
    {
        GridSlots.OnGridInstantiated += ReceivesSlotGrid;
        ColumnButtons.OnColumnButtonPressed += ColumnSelected;
    }

    private void ReceivesSlotGrid(Slot[,] grid)
    {
        _gameGrid = grid;
    }

    private void ColumnSelected(int columnIndex)
    {
        for (var i = 0; i < GridSlots.Rows; i++)
        {
            var slot = _gameGrid[i, columnIndex];
            if (slot.SlotContent != SlotContent.Void) continue;
            
            slot.SlotContent = players[_activePlayer].content;
            break;
        }

        TogglePlayer();
    }

    public List<int> AvailableColumns()
    {
        var list = new List<int>();
        var rows = GridSlots.Rows;
        for (var i = 0; i < GridSlots.Columns; i++)
        {
            if(_gameGrid[rows - 1, i].SlotContent == SlotContent.Void)
                list.Add(i);
        }

        if (list.Count != 0) return list;
        
        // Whole grid is filled, call UI and game over
        OnGridFilled?.Invoke();
        return null;

    }

    private void TogglePlayer()
    {
        _activePlayer++;
        if (_activePlayer >= players.Count)
            _activePlayer = 0;

        if (players[_activePlayer].automaticInput)
            StartCoroutine(PickColumn());
    }

    private IEnumerator PickColumn()
    {
        // Add delay to not feel too automatic
        yield return new WaitForSeconds(0.5f);
        var availableColumns = AvailableColumns();
        var index = Random.Range(0, availableColumns.Count);
        ColumnSelected(availableColumns[index]);
    }
}
