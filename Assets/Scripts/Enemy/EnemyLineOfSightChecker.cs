using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyLineOfSightChecker : MonoBehaviour
    {
        public SphereCollider sightCollider;
        public float sightFieldOfView = 90f;
        public LayerMask sightLayers;
        public EnemyData enemyData;

        public delegate void GainSightEvent(Players player);

        public GainSightEvent OnGainSight;

        public delegate void LoseSightEvent(Players player);

        public LoseSightEvent OnLoseSight;


        private Coroutine _checkForLineOfSightCoroutine;

        private void Awake()
        {
            sightCollider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (enemyData.EnemyIsAlive)
            {
                Players player;
                if (other.TryGetComponent<Players>(out player))
                {
                    if (!CheckLineOfSight(player))
                    {
                        _checkForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(player));
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Players player;
            if (other.TryGetComponent<Players>(out player))
            {
                OnLoseSight?.Invoke(player);
                if (_checkForLineOfSightCoroutine != null)
                {
                    StopCoroutine(_checkForLineOfSightCoroutine);
                }
            }
        }


        private bool CheckLineOfSight(Players player)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            float DotProduct = Vector3.Dot(transform.forward, direction);
            if (DotProduct >= Mathf.Cos(sightFieldOfView))
            {
                Debug.Log(DotProduct);
                Debug.Log(Mathf.Cos(sightFieldOfView) + "Cos");
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit, sightCollider.radius, sightLayers))
                {
                    if (hit.transform.GetComponent<Players>() != null)
                    {
                        OnGainSight?.Invoke(player);
                        return true;
                    }
                }
            }

            return false;
        }


        private IEnumerator CheckForLineOfSight(Players player)
        {
            WaitForSeconds wait = new WaitForSeconds(0.1f);
            while (!CheckLineOfSight(player))
            {
                yield return wait;
            }
        }
    }
}