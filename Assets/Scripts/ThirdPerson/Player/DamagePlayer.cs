using System;
using UnityEngine;

namespace ThirdPerson.Player
{
    public class DamagePlayer : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
               playerStats.TakeDamage(10); 
               Debug.Log(playerStats);
            }
        }
    }
}