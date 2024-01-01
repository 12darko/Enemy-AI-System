using System;
using System.Collections;
using System.Collections.Generic;
using ThirdPerson.Player;
using UnityEngine;

public class PlayerAnimationManager : AnimatorManager
{
 
   [SerializeField] private bool isCanRotate;
   [SerializeField] private InputHandler inputHandler;
   [SerializeField] private PlayerLocomotion playerLocomotion;
   [SerializeField] private PlayerManager playerManager;

   private PlayerStats _playerStats;
   private int vertical;
   private int horizontal;

   #region Props

   public Animator Anim
   {
      get => anim;
      set => anim = value;
   }

   public bool IsCanRotate
   {
      get => isCanRotate;
      set => isCanRotate = value;
   }

   #endregion


   public void Initialize()
   {
      anim = GetComponent<Animator>();
      _playerStats = GetComponentInParent<PlayerStats>();
      vertical = Animator.StringToHash("Vertical");
      horizontal = Animator.StringToHash("Horizontal");
   }

   public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
   {
      #region Vertical
      float v = 0;
      if (verticalMovement > 0 && verticalMovement < 0.55f)
      {
         v = 0.5f;
      }else if (verticalMovement > 0.55f)
      {
         v = 1;
      }
      else if (verticalMovement < 0 && verticalMovement > -0.55)
      {
         v = -0.5f;
      }
      else if (verticalMovement < -0.55f)
      {
         v = -1;
      }
      else
      {
         v = 0;
      }
      #endregion

      #region Horizontal

      float h = 0;
      
      if (horizontalMovement > 0 && horizontalMovement < 0.55f)
      {
         h = 0.5f;
      }
      else if (horizontalMovement > 0.55f)
      {
         h = 1;
      } else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
      {
         h = -0.5f;
      }else if (horizontalMovement < -0.55f)
      {
         h = -1f;
      }
      else
      {
         h = 0;
      }
      

      #endregion

      if (isSprinting)
      {
         v = 2;
         h = horizontalMovement;
      }
      
      anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
      anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
   }
   
   public void CanRotate()
   {
      isCanRotate = true;
   }
   
   public void StopRotation()
   {
      isCanRotate = false;
   }

   public void EnableCombo()
   {
      anim.SetBool("canDoCombo", true);
   }

   public void DisableCombo()
   {
      anim.SetBool("canDoCombo", false);
   }

   public void EnableIsInvulnerable()
   {
      anim.SetBool("isInvulnerable", true);
   }
   public void DisableIsInvulnerable()
   {
      anim.SetBool("isInvulnerable", false);
   }

   public void EnableIsParrying()
   {
      playerManager.isParrying = true;
   }

   public void DisableIsParrying()
   {
      playerManager.isParrying = false;
   }

   public void EnableCanBeRiposted()
   {
      playerManager.canBeRiposted = true;
   } 
   public void DisableCanBeRiposted()
   {
      playerManager.canBeRiposted = false;
   }
   
   public override void TakeCriticalDamageAnimationEvent()
   {
      _playerStats.TakeDamageNoAnimation(playerManager.pendingCriticalDamage);
      playerManager.pendingCriticalDamage = 0;
   }

   private void OnAnimatorMove()
   {
      if (playerManager.IsInteracting == false)
         return;

      float delta = Time.deltaTime;
      playerLocomotion.Rigidbody.drag = 0;
      Vector3 deltaPosition = anim.deltaPosition;
      deltaPosition.y = 0;
      Vector3 velocity = deltaPosition / delta;
      playerLocomotion.Rigidbody.velocity = velocity;
   }
  
}
