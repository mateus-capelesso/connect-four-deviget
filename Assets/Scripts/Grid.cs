using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid
{
    public Slot[,] Slots;
    public int Rows;
    public int Columns;
    private List<Slot> _winnerSlots;

    public Grid(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        // Slots = new Slot[Rows, Columns];
    }

    public List<Slot> WinnerSlots => _winnerSlots;
    
    public bool WinDetected()
    {
        _winnerSlots = new List<Slot>();
        // Horizontal Check
        for (var row = 0; row < Rows; row++)
        {
            for (var column = 0; column < Columns - 3; column++)
            {
                if (Slots[row, column].SlotContent == SlotContent.Void) continue;
                
                var horizontal = true;
                for (var i = 0; i < 4; i++)
                {
                    _winnerSlots.Add(Slots[row, column + i]);
                    if (Slots[row, column].SlotContent == Slots[row, column + i].SlotContent)
                        continue;
                    
                    horizontal = false;
                    break;
                }

                if (horizontal) return true;
                _winnerSlots.Clear();
            }
        }
        
        // Vertical Check
        for (var row = 0; row < Rows - 3; row++)
        {
            for (var column = 0; column < Columns; column++)
            {
                if (Slots[row, column].SlotContent == SlotContent.Void) continue;
                
                var vertical = true;
                for (var i = 0; i < 4; i++)
                {
                    _winnerSlots.Add(Slots[row + i, column]);
                    if (Slots[row, column].SlotContent == Slots[row + i, column].SlotContent) 
                        continue;
                    
                    vertical = false;
                    break;
                }

                if (vertical) return true;
                _winnerSlots.Clear();
            }
        }
        
        
        for (var column = 0; column < Columns - 3; column++)
        {
            // Ascending Diagonal Check
            for (var row = 0; row < Rows - 3; row++)
            {
                if (Slots[row, column].SlotContent == SlotContent.Void) continue;
                
                var ascendingDiagonal = true;
                for (var i = 0; i < 4; i++)
                {
                    _winnerSlots.Add(Slots[row + i, column + i]);
                    if (Slots[row, column].SlotContent == Slots[row + i, column + i].SlotContent)
                        continue;
                    
                    ascendingDiagonal = false;
                    break;
                }

                if (ascendingDiagonal) return true;
                _winnerSlots.Clear();
            }
            
            // Descending Diagonal Check
            for (var row = 3; row < Rows; row++)
            {
                if (Slots[row, column].SlotContent == SlotContent.Void) continue;
                
                var descendingDiagonal = true;
                for (var i = 0; i < 4; i++)
                {
                    _winnerSlots.Add(Slots[row - i, column + i]);
                    if (Slots[row, column].SlotContent == Slots[row - i, column + i].SlotContent)
                        continue;
                    
                    descendingDiagonal = false;
                    break;
                }

                if (descendingDiagonal) return true;
                _winnerSlots.Clear();
            }
        }

        return false;
    }

    public List<int> GetAvailableColumns()
    {
        if (IsFull()) return null;
        
        var list = new List<int>();
        var rows = Rows;
        for (var i = 0; i < Columns; i++)
        {
            if(Slots[rows - 1, i].SlotContent == SlotContent.Void)
                list.Add(i);
        }

        return list;
    }

    public List<int> GetOptimalColumns(SlotContent content)
    {
        var list = new List<int>();

        for (var row = 0; row < Rows; row++)
        {
            for (var column = 0; column < Columns; column++)
            {
                if (Slots[row, column].SlotContent != content) continue;
                
                var available = GetAvailableSurroundings(row, column);
                list.AddRange(available);

            }
        }

        return list;
    }

    private List<int> GetAvailableSurroundings(int row, int column)
    {
        var list = new List<int>();
        for (var i = row - 1; i <= row + 1; i++)
        {
            for (var j = column - 1; j <= column + 1; j++)
            {
                if (j < 0 || j >= Columns) continue;
                if (i < 0 || i >= Rows) continue;


                if (Slots[i, j].SlotContent != SlotContent.Void) continue;
                
                if (!list.Contains(j)) list.Add(j);
            }
        }

        return list;
    }

    /// <summary>
    /// Check upper row on every column, to see if one of them is empty
    /// </summary>
    public bool IsFull()
    {
        var rows = Rows - 1;
        for (var i = 0; i < Columns; i++)
        {
            if (Slots[rows, i].SlotContent == SlotContent.Void)
                return false;
        }

        return true;
    }
    

}
