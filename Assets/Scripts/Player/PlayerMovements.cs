using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private RaycastHit[] _hits = new RaycastHit[1];
    private void Update()
    {
       Move();
    }


    private void Start()
    {
        PlayerComponents.Instance.PlayerAgentLinkMover.OnLinkStart += HandleLinkStart;
        PlayerComponents.Instance.PlayerAgentLinkMover.OnLinkEnd += HandleLinkEnd;
    }

    private void HandleLinkStart()
    {
        PlayerComponents.Instance.PlayerAnimator.SetTrigger(PlayerConstData.Jump);
    }

    private void HandleLinkEnd()
    {
        PlayerComponents.Instance.PlayerAnimator.SetTrigger(PlayerConstData.Landed);
    }

    private void Move()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            var ray = PlayerComponents.Instance.PlayerCam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.RaycastNonAlloc(ray, _hits) > 0)
            {
                PlayerComponents.Instance.PlayerNavMesh.SetDestination(_hits[0].point); // Kullanıcı seçilen noktaya yürüyor
            }
        }
        
        PlayerComponents.Instance.PlayerAnimator.SetBool(PlayerConstData.IsRunning, PlayerComponents.Instance.PlayerNavMesh.velocity.magnitude > 0.01f);
    }
}
