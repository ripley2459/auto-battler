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

    #endregion Fields

    #region Methods

    protected override void Update()
    {
        base.Update();

        if (TargetInRange() && ChargeDash() >= _dashCooldown)
        {
            Dash(GetDashDestination());
        }
    }

    private float ChargeDash()
    {
        return _actualDashCooldown += Time.deltaTime;
    }

    private Vector3 GetDashDestination()
    {
        _dashIterations++;
        Vector2 fromTarget = (transform.position - _target.transform.position).normalized;
        Vector2 perpendicular = new Vector2(-fromTarget.y, fromTarget.x);

        float angle = Random.Range(0, Mathf.PI);

        Vector2 offset = fromTarget * Mathf.Sin(angle) + perpendicular * Mathf.Cos(angle);

        Vector3 finalPosition = _target.transform.position + (Vector3)(offset * _dashRange);

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
        }
        else
        {
            // pas de destination pour l'instant donc attendre.
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