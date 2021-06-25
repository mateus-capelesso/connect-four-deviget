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
               ChangeColor();
          }
     }

     private void Start()
     {
          if (_slotContent == SlotContent.Void)
          {
               imageFill.color = Color.white;
          }
     }

     private void ChangeColor()
     {
          switch (_slotContent)
          {
               case SlotContent.Red:
                    imageFill.color = Color.red;
                    break;
               case SlotContent.Yellow:
                    imageFill.color = Color.yellow;
                    break;
          }
     }
}