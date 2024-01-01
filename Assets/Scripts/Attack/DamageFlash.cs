using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
   [SerializeField] private SkinnedMeshRenderer enemyRenderer;
   [SerializeField] private Color originEnemyColor;
   [SerializeField] private Color changeColor;
   [SerializeField] private float flashTime = .1f;


   private void Awake()
   {  
      enemyRenderer = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
      originEnemyColor = enemyRenderer.material.color;
      ColorUtility.TryParseHtmlString("#23D8FD", out changeColor);
      changeColor.a = .35f;
      enemyRenderer.material.SetFloat("_Metallic", .5f);
      enemyRenderer.material.SetFloat("_Glossiness", .8f);
   }

 

   #region MyRegion
   public void DamageFlashStart()
   {
      enemyRenderer.material.color = changeColor;
      Invoke("DamageFlashEnd", flashTime);
   }

   public void DamageFlashEnd()
   {
      enemyRenderer.material.color = originEnemyColor;
   }
   

   #endregion
   
   public IEnumerator DamagesFlash()
   {
      enemyRenderer.material.SetFloat("_Mode", 3);
      enemyRenderer.material.color = changeColor;
      yield return new WaitForSeconds(flashTime);
      enemyRenderer.material.color = originEnemyColor;
      enemyRenderer.material.SetFloat("_Mode", 0);
   }
}
