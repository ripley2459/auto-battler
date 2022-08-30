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
    /// Distance minimale entre deux agent. Permet d'éviter des problèmes liés à la physique.
    /// </summary>
    [SerializeField] private float _minimalDistance = 1.0f;

    private List<Vector3> _usedPositions = new List<Vector3>();

    private bool _inBattle = false;

    #endregion Fields

    #region Methods

    private void Start()
    {
        InitBattle();
    }

    public void InitBattle()
    {
        if (_inBattle) return;

        _inBattle = true;
        _agent47 = Instantiate(_agent47, Vector3.up, Quaternion.identity, transform);

        foreach (TeamManager team in _teams)
        {
            team.InitMembers();
        }

        foreach (TeamManager team in _teams)
        {
            team.InitEnemies();
        }
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

    private void EndBattle()
    {
        _inBattle = false;

        foreach (TeamManager team in _teams)
        {
            team.ResetMembers();
        }

        throw new NotImplementedException();
    }

    #endregion Methods
}