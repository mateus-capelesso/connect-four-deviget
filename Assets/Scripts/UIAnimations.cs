using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIAnimations : MonoBehaviour
{
    public float animationTime = 0.5f;
    public GameObject startButton;
    public GameObject resetButton;
    [Space(10)]
    public TextMeshProUGUI instructionsText;
    public Color textColor;

    [Header("Events")]
    public UnityEvent onStartPressedDelay;

    private void Start()
    {
        GameManager.OnWinnerDetected += SetWinText;
        GameManager.OnPlayerTurn += InstructionText;
        GameManager.OnPlayerPickColumn += HideText;
        GameManager.OnGridFilled += DrawText;
    }

    #region ButtonAnimations

    public void HideStartButton()
    {
        startButton.GetComponent<CanvasGroup>().DOFade(0f, animationTime).OnComplete(() => onStartPressedDelay?.Invoke());
    }

    public void ShowStartButton()
    {
        startButton.GetComponent<CanvasGroup>().DOFade(1f, animationTime);
    }
    
    public void HideResetButton()
    {
        resetButton.GetComponent<CanvasGroup>().DOFade(0f, animationTime);
    }

    public void ShowResetButton()
    {
        resetButton.GetComponent<CanvasGroup>().DOFade(1f, animationTime);
    }

    public void PunchResetButton()
    {
        resetButton.transform.DOPunchScale(Vector3.one * -0.1f, animationTime, 0, 0);
    }

    #endregion

    private void SetWinText(Player player)
    {
        instructionsText.text = $"Player {player.content.ToString()} won";
        instructionsText.color = player.color;
    }

    private void InstructionText()
    {
        instructionsText.DOFade(1f, 0.5f);
        instructionsText.color = textColor;
        instructionsText.text = $"Pick a column";
    }

    private void HideText()
    {
        instructionsText.DOFade(0f, 0.2f);
    }

    private void DrawText()
    {
        instructionsText.DOFade(1f, 0.5f);
        instructionsText.color = textColor;
        instructionsText.text = $"Game Draw";
    }

}
