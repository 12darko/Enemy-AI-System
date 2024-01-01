using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float interactRadius;
    [SerializeField] private string interactableText;


    public float InteractRadius => interactRadius;

    public string InteractableText => interactableText;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }

    public virtual void Interact(PlayerManager playerManager)
    {
        Debug.Log("Bir itemle etkileşime girdiniz");
    }
}
