using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Slot: MonoBehaviour
{
     private SlotContent _slotContent = SlotContent.Void;
     public Image imageFill;
     [HideInInspector]
     public float referenceY;
     
     public SlotContent SlotContent
     {
          get => _slotContent;
          set
          {
               if (_slotContent != SlotContent.Void) return;
               
               _slotContent = value;
          }
     }

     public void SetSlotContent(Player player, Action callback)
     {
          SlotContent = player.content;

          var position = GetComponent<RectTransform>().anchoredPosition;
          GetComponent<RectTransform>().anchoredPosition = new Vector2(position.x, referenceY);
          GetComponent<RectTransform>().DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
          {
               GetComponent<RectTransform>().DOAnchorPosY(position.y, 0.5f).SetEase(Ease.InExpo).OnComplete(() => callback?.Invoke());
          });
          ChangeColor(player.color);
     }

     private void Start()
     {
          Clear();
     }

     private void ChangeColor(Color color)
     {
          imageFill.color = color;
     }

     public void ScaleSlotUp()
     {
          transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBack);
     }
     
     public void ScaleSlotDown()
     {
          transform.DOScale(Vector3.one * 0.25f, 0.5f).SetEase(Ease.OutBack);
     }

     public void Clear()
     {
          transform.localScale = Vector3.zero;
          _slotContent = SlotContent.Void;
          imageFill.color = Color.white;
     }
}