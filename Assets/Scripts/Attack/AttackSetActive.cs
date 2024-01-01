using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSetActive : MonoBehaviour
{
     [SerializeField] private SphereCollider collider;
     
     public void ColliderSetActiveTrue()
     {
          collider.enabled = true;
     }
     
     public void ColliderSetActiveFalse()
     {
          collider.enabled = false;
     }
}
