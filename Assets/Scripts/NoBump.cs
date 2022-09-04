using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ce script permet d'empécher les RigidBody de prendre de la vitesse quand ils se rentrent dedans (pouvant créer des situations étranges).
/// Le système de nav mesh n'utilise pas de force pour déplacer les agents
/// et comme la physique n'est pas importante dans ce projet on peut donc se permettre de les "fixer".
/// </summary>
public class NoBump : MonoBehaviour
{
    private Rigidbody _rb;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rb.velocity = Vector3.zero;
    }
}
