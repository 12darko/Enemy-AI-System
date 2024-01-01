using UnityEngine;

namespace States
{
    public class StatesData : MonoBehaviour
    {
        
        [HideInInspector] public EnemyStates state;
        public delegate void StateChangeEvent(EnemyStates oldState,  EnemyStates newState);
        public StateChangeEvent OnStateChange;
        public EnemyStates State
        {
            get
            {
                return state;
            }
            set
            {
                OnStateChange?.Invoke(state, value);
                state = value;
            }
        }
    }
}