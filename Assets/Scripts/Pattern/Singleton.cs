using UnityEngine;

namespace Pattern
{
    public abstract class Singleton<T> : MonoBehaviour
    {
        public static T Instance { get; private set; }
        
        protected virtual void Awake()
        {
            Instance = GetComponent<T>();
        }
    }
}