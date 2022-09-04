using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Attack
{
    #region Fields

    [SerializeField] private float _dashRange = 3f;
    [SerializeField] private float _dashArc = 3f;
    [SerializeField] private float _dashCooldown = 10f;
    private float _actualDashCooldown = 10f;
    [SerializeField] private float _damageMultiplier = 1.25f;
    private int _dashIterations = 0;
    private TeamMember _dashedTarget;
    
    #endregion Fields

    #region Properties

    public float DashCooldown => _dashCooldown;

    public float ActualDashCooldown
    {
        get => _actualDashCooldown;
        set
        {
            _actualDashCooldown = value;
            _onChargingDash?.Invoke();
        }
    }

    #endregion Properties

    #region Events

    private event Action _onChargingDash;

    public event Action OnChargingDash
    {
        add
        {
            _onChargingDash -= value;
            _onChargingDash += value;
        }
        remove => _onChargingDash -= value;
    }

    #endregion Events

    #region Methods

    protected override void Update()
    {
        base.Update();

        if (TargetInRange() && ChargeDash() >= _dashCooldown && !ReferenceEquals(_target, _dashedTarget))
        {
            Dash(GetDashDestination());
        }
    }

    private float ChargeDash()
    {
        return ActualDashCooldown += Time.deltaTime;
    }

    private Vector3 GetDashDestination()
    {
        // return Vector3.zero;
        _dashIterations++;

        Vector3 offset = _target.transform.position - transform.position;
        Vector3 finalPosition = _target.transform.position + offset;

        if (_dashIterations < 10)
        {
            _dashIterations = 0;
            return BattleManager.Instance.Agent47.CanReach(finalPosition) ? finalPosition : GetDashDestination();
        }

        return Vector3.zero;
    }

    private void Dash(Vector3 destination)
    {
        if (destination != Vector3.zero)
        {
            _actualDashCooldown = 0f;
            _agent.AgentI.Warp(destination);
            _dashedTarget = _target;
        }
        else
        {
            // pas de destination pour l'instant donc attendre.
            // TODO: WAIT BEFORE RETRYING
        }
    }

    protected override void ApplyAttack()
    {
        if (!ReferenceEquals(_animationsController, null)) _animationsController.Attack();
        AttackProgression = 0f;
        float damage = _target.Life.Harm(_damage);
        if (damage > 0)
        {
            DamageDealt += damage;
        }
    }

    #endregion Methods
}