using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : Singleton<BattleManager>
{
    #region Fields

    [SerializeField] private Agent _agent47;

    /// <summary>
    /// Liste des équipes participantes à la bataille.
    /// </summary>
    [SerializeField] private List<TeamManager> _teams = new List<TeamManager>();

    /// <summary>
    /// Liste des équipes n'ayant pas encore perdues.
    /// </summary>
    [SerializeField]private List<TeamManager> _actualTeams = new List<TeamManager>();
    
    /// <summary>
    /// Distance minimale entre deux agent. Permet d'éviter des problèmes liés à la physique.
    /// </summary>
    [SerializeField] private float _minimalDistance = 1.0f;

    private List<Vector3> _usedPositions = new List<Vector3>();

    private bool _inBattle = false;

    [SerializeField] private GameObject _startButton;

    #endregion Fields

    #region Events

    private event Action _onPreInit;

    public event Action OnPreInit
    {
        add
        {
            _onPreInit -= value;
            _onPreInit += value;
        }
        remove => _onPreInit -= value;
    }

    private event Action _onInit;

    public event Action OnInit
    {
        add
        {
            _onInit -= value;
            _onInit += value;
        }
        remove => _onInit -= value;
    }

    private event Action _onPostInit;

    public event Action OnPostInit
    {
        add
        {
            _onPostInit -= value;
            _onPostInit += value;
        }
        remove => _onPostInit -= value;
    }

    private event Action<TeamMember> _notifyDeath;

    public event Action<TeamMember> NotifyDeath
    {
        add
        {
            _notifyDeath -= value;
            _notifyDeath += value;
        }
        remove => _notifyDeath -= value;
    }

    private event Action _onEndBattle;

    public event Action OnEndbattle
    {
        add
        {
            _onEndBattle -= value;
            _onEndBattle += value;
        }
        remove => _onEndBattle -= value;
    }
    
    #endregion Events

    #region Methods

    private void Start()
    {
        _agent47 = Instantiate(_agent47, Vector3.up, Quaternion.identity, transform);
    }

    public void InitBattle()
    {
        if (_inBattle) return;

        _actualTeams = new List<TeamManager>(_teams);
        
        _startButton.SetActive(false);

        _inBattle = true;

        _onPreInit?.Invoke();
        _onInit?.Invoke();
        _onPostInit?.Invoke();
    }

    public Vector3 GetRandomPositionInBounds(Bounds bounds)
    {
        float rad = _agent47.Radius;

        float x = Random.Range(bounds.min.x + rad, bounds.max.x - rad);
        float z = Random.Range(bounds.min.z + rad, bounds.max.z - rad);

        Vector3 pos = new Vector3(x, 0, z);

        bool flag = true;
        foreach (Vector3 pos2 in _usedPositions)
        {
            if (Vector3.Distance(pos, pos2) < _minimalDistance)
            {
                flag = false;
                break;
            }
        }

        if (_agent47.CanReach(pos) && flag)
        {
            _usedPositions.Add(pos);
            return pos;
        }

        return GetRandomPositionInBounds(bounds);
    }

    public List<TeamMember> GetEnemies(TeamManager.TeamList ally)
    {
        if (ally == TeamManager.TeamList.No)
        {
            throw new ArgumentException("Can't get the opposite team.");
        }

        List<TeamMember> enemies = new List<TeamMember>();

        foreach (TeamManager team in _teams)
        {
            if (team.Team != ally)
            {
                enemies.AddRange(team.Members);
            }
        }

        return enemies;
    }

    public void ApplyDeath(TeamMember member)
    {
        _notifyDeath?.Invoke(member);
        Destroy(member.gameObject);
    }

    public void ApplyLoose(TeamManager team)
    {
        _actualTeams.Remove(team);

        if (_actualTeams.Count == 1)
        {
            EndBattle();
        }
    }
    
    private void EndBattle()
    {
        _onEndBattle?.Invoke();
        _actualTeams.Clear();
        
        _startButton.SetActive(true);
        _inBattle = false;
    }

    #endregion Methods
}