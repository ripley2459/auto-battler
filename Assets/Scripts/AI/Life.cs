using System;
using UnityEngine;

public class Life : MonoBehaviour
{
    #region Fields

    [SerializeField] private float _maxLife = 50f;

    [SerializeField] private float _actualLife = 50f;

    private TeamMember _teamMember;

    private Armor _armor;

    #endregion Fields

    #region Properties

    public float MaxLife
    {
        get => _maxLife;
        set => _maxLife = value;
    }

    public float ActualLife
    {
        get => _actualLife;
        set
        {
            _actualLife = value;
            _onLifeChanged?.Invoke();
        }
    }

    #endregion Properties

    #region Events

    private event Action _onLifeChanged;

    public event Action OnLifeChanged
    {
        add
        {
            _onLifeChanged -= value;
            _onLifeChanged += value;
        }
        remove => _onLifeChanged -= value;
    }

    #endregion Events

    #region Methods

    private void Awake()
    {
        OnLifeChanged += Death;

        _teamMember = GetComponent<TeamMember>();
        _armor = GetComponent<Armor>();
    }

    private void Start()
    {
        ActualLife = MaxLife;
    }

    public float Harm(float damage)
    {
        if (!ReferenceEquals(_armor, null)) damage = _armor.reduceDamage(damage);
        ActualLife -= damage;
        return damage;
    }

    public bool IsAlive()
    {
        return ActualLife > 0f;
    }

    private void Death()
    {
        if (!IsAlive())
        {
            BattleManager.Instance.ApplyDeath(_teamMember);
        }
    }

    #endregion Methods
}