using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public class EnemyData : MonoBehaviour
    {

        [Header("Classes")] 
        [SerializeField] private EnemyMovement enemyMovement;
        [SerializeField] private EnemyPatrol enemyPatrol;
        [SerializeField] private EnemyIdle enemyIdle;
        
        [Header("Component")]
        [SerializeField] private NavMeshAgent enemyAgent;
        [SerializeField] private Vector3[] wayPoints = new Vector3[4];
        [SerializeField] private Vector3 idleStartPosition;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private AgentLinkMover enemyAgentLinkMover;
        [SerializeField] private Animator enemyAnimator;
       
        
        [Header("Normal Variables")]
        [SerializeField] private  int wayPointsIndex = 0;
        [SerializeField] private float updateRate = 0.01f;
        [SerializeField] private float idleLocationRadius = 4f;
        [SerializeField] private float idleMoveSpeedMultiplier = 0.5f;
        [SerializeField] private bool enemyIsAlive;

        
        //Public Props
        public Coroutine FollowCoroutine;
        
        #region Props
        
        //Classes
        public EnemyMovement EnemyMovement
        {
            get => enemyMovement;
            set => enemyMovement = value;
        }

        public EnemyPatrol EnemyPatrol
        {
            get => enemyPatrol;
            set => enemyPatrol = value;
        }

        public EnemyIdle EnemyIdle
        {
            get => enemyIdle;
            set => enemyIdle = value;
        }


        //Component
        public NavMeshAgent EnemyAgent
        {
            get => enemyAgent;
            set => enemyAgent = value;
        }


        public Vector3[] WayPoints
        {
            get => wayPoints;
            set => wayPoints = value;
        }

        public Vector3 IdleStartPosition
        {
            get => idleStartPosition;
            set => idleStartPosition = value;
        }

        public Transform PlayerTransform
        {
            get => playerTransform;
            set => playerTransform = value;
        }

        public AgentLinkMover EnemyAgentLinkMover
        {
            get => enemyAgentLinkMover;
            set => enemyAgentLinkMover = value;
        }

        public Animator EnemyAnimator
        {
            get => enemyAnimator;
            set => enemyAnimator = value;
        }


        //Normal Variables
        public int WayPointsIndex
        {
            get => wayPointsIndex;
            set => wayPointsIndex = value;
        }

        public float UpdateRate
        {
            get => updateRate;
            set => updateRate = value;
        }
        public float IdleLocationRadius
        {
            get => idleLocationRadius;
            set => idleLocationRadius = value;
        }

        public float IdleMoveSpeedMultiplier
        {
            get => idleMoveSpeedMultiplier;
            set => idleMoveSpeedMultiplier = value;
        }

        public bool EnemyIsAlive
        {
            get => enemyIsAlive;
            set => enemyIsAlive = value;
        }

        #endregion
    
    }
}