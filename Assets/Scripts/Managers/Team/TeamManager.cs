using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    #region Fields

    /// <summary>
    /// Choix de l'équipe.
    /// </summary>
    [SerializeField] private TeamList _team = TeamList.No;

    /// <summary>
    /// Limites de la zone de cette équipe.
    /// </summary>
    [SerializeField] private Bounds _bounds;

    /// <summary>
    /// Liste des prefabs qui seront instantiés au début des combats.
    /// </summary>
    [SerializeField] private List<TeamMember> _prefabs = new List<TeamMember>();

    /// <summary>
    /// Liste des combattants.
    /// </summary>
    private List<TeamMember> _members = new List<TeamMember>();

    /// <summary>
    /// Liste des ennemis.
    /// </summary>
    private List<TeamMember> _enemies = new List<TeamMember>();

    #endregion Fields

    #region Properties

    public TeamList Team => _team;

    public List<TeamMember> Members
    {
        get => _members;
    }

    public List<TeamMember> Enemies => _enemies;

    #endregion Properties

    #region Methods

    public void InitMembers()
    {
        foreach (TeamMember prefab in _prefabs)
        {
            Vector3 pos = BattleManager.Instance.GetRandomPositionInBounds(_bounds);

            TeamMember newAgent = Instantiate(prefab, pos, Quaternion.identity);

            newAgent.TeamManager = this;
            newAgent.Team = _team;
            newAgent.Attack.TeamManager = this;
            newAgent.Attack.Team = _team;
            
            _members.Add(newAgent);
        }
    }

    public void InitEnemies()
    {
        _enemies = BattleManager.Instance.GetEnemies(Team);
    }

    public void ResetMembers()
    {
        throw new NotImplementedException();
    }
    
    public bool RemoveEnemy(TeamMember enemy)
    {
        return _enemies.Remove(enemy);
    }

    public TeamMember GetNearestTarget(Transform member)
    {
        TeamMember bestTarget = null;
        float closestDist = Mathf.Infinity;
        Vector3 currentPos = member.position;

        foreach (TeamMember pTarget in _enemies)
        {
            if (pTarget.Life.IsAlive())
            {
                Vector3 directionToTarget = pTarget.transform.position - currentPos;
                float dSqrToTarget = directionToTarget.sqrMagnitude;

                if (dSqrToTarget < closestDist)
                {
                    closestDist = dSqrToTarget;
                    bestTarget = pTarget;
                }
            }
        }

        return bestTarget;
    }
    
    #endregion Methods

    public enum TeamList
    {
        Blue,
        Red,
        No
    }
}