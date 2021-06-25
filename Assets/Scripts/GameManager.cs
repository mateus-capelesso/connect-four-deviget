using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Slot[,] _gameGrid;
    
    private void Start()
    {
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
            
            slot.SlotContent = SlotContent.Red;
            break;
        }
    }
}
