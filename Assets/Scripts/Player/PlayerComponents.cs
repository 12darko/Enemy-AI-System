using System.Collections;
using System.Collections.Generic;
using Pattern;
using UnityEngine;
using UnityEngine.AI;

public class PlayerComponents : Singleton<PlayerComponents>
{
    [Header("Components")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private NavMeshAgent playerNavMesh;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AgentLinkMover playerAgentLinkMover;

    #region Props

    public Camera PlayerCam => playerCam;
    public NavMeshAgent PlayerNavMesh => playerNavMesh;
    public Animator PlayerAnimator => playerAnimator;

    public AgentLinkMover PlayerAgentLinkMover
    {
        get => playerAgentLinkMover;
        set => playerAgentLinkMover = value;
    }

    #endregion

}
