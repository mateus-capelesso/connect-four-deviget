using System;
using UnityEngine;
using UnityEngine.UI;

public class ColumnButtons : MonoBehaviour
{
    public GameObject columnButton;

    public static Action<int> OnColumnButtonPressed;
    
    void Start()
    {
        InstantiateColumnButtons();
    }

    private void InstantiateColumnButtons()
    {
        for (var i = 0; i < GridSlots.Columns; i++)
        {
            var column = Instantiate(columnButton, transform);
            column.GetComponent<ColumnButton>().index = i;
            var button = column.GetComponent<Button>();
            button.onClick.AddListener(() => OnColumnButtonPressed?.Invoke(button.GetComponent<ColumnButton>().index));
        }
    }
}
