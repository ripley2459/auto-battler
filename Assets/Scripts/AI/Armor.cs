using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    #region Fields

    [SerializeField] private float _damageReduction = 0.5f;

    #endregion Fields
    
    #region Properties

    public float reduceDamage(float damage)
    {
        return damage * _damageReduction;
    }
    
    #endregion Properties

    #region Methods

    private void Start()
    {
        if (_damageReduction == 0f)
        {
            Debug.LogError(gameObject.name + "can't be damaged. Damage reduction= " + _damageReduction);
        }
    }

    #endregion Methods
}
