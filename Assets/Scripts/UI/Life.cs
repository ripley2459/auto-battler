using System;
using UnityEngine;

public class Life : MonoBehaviour
{
    #region Fields

    [SerializeField] private float _maxLife = 50f;
    
    [SerializeField] private float _actualLife = 50f;

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
    }

    private void Start()
    {
        ActualLife = MaxLife;
    }

    public float Harm(float damage)
    {
        ActualLife -= damage;
        // Possibilité d'ajouter de l'armure et de renvoyer les dégâts réellement infligés.
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
            gameObject.SetActive(false);
            // TODO: Notify managers this one is dead to update lists and check if batle is ended.
        }
    }

    #endregion Methods
}