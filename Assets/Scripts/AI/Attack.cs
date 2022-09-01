using System;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    #region Fields

    private TeamManager _teamManager;

    private Agent _agent;

    [SerializeField] private float _damage = 5f;

    [SerializeField] private float _attackRate = 0.5f;

    [SerializeField] private float _attackProgression;

    [SerializeField] private float _attackRange = 1.0f;

    [SerializeField] private bool _debugLink = false;

    private float _damageDealt;

    private TeamMember _target;

    private TeamManager.TeamList _team = TeamManager.TeamList.No;

    private LineRenderer _line;

    #endregion Fields

    #region Events

    private event Action _onAttack;

    public event Action OnAttack
    {
        add
        {
            _onAttack -= value;
            _onAttack += value;
        }
        remove => _onAttack -= value;
    }

    private event Action _onDamageDealt;

    public event Action OnDamageDealt
    {
        add
        {
            _onDamageDealt -= value;
            _onDamageDealt += value;
        }
        remove => _onDamageDealt -= value;
    }

    private event Action _onChargingAttack;

    public event Action OnChargingAttack
    {
        add
        {
            _onChargingAttack -= value;
            _onChargingAttack += value;
        }
        remove => _onChargingAttack -= value;
    }

    #endregion Events

    #region Properties

    public float AttackProgression
    {
        get => _attackProgression;
        set
        {
            _attackProgression = value;
            _onChargingAttack?.Invoke();
        }
    }

    public float DamageDealt
    {
        get => _damageDealt;
        set
        {
            _damageDealt = value;
            _onDamageDealt?.Invoke();
        }
    }

    public TeamManager TeamManager
    {
        get => _teamManager;
        set => _teamManager = value;
    }

    public TeamManager.TeamList Team
    {
        get => _team;
        set => _team = value;
    }

    #endregion Properties

    #region Methods

    private void Awake()
    {
        _agent = GetComponent<Agent>();
        _line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        AttackProgression = 0f;
        _attackRange = Mathf.Clamp(_attackRange, 0f, Mathf.Infinity);
        _agent.AgentI.stoppingDistance = _attackRange;
    }

    private void Update()
    {
        GetNewTarget();
        RotateToTarget();

        if (_debugLink)
        {
            _line.SetPosition(0, transform.position);
            _line.SetPosition(1, _target.transform.position);
        }
        else
        {
            _line.SetPosition(0, Vector3.zero);
            _line.SetPosition(1, Vector3.zero);
        }

        if (TargetInRange())
        {
            _agent.StopMoving();

            if (ChargeAttack() >= 1.0f)
            {
                AttackProgression = 0f;

                float damage = _target.Life.Harm(_damage);
                if (damage > 0)
                {
                    DamageDealt += damage;
                }
            }
        }
    }

    /// <summary>
    /// Prendre l'agent le plus proche comme nouvelle cible. Abandonne donc l'ancienne.
    /// </summary>
    private void GetNewTarget()
    {
        _target = _teamManager.GetNearestTarget(transform);
        _agent.SetDestination(_target.transform.position);
    }

    private float ChargeAttack()
    {
        return AttackProgression += Time.deltaTime * _attackRate;
    }

    private void RotateToTarget()
    {
        Quaternion lookRotation = Quaternion.LookRotation(_target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,
            Time.deltaTime * 120 /* TODO: use var angular rotation */);
    }

    #region TargetSelectionMethods

    #endregion TargetSelectionMethods

    private bool TargetInRange()
    {
        return Vector3.Distance(transform.position, _target.transform.position) <= _attackRange;
    }

    #endregion Methods
}