using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulCounterBar : MonoBehaviour
{
   public TMP_Text soulCountText;

   public void SetSoulCountText(int soulCount)
   {
       soulCountText.text = soulCount.ToString();
   }
}
