using UnityEngine;
using UnityEngine.UI;

public class ColumnButton : MonoBehaviour
{
    public int index;

    private void Start()
    {
        GameManager.OnPlayerTurn += EnableButton;
        GameManager.OnDisableButton += DisableButtons;
    }

    private void DisableButtons()
    {
        GetComponent<Button>().interactable = false;
    }

    private void EnableButton()
    {
        GetComponent<Button>().interactable = true;
    }
}