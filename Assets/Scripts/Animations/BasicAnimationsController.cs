using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAnimationsController : MonoBehaviour
{
    #region Fields

    private Animator _animator;

    #endregion Fields

    #region Methods

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }
    
    public void SetRun()
    {
        _animator.SetBool("Run", true);
    }
    
    #endregion Methods

    public void StopRun()
    {
        _animator.SetBool("Run", false);
    }
}
