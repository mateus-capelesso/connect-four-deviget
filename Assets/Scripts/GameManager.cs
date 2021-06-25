using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<SlotContent> players;
    
    private Slot[,] _gameGrid;
    private int _activePlayer;

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
            
            slot.SlotContent = players[_activePlayer];
            break;
        }

        TogglePlayer();
    }

    private void TogglePlayer()
    {
        _activePlayer++;
        if (_activePlayer >= players.Count)
            _activePlayer = 0;
    }
}
