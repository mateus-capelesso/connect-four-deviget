using System;
using UnityEngine;
using UnityEngine.Events;

public class GridSlots : MonoBehaviour
{
    public static readonly int Columns = 7;
    public static readonly int Rows = 6;

    public GameObject slotPrefab;
    public Transform slotHolders;
    public static Action<Slot[,]> OnGridInstantiated;

    private Slot[,] _slotGrid;

    private void Start()
    {
        InstantiateGrid();
    }

    public void InstantiateGrid()
    {
        // Create 2D array and clear it
        _slotGrid = new Slot[Rows, Columns];
        Array.Clear(_slotGrid, 0, _slotGrid.Length);
        
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                var slotObject = Instantiate(slotPrefab, slotHolders);
                var slot = slotObject.GetComponent<Slot>();
                slot.SlotContent = SlotContent.Void;
                _slotGrid[i, j] = slot;
            }
        }
        
        OnGridInstantiated?.Invoke(_slotGrid);
    }

    public void Clear()
    {
        foreach (var slot in _slotGrid)
        {
            slot.Clear();
        }
    }
}
