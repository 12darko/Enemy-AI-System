using UnityEngine;
using UnityEngine.Events;

namespace ThirdPerson.ThirdEnemy.States
{
    public abstract class State : MonoBehaviour
    {
   
        public abstract State OnUpdate(EnemyManager enemyManager, EnemyStats enemyStats,
            EnemyAnimatorManager enemyAnimatorManager);
    }
}