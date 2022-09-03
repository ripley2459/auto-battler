using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaAnimationsController : BasicAnimationsController
{
    public void Dash()
    {
        _animator.SetTrigger("Dash");
    }
}
