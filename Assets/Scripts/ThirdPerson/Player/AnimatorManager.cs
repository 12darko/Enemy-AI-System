﻿using UnityEngine;

namespace ThirdPerson.Player
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;
        public bool canRotate;
        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("canRotate", canRotate);
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }


        public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isRotatingWithRootMotion", true);
            anim.SetBool("isInteracting",isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }
        
        public virtual void TakeCriticalDamageAnimationEvent()
        {
            
        }
    }
}