using UnityEngine;

namespace ThirdPerson.ThirdEnemy.States
{
    public class RotateTowardsTargetState : State
    {
        public CombatStanceState combatStanceState;
 
        public override State OnUpdate(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
          enemyAnimatorManager.anim.SetFloat("Vertical", 0);
          enemyAnimatorManager.anim.SetFloat("Horizontal", 0);
          Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
          float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

          if (enemyManager.isInteracting)
              return this;
          
          
          if (viewableAngle >= 100 && viewableAngle <= 180 && !enemyManager.isInteracting)
          {
              enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
              return combatStanceState;
          }else if (viewableAngle <= -101 && viewableAngle >= -180 && !enemyManager.isInteracting)
          {
            enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
              return combatStanceState;
          }else if (viewableAngle <= -45 && viewableAngle >= -100 && !enemyManager.isInteracting)
          {
              enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Right", true);
              return combatStanceState;
          }else if (viewableAngle >= 45 && viewableAngle <= 100 && !enemyManager.isInteracting)
          {
              enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Left", true);
              Debug.Log("1");
              return combatStanceState;
          }
 
          return combatStanceState;
        }
    }
}