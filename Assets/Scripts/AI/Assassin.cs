using System;
using UnityEngine;

/// <summary>
/// L'assassin est un agent spéciale qui peut se téléporter derrière sa cible.
/// Il ne peut se téléporter deux fois de suite sur la même cible.
/// Les dégâts sont augmentés si l'assassin est dérrière sa cible.
/// </summary>
public class Assassin : Attack
{
    #region Fields

    [SerializeField] private float _dashRange = 3f;

    [SerializeField] private float _dashCooldown = 10f;

    private float _actualDashCooldown = 10f;

    [SerializeField] private float _damageMultiplier = 1.25f;

    /// <summary>
    /// Permet d'éviter un stack overflow si aucune destination n'est disponible.
    /// </summary>
    private int _dashIterations = 0;

    private TeamMember _dashedTarget;

    private NinjaAnimationsController _ninjaAnimationsController;

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

    protected override void Awake()
    {
        base.Awake();

        _ninjaAnimationsController = (NinjaAnimationsController)_animationsController;
    }

    protected override void Update()
    {
        base.Update();

        if (TargetInRange() && ChargeDash() >= _dashCooldown && !ReferenceEquals(_target, _dashedTarget) &&
            !ReferenceEquals(_target, null))
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
            return BattleManager.Instance.Agent47.CanReach(finalPosition) ? finalPosition : GetDashDestination();
        }

        return Vector3.zero;
    }

    private void Dash(Vector3 destination)
    {
        if (destination != Vector3.zero)
        {
            //  if (!ReferenceEquals(_animationsController, null)) _ninjaAnimationsController.Dash();
            _actualDashCooldown = 0f;
            _agent.AgentI.Warp(destination);
            _dashIterations = 0;
        }
        
        _dashedTarget = _target;
    }

    protected override void ApplyAttack()
    {
        if (!ReferenceEquals(_animationsController, null)) _animationsController.Attack();
        AttackProgression = 0f;

        float mult = 1.0f;
        float dot = Vector3.Dot(transform.forward, _target.transform.forward);

        if (dot < 0f)
        {
            mult = _damageMultiplier;
        }

        float damage = _target.Life.Harm(_damage * mult);
        if (damage > 0)
        {
            DamageDealt += damage;
        }
    }

    #endregion Methods
}