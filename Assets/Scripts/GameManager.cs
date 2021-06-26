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
    private bool _gameOver;

    public static Action OnGridFilled;
    public static Action OnEnableButtons;
    public static Action OnDisableButton;
    public static Action<List<Slot>> OnWinDetected;

    private void Start()
    {
        _activePlayer = 0;
        AssignEvents();
    }

    private void AssignEvents()
    {
        GridSlots.OnGridInstantiated += ReceivesSlotGrid;
        ColumnButtons.OnColumnButtonPressed += ColumnSelected;
        OnWinDetected += GameOver;
    }

    private void ReceivesSlotGrid(Slot[,] grid)
    {
        _gameGrid = grid;
    }

    private void ColumnSelected(int columnIndex)
    {
        if (_gameOver) return;
        
        // Disable buttons to avoid input errors
        OnDisableButton?.Invoke();

        for (var i = 0; i < GridSlots.Rows; i++)
        {
            var slot = _gameGrid[i, columnIndex];
            if (slot.SlotContent != SlotContent.Void) continue;
            
            slot.SlotContent = players[_activePlayer].content;
            break;
        }
        
        CheckWinCondition();
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
        
        // Whole grid is filled, call draw UI and game over 
        OnGridFilled?.Invoke();
        return null;
    }

    private void TogglePlayer()
    {
        if (_gameOver) return;
        
        _activePlayer++;
        if (_activePlayer >= players.Count)
            _activePlayer = 0;

        if (players[_activePlayer].automaticInput)
            StartCoroutine(PickColumn());
        else
            OnEnableButtons?.Invoke();    
    }

    private IEnumerator PickColumn()
    {
        // Add delay to not feel too automatic
        yield return new WaitForSeconds(0.5f);
        var availableColumns = AvailableColumns();
        var index = Random.Range(0, availableColumns.Count);
        ColumnSelected(availableColumns[index]);
    }

    public void GameOver(List<Slot> slots)
    {
        _gameOver = true;
    }

    private void CheckWinCondition()
    {
        var list = new List<Slot>();
        
        // Horizontal Check
        for (var row = 0; row < GridSlots.Rows; row++)
        {
            for (var column = 0; column < GridSlots.Columns - 3; column++)
            {
                if (_gameGrid[row, column].SlotContent == SlotContent.Void) continue;
                
                var horizontal = true;
                for (var i = 0; i < 4; i++)
                {
                    list.Add(_gameGrid[row, column + i]);
                    if (_gameGrid[row, column].SlotContent == _gameGrid[row, column + i].SlotContent)
                        continue;
                    
                    horizontal = false;
                    break;
                }

                if (!horizontal)
                {
                    list.Clear();
                    continue;
                }
                
                Debug.Log($"Win horizontal {_gameGrid[row, column].SlotContent.ToString()}");
                OnWinDetected?.Invoke(list);
                return;


            }
        }
        
        // Vertical Check
        for (var row = 0; row < GridSlots.Rows - 3; row++)
        {
            for (var column = 0; column < GridSlots.Columns; column++)
            {
                if (_gameGrid[row, column].SlotContent == SlotContent.Void) continue;
                
                var vertical = true;
                for (var i = 0; i < 4; i++)
                {
                    list.Add(_gameGrid[row + i, column]);
                    if (_gameGrid[row, column].SlotContent == _gameGrid[row + i, column].SlotContent) 
                        continue;
                    
                    vertical = false;
                    break;
                }

                if (!vertical)
                {
                    list.Clear();
                    continue;
                }
                
                Debug.Log($"Win vertical {_gameGrid[row, column].SlotContent.ToString()}");
                OnWinDetected?.Invoke(list);
                return;
            }
        }
        
        
        for (var column = 0; column < GridSlots.Columns - 3; column++)
        {
            // Ascending Diagonal Check
            for (var row = 0; row < GridSlots.Rows - 3; row++)
            {
                if (_gameGrid[row, column].SlotContent == SlotContent.Void) continue;
                
                var ascendingDiagonal = true;
                for (var i = 0; i < 4; i++)
                {
                    list.Add(_gameGrid[row + i, column + i]);
                    if (_gameGrid[row, column].SlotContent == _gameGrid[row + i, column + i].SlotContent)
                        continue;
                    
                    ascendingDiagonal = false;
                    break;
                }

                if (!ascendingDiagonal)
                {
                    list.Clear();
                    continue;
                }
                
                Debug.Log($"Win ascending vertical {_gameGrid[row, column].SlotContent.ToString()}");
                OnWinDetected?.Invoke(list);
                return;
            }
            
            // Descending Diagonal Check
            for (var row = 3; row < GridSlots.Rows; row++)
            {
                if (_gameGrid[row, column].SlotContent == SlotContent.Void) continue;
                
                var descendingDiagonal = true;
                for (var i = 0; i < 4; i++)
                {
                    list.Add(_gameGrid[row - i, column + i]);
                    if (_gameGrid[row, column].SlotContent == _gameGrid[row - i, column + i].SlotContent)
                        continue;
                    
                    descendingDiagonal = false;
                    break;
                }

                if (!descendingDiagonal)
                {
                    list.Clear();
                    continue;
                }
                
                Debug.Log($"Win descending vertical {_gameGrid[row, column].SlotContent.ToString()}");
                OnWinDetected?.Invoke(list);
                return;
            }
        }
    }

    public void Clear()
    {
        _activePlayer = 0;
        _gameOver = false;
    }
}
