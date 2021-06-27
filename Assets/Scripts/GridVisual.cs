using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class GridVisual : MonoBehaviour
{
    public CanvasGroup canvas;
    public UnityEvent onGridHide;

    private void Start()
    {
        GameManager.OnWinDetected += ShowWinningPieces;
    }

    private void ShowWinningPieces(List<Slot> slots)
    {
        var allSlots = GetComponent<GridSlots>().SlotGrid;
        foreach (var slot in allSlots)
        {
            if(!slots.Contains(slot))
                slot.ScaleSlotDown();
        }
    }
    public void ShowCanvasGroup(float time = 1f)
    {
        canvas.DOFade(1f, time);
    }
    
    public void HideCanvas(float time = 1f)
    {
        canvas.DOFade(0f, time);
    }

    public void BlinkCanvas()
    {
        canvas.DOFade(0f, 0.5f).OnComplete(() =>
        {
            onGridHide?.Invoke();
            ShowCanvasGroup(0.5f);
        });
    }
}
