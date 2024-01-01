using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private int _clickAmount; // canditad
    private bool _isClick; // puedo

    [SerializeField] private AttackRadius aRadius;
    private void Start()
    {
        _clickAmount = 0;
        _isClick = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InitializeCombo();
        }
    }


    private void InitializeCombo()
    {
        if (_isClick)
        {
            _clickAmount++;
        }

        if (_clickAmount == 1)
        {
              aRadius.animType = DamageAnimationType.Base;
            animator.SetInteger("Attack", 1);
        }
    }

    private void ContinueCombo()
    {
        _isClick = false;
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Left Slash") && _clickAmount == 1)
        {
            animator.SetInteger("Attack", 0);
            aRadius.animType = DamageAnimationType.Base;
            _isClick = true;
            _clickAmount = 0;
        }else if (animator.GetCurrentAnimatorStateInfo(1).IsName("Left Slash") && _clickAmount >= 2)
        {
            aRadius.animType = DamageAnimationType.Right;
            animator.SetInteger("Attack", 2);
            _isClick = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(1).IsName("Upper Slash") && _clickAmount == 2)
        {
            animator.SetInteger("Attack", 0);
            _isClick = true;
            _clickAmount = 0;
        }
        else if (animator.GetCurrentAnimatorStateInfo(1).IsName("Upper Slash") && _clickAmount >= 3)
        {
          
            aRadius.animType = DamageAnimationType.Right;
            animator.SetInteger("Attack", 3);
            _isClick = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(1).IsName("Crouching Slash"))
        {
            animator.SetInteger("Attack", 0);
            _isClick = true;
            _clickAmount = 0;
        }
    }
}

 