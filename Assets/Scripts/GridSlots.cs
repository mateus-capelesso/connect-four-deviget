using System;
using UnityEngine;
using UnityEngine.Events;

public class GridSlots : MonoBehaviour
{
    public static readonly int Columns = 7;
    public static readonly int Rows = 6;

    public GameObject slotPrefab;
    public Transform slotHolders;
    public static Action<Grid> OnGridInstantiated;
    public RectTransform slotAnimationReference;

    private Grid _grid;
    public Slot[,] SlotGrid => _grid.Slots;
    
    public void InstantiateGrid()
    {
        _grid = new Grid(Rows, Columns);
        var grid = new Slot[Rows, Columns];
        
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                var slotObject = Instantiate(slotPrefab, slotHolders);
                var slot = slotObject.GetComponent<Slot>();
                slot.SlotContent = SlotContent.Void;
                slot.referenceY = slotAnimationReference.anchoredPosition.y;
                grid[i, j] = slot;
            }
        }

        _grid.Slots = grid;
        OnGridInstantiated?.Invoke(_grid);
    }

    public void Clear()
    {
        foreach (var slot in _grid.Slots)
        {
            Destroy(slot.gameObject);
        }
        
        InstantiateGrid();
    }
}
