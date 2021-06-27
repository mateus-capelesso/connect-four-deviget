using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Slot: MonoBehaviour
{
     private SlotContent _slotContent = SlotContent.Void;
     public Image imageFill;
     
     public SlotContent SlotContent
     {
          get => _slotContent;
          set
          {
               if (_slotContent != SlotContent.Void) return;
               
               _slotContent = value;
          }
     }

     public void SetSlotContent(SlotContent content, Color color)
     {
          SlotContent = content;

          var position = transform.position;
          var dif = 470 - position.y;
          transform.position = new Vector3(position.x, position.y + dif, position.z);
          transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
          {
               transform.DOMove(position, 0.5f).SetEase(Ease.InExpo);
          });
          ChangeColor(color);
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