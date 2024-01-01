using UnityEngine;

namespace ThirdPerson.ThirdEnemy
{
    [CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3;
        public float recoveryTime = 2;

        public bool canCombo;
        public EnemyAttackAction comboAction;
        
        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;

        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;
    }
}